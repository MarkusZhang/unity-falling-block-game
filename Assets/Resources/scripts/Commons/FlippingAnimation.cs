using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingAnimation : MonoBehaviour
{
	public float flipSpeed;
	public bool flipOnStart = true;
	
	private float originalXScale;
	
	// Use this for initialization
	void Start ()
	{
		originalXScale = transform.localScale.x;
		if (flipOnStart)
		{
			StartCoroutine("flip");
		}
	}

	IEnumerator flip()
	{
		var sign = -1;
		while (true)
		{
			var newX = transform.localScale.x + sign * flipSpeed * Time.deltaTime;
			transform.localScale = new Vector3(newX,transform.localScale.y,transform.localScale.z);

			if (newX <= 0)
			{
				sign = 1;
			}
			if (newX >= originalXScale)
			{
				sign = -1;
			}
			yield return null;
		}
	}

	public void StartFlipping()
	{
		StartCoroutine("flip");
	}

	public void StopFlipping()
	{
		StopCoroutine("flip");
		transform.localScale = new Vector3(originalXScale,transform.localScale.y,transform.localScale.z);
	}
}
