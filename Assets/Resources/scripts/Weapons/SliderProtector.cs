using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderProtector : TimedWeapon {

	public float moveSpeed = 5f;
	private bool isMovingRight;
	private float screenHalfWidth;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		transform.position = new Vector2 (0, 0);
		isMovingRight = true;
		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (transform.position.x > screenHalfWidth) {
			isMovingRight = false;
		} else if(transform.position.x < - screenHalfWidth){
			isMovingRight = true;
		}

		if (isMovingRight) {
			transform.Translate (Vector2.right * moveSpeed * Time.deltaTime);
		} else {
			transform.Translate (Vector2.left * moveSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "falling-block") {
			Destroy (collider.gameObject);
		}
	}

	protected override float TotalAliveTime(){
		return 15f;
	}
}
