using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ExplosionAnimation : MonoBehaviour {

	public GameObject[] effects;
	public float animationLength = 1E6f; // length of animation in seconds
	public float minX, minY, maxX, maxY;

	float startTime;

	void Start(){
		startTime = Time.time;
		StartCoroutine (Animate ());
	}
		
	IEnumerator Animate(){
		while (Time.time - startTime < animationLength) {
			// random choose a position to animate
			float x = Random.Range(minX,maxX);
			float y = Random.Range (minY, maxY);
			// instantiate explosion effect
			int effectIdx = Random.Range(0,effects.Length);
			GameObject effect = Instantiate (effects [effectIdx], new Vector3 (x, y, 0), Quaternion.identity);
			float scaling = Random.Range (0.5f, 2);
			effect.transform.localScale = new Vector3(scaling,scaling,1);
			yield return new WaitForSeconds (0.01f);
			// change opcacity of target
//			float opacity = Time.time - startTime >= animationLength?
//				0 : 1 - (Time.time - startTime) / animationLength;
//			targetObject.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, opacity);
		}

		Destroy (gameObject);
	}
}
