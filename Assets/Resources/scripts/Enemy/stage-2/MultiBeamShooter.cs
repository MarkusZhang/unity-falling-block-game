using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletAttacker),typeof(FlippingAnimation))]
public class MultiBeamShooter : LivingEntity,IControlledAttacker
{
	public float shootInterval;
	public float shootLength;
	public int roundsOfAttack;
	public float moveSpeed;

	public bool attackOnStart = false;
	public bool flipOnStart = true;

	private int round;
	
	protected override void Start(){
		base.Start();
		if (flipOnStart)
		{
			startFlipping();
		}
		if (attackOnStart)
		{
			StartAttack();
		}
	}
	
	public void StartAttack()
	{
		round = 0;
		if (flipOnStart)
		{
			stopFlipping();
		}
		doNextAction();
	}

	void doNextAction()
	{
		if (round == 0)
		{
			StartCoroutine(shoot());
			round++;
		}else if (round < roundsOfAttack)
		{
			StartCoroutine(moveAndShoot());
			round++;
		}
		else // end of attack
		{
			StartCoroutine(moveOut());
		}
	}
	
	//
	// Corountines
	//

	IEnumerator shoot()
	{
		var startTime = Time.time;
		while (Time.time - startTime < shootLength)
		{
			GetComponent<BulletAttacker>().Attack();
			yield return new WaitForSeconds(shootInterval);
		}
		
		// short wait for avoid too fast attack
		yield return new WaitForSeconds(0.5f);
		doNextAction();
	}

	IEnumerator moveAndShoot()
	{
		// move to opposite side, with slight random offset
		var targetX = - transform.position.x + Random.Range(-1f, 1f);
		var targetPos = new Vector3(targetX,transform.position.y,0);
		startFlipping();
		while (transform.position.x != targetX)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
			yield return null;
		}
		stopFlipping();
		
		var startTime = Time.time;
		while (Time.time - startTime < shootLength)
		{
			GetComponent<BulletAttacker>().Attack();
			yield return new WaitForSeconds(shootInterval);
		}
		
		// short wait for avoid too fast attack
		yield return new WaitForSeconds(0.5f);
		doNextAction();
	}

	IEnumerator moveOut()
	{
		while (true)
		{
			transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
			yield return null;
		}
		
		// assume `DestroyWhenOffScreen` is attached to the gameobject
	}

	void startFlipping()
	{
		GetComponent<FlippingAnimation>().StartFlipping();
	}

	void stopFlipping()
	{
		GetComponent<FlippingAnimation>().StopFlipping();
	}
}
