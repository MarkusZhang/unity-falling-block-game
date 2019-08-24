using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SoundWave : MonoBehaviour
{

	public float stayTime;
	public Vector2 expandSpeed;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(expandWave());
	}

	IEnumerator expandWave()
	{
		var startTime = Time.time;
		while (Time.time - startTime < stayTime)
		{
			var newX = transform.localScale.x + expandSpeed.x * Time.deltaTime;
			var newY = transform.localScale.y + expandSpeed.y * Time.deltaTime;
			transform.localScale = new Vector3(newX,newY,1);
			
			// update collider
			GetComponent<Collider2D>().transform.localScale = transform.localScale;

			yield return null;
		}
		
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "player:bullet")
		{
			Destroy(other.gameObject);
		}
	}
}
