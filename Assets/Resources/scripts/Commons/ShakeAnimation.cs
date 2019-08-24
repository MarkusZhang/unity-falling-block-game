using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
	public float shakeAngle;
	public float shakeInterval;

	public bool autoStart;
	
	// Use this for initialization
	void Start ()
	{
		if (autoStart)
		{
			StartAnimation();
		}
		
	}

	public void StartAnimation()
	{
		StartCoroutine(shake());
	}

	IEnumerator shake()
	{
		while (true)
		{
			transform.eulerAngles = Vector3.forward * shakeAngle;
			yield return new WaitForSeconds(shakeInterval);
			transform.eulerAngles = - Vector3.forward * shakeAngle;
			yield return new WaitForSeconds(shakeInterval);
		}
	}
}
