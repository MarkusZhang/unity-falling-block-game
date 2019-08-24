using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enemy that runs after the player and shoot periodically, it destroys itself when going out of screen
public class RunAftShooter : LivingEntity, IControlledAttacker
{
	public int maxUpdateDirTimes; // update number of times to update direction
	public float updateDirInterval;
	public float turnSpeed;
	public float moveForwardSpeed;
	public GameObject bulletPrefab;
	public Transform muzzle;
	public float shootInterval;

	public bool attackOnStart = false;
	
	private int updateTimes;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		updateTimes = 0;
		if (attackOnStart)
		{
			StartAttack();
		}
	}

	public void StartAttack()
	{
		StartCoroutine(runAftAndShoot());
	}
	
	IEnumerator runAftAndShoot()
	{
		float nextUpdateTime;
		float lastShootTime = Time.time;
		while (updateTimes < maxUpdateDirTimes)
		{
			nextUpdateTime = Time.time + updateDirInterval;
			float targetAngle = getAngleToPlayer();
			
			// turn to run after player
			while (Time.time < nextUpdateTime)
			{
				rotateTowardTarget(targetAngle,turnSpeed);
				transform.Translate(Vector3.down * Time.deltaTime * moveForwardSpeed);
				yield return new WaitForSeconds(Time.deltaTime);
			}

			if (Time.time - lastShootTime >= shootInterval)
			{
				Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
				lastShootTime = Time.time;
			}
			
			updateTimes++;
		}
		
		// no more dir update, just move forward
		while (true)
		{
			if (Utils.IsOffScreen2D(transform.position))
			{
				Destroy(gameObject);
			}
			transform.Translate(Vector3.down * Time.deltaTime * moveForwardSpeed);
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	
	float getAngleToPlayer()
	{
		var playerRef = GameObject.FindGameObjectWithTag("player");
		if (playerRef != null)
		{
			Vector3 targetDir = (playerRef.transform.position - transform.position).normalized;
			return 90 + Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
		}
		else
		{
			return transform.eulerAngles.z;
		}
	}
	
	void rotateTowardTarget(float targetAngle,float turnSpeed)
	{
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z,targetAngle,Time.deltaTime * turnSpeed);
		transform.eulerAngles = Vector3.forward * angle;
	}

	
}
