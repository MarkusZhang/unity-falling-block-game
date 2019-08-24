using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLaserShooter : LivingEntity, IControlledAttacker {

    public float rotationSpeed;
	public Vector3 rotationCenter; // TODO: make it rotate around a custom angle
	public Transform muzzle;
	public GameObject laserPrefab;
	public float waitSecsAftShoot;

	public bool attackOnStart = false;

	protected override void Start() {
		base.Start();
		if (attackOnStart)
		{
			StartAttack();
		}
	}
	
	public void StartAttack()
	{
		StartCoroutine(aimAndShoot());
	}
	
	IEnumerator aimAndShoot()
	{
		while (true)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (playerRef != null)
			{
				// get the angle to face the player
				float angleToPlayer = getAngleToPlayer(playerRef.transform.position);
				if (Mathf.Abs(Mathf.DeltaAngle(angleToPlayer , transform.rotation.eulerAngles.z)) < 5f)
				{
					shootLaser();
					yield return new WaitForSeconds(waitSecsAftShoot);
				}
				else
				{
					rotateTowardTarget(angleToPlayer,rotationSpeed);
					yield return new WaitForSeconds(0.1f);
				}
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
			
		}
	}

	void shootLaser()
	{
		Instantiate(laserPrefab, muzzle.position, muzzle.rotation);
	}

	void rotateTowardTarget(float targetAngle,float turnSpeed)
	{
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z,targetAngle,Time.deltaTime * turnSpeed);
		transform.eulerAngles = Vector3.forward * angle;
	}

	float getAngleToPlayer(Vector3 playerPos)
	{
		Vector3 targetDir = (playerPos - transform.position).normalized;
		return 90 + Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
	}

	
}
