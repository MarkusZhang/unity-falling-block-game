using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingProtector : TimedWeapon {

	private float currentAngle;
	public int rotationSpeed = 4; // degrees rotated per frame
	public float initialRadius = 4;
	public Vector2 radiusChangeLimit = new Vector2(1,4);
	public float radiusChangeSpeed = 2;
	public Transform player;

	float radius;
			
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		currentAngle = 0;
		radius = initialRadius;
		player = GameObject.FindGameObjectWithTag ("player").transform;

		StartCoroutine (ChangeRadius ());
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		float x = radius * Mathf.Cos (currentAngle * Mathf.Deg2Rad);
		float y = radius * Mathf.Sin (currentAngle * Mathf.Deg2Rad);
		transform.position = new Vector2 (player.position.x + x, player.position.y + y);
		currentAngle = (currentAngle + rotationSpeed) % 360;
	}

	IEnumerator ChangeRadius(){
		float inLimit = radiusChangeLimit.x;
		float outLimit = radiusChangeLimit.y;
		float changeDir = 1;
		while (true) {
			if (radius > initialRadius + outLimit) {
				changeDir = -1;
			} else if (radius < initialRadius - inLimit) {
				changeDir = 1;
			}
			radius += Time.deltaTime * radiusChangeSpeed * changeDir;
			yield return null;
		}
	}

	protected override float TotalAliveTime(){
		return 15f;
	}
}
