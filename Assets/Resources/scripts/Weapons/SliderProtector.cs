using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderProtector : TimedWeapon {

	public float moveSpeed = 8f;
	public float initialY = 0;
	public float verticalMoveLimit = 2f; // max distance it can move up and down
	public float verticalMoveSpeed = 2f;
	private bool isMovingRight;
	private float screenHalfWidth;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		transform.position = new Vector2 (0, initialY);
		isMovingRight = true;
		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;

		StartCoroutine (MoveUpDown ());
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

	IEnumerator MoveUpDown(){
		Vector2 moveDir = Vector2.up;
		while (true) {
			if (transform.position.y > initialY + verticalMoveLimit) {
				moveDir = Vector2.down;
			} else if(transform.position.y < initialY - verticalMoveLimit){
				moveDir = Vector2.up;
			}
			transform.Translate (moveDir * verticalMoveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	protected override float TotalAliveTime(){
		return 15f;
	}
}
