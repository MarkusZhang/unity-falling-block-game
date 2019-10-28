using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EyeBlinkEffect : MonoBehaviour
{
	public float effectTime;
	public float rotateSpeed;
	public float maxScale;
	public float startScale;
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(startScale,startScale,1);
		StartCoroutine(blink());
	}

	IEnumerator blink()
	{
		var startTime = Time.time;
		var angle = 0f;
		var scaleDelta = (maxScale - startScale) * 2 * Time.deltaTime / effectTime;
		
		// scale out
		while (Time.time - startTime < effectTime/2 && transform.localScale.x < maxScale)
		{
			var scale = transform.localScale.x + scaleDelta;
			transform.localScale = new Vector3(scale,scale,1);
			angle += rotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(angle * Vector3.forward);
			yield return null;
		}
		
		// scale back
		while (Time.time - startTime < effectTime && transform.localScale.x > 0)
		{
			var scale = transform.localScale.x - scaleDelta;
			transform.localScale = new Vector3(scale,scale,1);
			angle += rotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(angle * Vector3.forward);
			yield return null;
		}
		
		Destroy(gameObject);
	}
}
