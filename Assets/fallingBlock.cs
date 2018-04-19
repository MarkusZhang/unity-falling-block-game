using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingBlock : MonoBehaviour {

	private float speed = 15;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.down * speed * Time.deltaTime);

		if (transform.position.y < -Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}

}
