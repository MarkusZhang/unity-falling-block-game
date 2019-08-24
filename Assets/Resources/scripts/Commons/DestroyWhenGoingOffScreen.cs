using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// destory the game object itself after it enters the screen and then goes off
public class DestroyWhenGoingOffScreen : MonoBehaviour
{
	private bool isInScreen = false;
	public bool onlyOffBottom; // only destroy when going off screen bottom
	public event System.Action OnDestoryOffScreen;
	public int timeout; // destroy the object after times out

	private float startTime;
	
	void Start()
	{
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isInScreen && !Utils.IsOffScreen2D(transform.position))
		{
			isInScreen = true;
		}
		
		if (isInScreen && Utils.IsOffScreen2D(transform.position))
		{
			if (onlyOffBottom && !Utils.IsOffBottom(transform.position))
			{
				return;
			}
			
			if (OnDestoryOffScreen != null)
			{
				OnDestoryOffScreen();
			}
			Destroy(gameObject);
		}

		if (timeout>0 && Time.time - startTime >= timeout)
		{
			Destroy(gameObject);
		}
	}

	private void OnBecameInvisible()
	{
		if (!onlyOffBottom && isInScreen)
		{
			Destroy(gameObject);
		}
	}
}
