using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireball : MonoBehaviour {

	public float speed = 7;
	public float angle = 15;

	float screenHalfWidth;
	float screenHalfHeight;
	Vector2 moveDir;

	// Use this for initialization
	void Start () {
		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		screenHalfHeight = Camera.main.orthographicSize;
		moveDir = Vector2.left;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -angle));
	}
	
	// Update is called once per frame
	void Update () {
		// change rotation and direction when reaching screen edge
		if (transform.position.x <= -screenHalfWidth) {
			GetComponent<SpriteRenderer> ().flipX = true;
			moveDir = Vector2.right;
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		} else if (transform.position.x >= screenHalfWidth) {
			GetComponent<SpriteRenderer> ().flipX = false;
			moveDir = Vector2.left;
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -angle));
		}

		transform.Translate (moveDir * speed * Time.deltaTime);

		if (transform.position.y > screenHalfHeight) {
			Destroy (gameObject);
		}
	}
}
