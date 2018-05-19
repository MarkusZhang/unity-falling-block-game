using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour {

	public GameObject targetObject;
	public GameObject[] effects;
	public float animationLength; // length of animation in seconds
	public event System.Action OnAnimationEnd;

	float minX, minY, maxX, maxY;

	void Start(){
		StartAnimation();
	}

	public void StartAnimation(){
		Vector3 bounds = targetObject.GetComponent<Renderer> ().bounds.size;
		float width = bounds.x;
		float height = bounds.y;
		minX = targetObject.transform.position.x - width / 2;
		maxX = targetObject.transform.position.x + width / 2;
		minY = targetObject.transform.position.y - height / 2;
		maxY = targetObject.transform.position.y + height / 2;

		StartCoroutine (AnimateExplosion ());
	}

	IEnumerator AnimateExplosion(){
		float startTime = Time.time;
		Color c = targetObject.GetComponent<SpriteRenderer>().color;

		while (Time.time - startTime < animationLength) {
			// random choose a position to animate
			float x = Random.Range(minX,maxX);
			float y = Random.Range (minY, maxY);
			// instantiate explosion effect
			int effectIdx = Random.Range(0,effects.Length);
			GameObject effect = Instantiate (effects [effectIdx], new Vector3 (x, y, 0), Quaternion.identity);
			float scaling = Random.Range (0.5f, 2);
			effect.transform.localScale = new Vector3(scaling,scaling,1);
			// change opcacity of target
			float opacity = Time.time - startTime >= animationLength?
				0 : 1 - (Time.time - startTime) / animationLength;
			targetObject.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, opacity);
			yield return null;
		}

		if (OnAnimationEnd != null) {
			OnAnimationEnd ();
		}
	}
}
