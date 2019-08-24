using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBullet : MonoBehaviour
{

	public Sprite[] sprites;
	public float switchInterval = 0.1f;
	public float stayTime = 1f;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(animate());
	}

	IEnumerator animate()
	{
		var startTime = Time.time;
		var spriteIndex = 0;
		while (Time.time - startTime <= stayTime)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
			spriteIndex = (spriteIndex + 1) % sprites.Length;
			yield return new WaitForSeconds(switchInterval);
		}
		
		Destroy(gameObject);
	}
}
