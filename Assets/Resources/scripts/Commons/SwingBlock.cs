using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// component that keeps swinging block left and right
public class SwingBlock : MonoBehaviour {

	public float swingSpeed = 1;
	public float swingDist = 0.5f;

	// Use this for initialization
	void Start () {
		StartCoroutine (SwingLeftRight ());
	}
	
	IEnumerator SwingLeftRight(){
		float centerX = transform.position.x;
		Vector2 moveDir = Vector2.left;

		while (true) {
			transform.Translate (moveDir * swingSpeed * Time.deltaTime);
			if (transform.position.x < centerX - swingDist) {
				moveDir = Vector2.right;
			} else if (transform.position.x > centerX + swingDist) {
				moveDir = Vector2.left;
			}
			yield return null;
		}
	}
}
