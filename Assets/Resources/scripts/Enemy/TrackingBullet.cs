using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : LivingEntity {

	public float maxAliveTime;
	public float moveSpeed;
	public float turnSpeed;
	public float updateTargetInterval;
	public bool destoryWhenOffScreen;

	Transform playerTrans;
	float startTime;
	Vector2 targetPos;
	Vector2 targetDir;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		playerTrans = GameObject.Find ("player").transform;
		StartCoroutine (UpdateTarget ());
	}
	
	// Update is called once per frame
	void Update () {
		// destroy when reaching max time or going off screen
		if (Time.time - startTime > maxAliveTime) {
			Destroy (gameObject);
		} else if (destoryWhenOffScreen && Utils.instance.IsOffScreen (transform.position)) {
			Destroy (gameObject);
		}

		// move to target
		transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
		// look to target
		float targetAngle = Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
		if (Mathf.Abs (Mathf.DeltaAngle (targetAngle, transform.eulerAngles.z)) > .05f) {
			// turning angle
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.z, targetAngle, Time.deltaTime * turnSpeed);
			transform.eulerAngles = new Vector3(0,0,angle);
		}
	}

	IEnumerator UpdateTarget(){
		while (true) {
			targetPos = playerTrans.position;
			float xDiff = playerTrans.position.x - transform.position.x;
			float yDiff = playerTrans.position.y - transform.position.y;
			targetDir = (new Vector2(xDiff,yDiff)).normalized;
			yield return new WaitForSeconds (updateTargetInterval);
		}
	}
}
