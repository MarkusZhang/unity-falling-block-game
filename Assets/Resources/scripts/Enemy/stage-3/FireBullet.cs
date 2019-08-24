using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FireBullet is a bullet that expands, but disappears after a while
public class FireBullet : MonoBehaviour
{
	public float moveSpeed;
	public float fadeOutSpeed;
	public float expandSpeed;
	public float destroyAlphaThreshold; // destroy the object when opacity drops below this
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(fadeOut());
		StartCoroutine(expand());
	}

	void Update()
	{
		transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
	}

	IEnumerator fadeOut()
	{
		var alpha = GetComponent<SpriteRenderer>().color.a;  
		while (alpha> destroyAlphaThreshold)
		{
			alpha -= Time.deltaTime * fadeOutSpeed;
			Utils.SetAlphaValue(gameObject,alpha);
			yield return null;
		}
		Destroy(gameObject);
	}

	IEnumerator expand()
	{
		while (true)
		{
			transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
