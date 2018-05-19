using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShip : LivingEntity {

	bool isMoving;
	bool isShooting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "bullet" || collider.tag == "weapon") {
			TakeDamage (1);

		}

		if (collider.tag == "bullet") {
			Destroy (collider.gameObject);
		}
	}
}
