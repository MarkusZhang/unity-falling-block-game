using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAroundBackground : MonoBehaviour {

	public float scrollSpeed = 5f;
	public float maxScrollSpeed = 35f;
	float startY;
	public float wraparoundDiff = 22.51f;

	public static WrapAroundBackground instance;
	
	private bool isBlinking = false;
	private bool cannotBlink = false; // cannot blink when background is faded out

	void Awake(){
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
		print("height is " + GetComponent<SpriteRenderer>().bounds.size.y);
		// resize background to fill screen
		startY = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector2 (transform.position.x, transform.position.y - scrollSpeed * Time.deltaTime);
		if (transform.position.y <= startY - wraparoundDiff) {
			transform.position =  new Vector2 (transform.position.x, startY);
		}
	}

	public void IncrScrollSpeed(float val)
	{
		scrollSpeed = Mathf.Min(scrollSpeed + val, maxScrollSpeed);
	}
	
	public void Blink(float blinkTime)
	{
		if (!isBlinking && !cannotBlink)
		{
			isBlinking = true;
			StartCoroutine(blink(blinkTime));
		}
	}

	//TODO: we may need better synchronization technique
	//TODO: so far we assume FadeIn and FadeOut will be called in close period
	public void FadeOut(float fadeTime)
	{
		StartCoroutine(fadeOut(fadeTime));
	}

	public void FadeIn(float fadeTime)
	{
		StartCoroutine(fadeIn(fadeTime));
	}

	IEnumerator fadeOut(float fadeOutTime)
	{
		cannotBlink = true;
		var alpha = 1f;
		while (alpha > 0f)
		{
			alpha -= Time.deltaTime / fadeOutTime;
			setAlphaValue(alpha);
			yield return null;
		}
	}

	IEnumerator fadeIn(float fadeInTime)
	{
		cannotBlink = true;
		var alpha = 0f;
		while (alpha < 1f)
		{
			alpha += Time.deltaTime / fadeInTime;
			setAlphaValue(alpha);
			yield return null;
		}

		cannotBlink = false;
	}

	IEnumerator blink(float blinkTime)
	{
		var blinkInterval = 0.1f;
		var startTime = Time.time;
		while (Time.time - startTime < blinkTime)
		{
			if (cannotBlink)
			{
				break;
			}
			setAlphaValue(0.3f);
			yield return new WaitForSeconds(blinkInterval);
		
			setAlphaValue(1f);
			yield return new WaitForSeconds(blinkInterval);
		}

		isBlinking = false;		
	}
	
	void setAlphaValue(float alpha) {
		Utils.SetAlphaValue(transform.GetChild(0).gameObject,alpha);
		Utils.SetAlphaValue(gameObject,alpha);
	}


	public void ChangeImage(string name)
	{
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (name);
		var otherObj = transform.GetChild(0).gameObject;
		otherObj.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (name);
	} 
}
