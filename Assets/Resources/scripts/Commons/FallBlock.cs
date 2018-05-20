using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// component that moves the object down until out of screen
public class FallBlock : MonoBehaviour {
	
	public float fallingSpeed = 2;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.down * fallingSpeed * Time.deltaTime);

		if (transform.position.y < -Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}
}
