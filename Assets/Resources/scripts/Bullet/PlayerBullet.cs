using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

	public float speed = 15;

	// Update is called once per frame
	public virtual void Update () {
		transform.Translate (Vector2.up * speed * Time.deltaTime);

		if (transform.position.y > Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}
}
