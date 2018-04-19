using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private float speed = 7;
	private float screenHalfWidth;

	// Use this for initialization
	void Start () {

		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;

	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxisRaw ("Horizontal");
		float velocity = inputX * speed;
		transform.Translate (Vector2.right * velocity * Time.deltaTime);

		// wrap around the scene
		if (transform.position.x < -screenHalfWidth) {
			transform.position = new Vector2(screenHalfWidth,transform.position.y);
		} else if (transform.position.x > screenHalfWidth) {
			transform.position = new Vector2(- screenHalfWidth, transform.position.y);
		}

	}


	void OnTriggerEnter2D(Collider2D other){
		FindObjectOfType<GameOver> ().OnGameOver ();
		Destroy (gameObject);
		print ("player died");
	}
}
