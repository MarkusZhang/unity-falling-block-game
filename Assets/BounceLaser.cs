using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLaser : MonoBehaviour
{

	public GameObject selfPrefab;
	public float speed = 5;

	public bool bounceLeft;
	public bool bounceRight;
	public bool bounceTop;
	public bool bounceBottom;

	public int bounceLimit = -1; // number of times laser can bounce, -1 means infinite

	public Vector3 additionalMove = Vector3.zero;

	private bool nextLaserGened;
	private bool isLeftward;
	private bool isUpward;

	private float deltaX; // half width of the projection onto x axis
	private float deltaY; // half height of the projection onto y axis
	
	// Use this for initialization
	void Start ()
	{
		isLeftward = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) > 0;
		isUpward = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) > 0;
		var height = GetComponent<SpriteRenderer>().bounds.size.y;
		deltaX = height/2 * Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
		deltaY = Mathf.Abs(height/2 * Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
		StartCoroutine(run());
	}

	IEnumerator run()
	{
		while (true)
		{
			transform.Translate(Vector3.up * speed * Time.deltaTime);
			transform.position = transform.position + additionalMove;
			
			// check whether we already touches the top
			float screenHalfHeight = Camera.main.orthographicSize;
			float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
			var isOverTop = transform.position.y + deltaY >= screenHalfHeight;

			if (bounceLimit == 0)
			{
				yield return null;
				continue;
			}
			// if it touches left wall
			if (bounceLeft && isLeftward && transform.position.x - deltaX <= -screenHalfWidth && !nextLaserGened && !isOverTop)
			{
				var newLaserPos = new Vector3(-screenHalfWidth - deltaX, transform.position.y, transform.position.z);
				var newLaserRot = Quaternion.Euler(new Vector3(0,0,360-transform.rotation.eulerAngles.z));
				var newLaser = Instantiate(selfPrefab, newLaserPos, newLaserRot);
				setBounceLimit(newLaser);
				nextLaserGened = true; // mark this so that we don't generate laser again
			}
			// if it touches the right wall
			else if (bounceRight && !isLeftward && transform.position.x - deltaX >= screenHalfWidth && !nextLaserGened && !isOverTop)
			{
				var newLaserPos = new Vector3(screenHalfWidth - deltaX, transform.position.y, transform.position.z);		
				var newLaserRot = Quaternion.Euler(new Vector3(0,0,360-transform.rotation.eulerAngles.z));
				var newLaser = Instantiate(selfPrefab, newLaserPos, newLaserRot);
				setBounceLimit(newLaser);
				nextLaserGened = true; // mark this so that we don't generate laser again
			}
			// if it touches the ceiling
			else if (bounceTop && isUpward && transform.position.y + deltaY >= screenHalfHeight && !nextLaserGened)
			{
				var newLaserPos = new Vector3(transform.position.x,screenHalfHeight + deltaY);
				var newLaserRot = Quaternion.Euler(new Vector3(0, 0, 180 - transform.rotation.eulerAngles.z));
				var newLaser = Instantiate(selfPrefab, newLaserPos, newLaserRot);
				setBounceLimit(newLaser);
				nextLaserGened = true;
			}
			// if it touches the bottom
			else if (bounceBottom && !isUpward && transform.position.y - deltaY <= -screenHalfHeight && !nextLaserGened)
			{
				var newLaserPos = new Vector3(transform.position.x,-screenHalfHeight - deltaY);
				var newLaserRot = Quaternion.Euler(new Vector3(0, 0, 180 - transform.rotation.eulerAngles.z));
				var newLaser = Instantiate(selfPrefab, newLaserPos, newLaserRot);
				setBounceLimit(newLaser);
				nextLaserGened = true;
			}
			yield return null;
		}
	}

	void setBounceLimit(GameObject newLaserObj)
	{
		if (bounceLimit > 0)
		{
			newLaserObj.GetComponent<BounceLaser>().bounceLimit = bounceLimit - 1;
		}
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
