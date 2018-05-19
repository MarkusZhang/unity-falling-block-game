using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

	public float speed = 15;
	public GameObject explodeEffect;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.up * speed * Time.deltaTime);

		if (transform.position.y > Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}

}
