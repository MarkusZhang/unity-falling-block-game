using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUpSignal : MonoBehaviour
{
	public Text lifeupTxt;
	public float fadeInTime;
	public float stayTime;
	public float fadeOutTime;
	public float moveSpeed;
	
	// Use this for initialization
	void Awake()
	{
		LifeCtrl.OnLifeUp += OnLifeUp;
	}

	void OnLifeUp()
	{
		lifeupTxt.gameObject.SetActive(true);
		StartCoroutine(fadeInOut());
		StartCoroutine(moveUp());
		if (AudioManager.instance != null && AudioStore.instance != null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.collection);
		}
	}

	IEnumerator fadeInOut()
	{
		var c = lifeupTxt.color;
		var alpha = 0f;
		// fade in
		while (alpha < 1f)
		{
			alpha += Time.deltaTime / fadeInTime;
			lifeupTxt.color = new Color(c.r,c.g,c.b,alpha);
			yield return null;
		}
		
		yield return new WaitForSeconds(stayTime);
		
		// fade out
		while (alpha > 0f)
		{
			alpha -= Time.deltaTime / fadeOutTime;
			lifeupTxt.color = new Color(c.r,c.g,c.b,alpha);
			yield return null;
		}
	}

	IEnumerator moveUp()
	{
		var originalPos = lifeupTxt.transform.position;
		var startTime = Time.time;
		while (Time.time - startTime < fadeInTime + fadeOutTime + stayTime)
		{
			lifeupTxt.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
			yield return null;
		}

		lifeupTxt.transform.position = originalPos;
		lifeupTxt.gameObject.SetActive(false);
	}
}
