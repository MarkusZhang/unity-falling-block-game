using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// squeeze, expand animation
public class SqeExpAnimation : MonoBehaviour
{

	public float speed;
	public Vector2 xRange;
	public Vector2 yRange;

	public bool autoStart;
	
	// Use this for initialization
	void Start () {
		if (autoStart)
		{
			doNextAction();
		}
	}
	
	// Update is called once per frame
	void doNextAction()
	{
		var randVal = Random.Range(0, 1f);
		if (randVal > 0.5f)
		{
			StartCoroutine(SqeExpX());
		}
		else
		{
			StartCoroutine(SqeExpY());
		}
	}

	IEnumerator SqeExpX()
	{
		var xMin = xRange.x;
		var xMax = xRange.y;
		// squeeze
		while (transform.localScale.x > xMin)
		{
			var newX = transform.localScale.x - Time.deltaTime * speed;
			transform.localScale = new Vector3(newX,transform.localScale.y,1);
			yield return null;
		}
		
		// expand
		while (transform.localScale.x < xMax)
		{
			var newX = transform.localScale.x + Time.deltaTime * speed;
			transform.localScale = new Vector3(newX,transform.localScale.y,1);
			yield return null;
		}
		
		doNextAction();
	}

	IEnumerator SqeExpY()
	{
		var yMin = yRange.x;
		var yMax = yRange.y;
		// squeeze
		while (transform.localScale.y > yMin)
		{
			var newY = transform.localScale.y - Time.deltaTime * speed;
			transform.localScale = new Vector3(transform.localScale.x,newY,1);
			yield return null;
		}
		
		// expand
		while (transform.localScale.y < yMax)
		{
			var newY = transform.localScale.y + Time.deltaTime * speed;
			transform.localScale = new Vector3(transform.localScale.x,newY,1);
			yield return null;
		}
		doNextAction();
	}
}
