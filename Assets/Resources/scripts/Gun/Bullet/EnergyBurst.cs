using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBurst : MonoBehaviour {
    
	public GameObject[] bursts;
	public Color[] colors;

	public float scale;
	public float stayTime;

	// Use this for initialization
	void Start () {
		Debug.Assert(bursts.Length>0);
		StartCoroutine(changeImg());
		if (colors.Length > 0)
		{
			StartCoroutine(changeBackgroundColor());
		}
	}

	IEnumerator changeImg()
	{
		var i = 0;
		var startTime = Time.time;
		while (Time.time - startTime < stayTime)
		{
			if (bursts[i] != null)
			{
				GameObject curBurst = Instantiate(bursts[i], transform.position, Quaternion.identity);
				curBurst.transform.localScale = scale * Vector3.one;
				var c = curBurst.GetComponent<SpriteRenderer>().color;
				curBurst.GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b,0.7f);
				i = (i + 1) % bursts.Length;
				yield return new WaitForSeconds(0.05f);
				Destroy(curBurst);
			}
			else
			{
				i = (i + 1) % bursts.Length;
				yield return new WaitForSeconds(0.05f);
			}
		}
		
		Destroy(gameObject);
	}

	IEnumerator changeBackgroundColor()
	{
		var i = 0;
		var startTime = Time.time;
		var originalColor = Camera.main.backgroundColor;
		while (Time.time - startTime < stayTime)
		{
			Camera.main.backgroundColor = colors[i];
			i = (i + 1) % colors.Length;
			yield return new WaitForSeconds(0.05f);
		}

		Camera.main.backgroundColor = originalColor;
	}
	
}
