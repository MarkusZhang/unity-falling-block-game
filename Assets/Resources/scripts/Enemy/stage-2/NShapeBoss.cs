using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LaserAttacker),typeof(BulletAttacker))]
public class NShapeBoss : LivingEntity,IControlledAttacker
{
	public float moveSpeed;
	public float shootLaserBulletInterval;
	public float shootBulletInterval;

	public bool attackOnStart = false;
	
	private bool lastMovIsDown; 
	
	// Use this for initialization
	protected override void Start(){
		base.Start();
		if (attackOnStart)
		{
			StartAttack();
		}
	}
	
	// Update is called once per frame
	void doNextAction()
	{
		if (isPlayerUnder())
		{
			StartCoroutine(laserDown());
		}
		else
		{
			if (lastMovIsDown)
			{
				lastMovIsDown = false;
				StartCoroutine(moveUpAndShoot());
			}
			else
			{
				lastMovIsDown = true;
				StartCoroutine(moveDownAndShoot());
			}
		}
	}

	bool isPlayerUnder()
	{
		var player = GameObject.FindGameObjectWithTag("player");
		if (player != null)
		{
			var diff = Mathf.Abs(transform.position.x - player.transform.position.x);
			if (diff <= 1.3)
			{
				return true;
			}
		}

		return false;
	}

	IEnumerator moveUpAndShoot()
	{
		// random pick a upper position
		var targetPos = new Vector3(Utils.GetRandomX(),Utils.GetRandomY(0.7f,1f),0);
		
		// move up and meanwhile shoot
		var bulletAttacker = GetComponents<BulletAttacker>()[1];
		float nextShootTime = Time.time;
		while (transform.position != targetPos)
		{
			transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
			if (Time.time > nextShootTime)
			{
				bulletAttacker.Attack();
				nextShootTime = Time.time + shootBulletInterval;
			}
			
			yield return null;
		}
		
		yield return new WaitForSeconds(1);
		
		doNextAction();
	}

	IEnumerator moveDownAndShoot()
	{
		// get target position
		var targetPos = new Vector3(Utils.GetRandomX(),Utils.GetRandomY(0,0.1f),0);
		var player = GameObject.FindGameObjectWithTag("player");
		if (player != null)
		{
			targetPos.x = player.transform.position.x;
		}
		
		// move down to player's position, and shoot
		var laserBulletAttacker = GetComponents<BulletAttacker>()[0];
		float nextShootTime = Time.time;
		while (transform.position != targetPos)
		{
			transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
			if (Time.time > nextShootTime)
			{
				laserBulletAttacker.Attack();
				nextShootTime = Time.time + shootLaserBulletInterval;
			}
			
			yield return null;
		}
		
		
		doNextAction();
	}

	IEnumerator laserDown()
	{
		// shoot laser
		var laserAttacker = GetComponent<LaserAttacker>();
		laserAttacker.Attack();
		yield return new WaitForSeconds(2f);
		
		// yield wait
		doNextAction();
	}

	public void StartAttack()
	{
		lastMovIsDown = false;
		doNextAction();
	}
}
