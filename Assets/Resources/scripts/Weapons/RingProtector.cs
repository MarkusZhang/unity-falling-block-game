using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingProtector : TimedWeapon {

	private float currentAngle;
	public int rotationSpeed = 4; // degrees rotated per frame
	public float rotationRadius = 4;
	public Transform player;
			
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		currentAngle = 0;
		player = GameObject.FindGameObjectWithTag ("player").transform;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		float x = rotationRadius * Mathf.Cos (currentAngle * Mathf.Deg2Rad);
		float y = rotationRadius * Mathf.Sin (currentAngle * Mathf.Deg2Rad);
		transform.position = new Vector2 (player.position.x + x, player.position.y + y);
		currentAngle = (currentAngle + rotationSpeed) % 360;
	}

	protected override float TotalAliveTime(){
		return 15f;
	}
}
