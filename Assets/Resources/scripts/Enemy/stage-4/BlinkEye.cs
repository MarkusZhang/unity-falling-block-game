using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEye : MonoBehaviour
{

	public float minOpacity = 0;
	public float maxOpacity = 1;
	public float blinkSpeed = 1;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(blink());
	}

	IEnumerator blink()
	{
		Utils.SetAlphaValue(gameObject,minOpacity);
		var alphaDir = 1; // increase alpha first
		var alpha = minOpacity;
		while (true)
		{
			alpha += alphaDir * Time.deltaTime * blinkSpeed;
			Utils.SetAlphaValue(gameObject,alpha);
			if (alpha >= maxOpacity || alpha <= minOpacity)
			{
				alphaDir = -alphaDir;
			}

			yield return null;
		}
	}
}
