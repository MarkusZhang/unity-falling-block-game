using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : LivingEntityWithAudio {

	public float maxSpeed = 20;
	public float minSpeed = 5;
	public float speed;

	public GameObject explosionEffect;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		speed = Mathf.Lerp (minSpeed, maxSpeed, Difficulty.GetDifficultyPercent ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.down * speed * Time.deltaTime);

		if (transform.position.y < -Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}

	public override void Die(){
		Instantiate (explosionEffect, transform.position, transform.rotation); // destroy animation
		ScoreCtrl.AddScore ();
		base.Die ();
	}
}
