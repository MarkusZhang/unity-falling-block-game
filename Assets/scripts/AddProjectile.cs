using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddProjectile : MonoBehaviour {

	public float speed = 5;

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

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" || other.tag == "bullet") {
			FindObjectOfType<PlayerControl> ().enableSideProjectile ();
			Destroy (gameObject);
		}
	}
}
