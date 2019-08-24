using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class Sinno : MonoBehaviour,IControlledAttacker {

	// weapons
	public Gun swingGun;
	public Gun sprayGun;
	public Gun burstGun;
	public Gun trackGun;
	
	// motion control
	public float chargeAttackSpeed;
	public float randomMoveSpeed;
	
	// config
	public bool autoStart;

	private int numHitsTaken = 0;
	
	// Use this for initialization
	void Start ()
	{
		GetComponent<LivingEntity>().OnTakeDamage += () => { numHitsTaken++; };
		if (autoStart)
		{
			StartAttack();
		}
	}
	
	// Update is called once per frame
	IEnumerator attackRoutine () {
		while (true)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (numHitsTaken > 20)
			{
				// try to do random escape
				numHitsTaken = 0;
				StopCoroutine("randomMove");
				transform.position = getEscapePosition();
				StartCoroutine("randomMove");
			}
			if (playerRef != null)
			{
				// this ranges from -90 to 90
				var angleToPlayer = Utils.getAngleTo(transform.position, playerRef.transform.position) + 90;
				var distToPlayer = (transform.position - playerRef.transform.position).magnitude;
				
				if (distToPlayer < 4.5f && Mathf.Abs(angleToPlayer) < 15)
				{
					burstGun.Shoot();
					yield return new WaitForSeconds(0.6f);
				}else if (Mathf.Abs(angleToPlayer) < 15 && Random.Range(0,1f) > 0.85f)
				{
					burstGun.Shoot();
					yield return new WaitForSeconds(0.6f);
				}
				else if (Mathf.Abs(Mathf.Abs(angleToPlayer) - 45) < 5)
				{
					sprayGun.Shoot();
				}
				else if (Mathf.Abs(angleToPlayer) < 15)
				{
					swingGun.Shoot();
				}
				else if (Mathf.Abs(angleToPlayer) > 70)
				{
					StopCoroutine("randomMove");
					var currentPosition = transform.position;
					var playerPosition = playerRef.transform.position;
					// bump into player
					while (transform.position!=playerPosition)
					{
						transform.position = Vector3.MoveTowards(transform.position, playerPosition,
							chargeAttackSpeed * Time.deltaTime);
						yield return null;
					}
					// go back
					while (transform.position!=currentPosition)
					{
						transform.position = Vector3.MoveTowards(transform.position, currentPosition,
							chargeAttackSpeed * Time.deltaTime);
						yield return null;
					}

					StartCoroutine("randomMove");
				}
				else
				{
					trackGun.Shoot();
					yield return new WaitForSeconds(0.5f);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	// randomly move left and right
	IEnumerator randomMove()
	{
		while (true)
		{
			var targetX = Utils.GetRandomX(-0.4f, 0.4f);
			var targetPos = new Vector3(targetX,transform.position.y,0);
			while (transform.position!=targetPos)
			{
				transform.position = Vector3.MoveTowards(transform.position,targetPos,Time.deltaTime*randomMoveSpeed);
				yield return null;
			}
		}
	}

	Vector3 getEscapePosition()
	{
		float screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		if (transform.position.x <= -screenHalfWidth * 0.7f) // move slightly to right
		{
			return new Vector3(Utils.GetRandomX(-0.2f,1f),transform.position.y,0);
		}else if (transform.position.x >= screenHalfWidth * 0.7f) // move slightly to left
		{
			return new Vector3(Utils.GetRandomX(-1f,0.2f),transform.position.y,0);
		}
		else
		{
			// move either to left or right
			var x = (Random.Range(0, 1f) > 0.5f) ? Utils.GetRandomX(-1f, -0.6f) : Utils.GetRandomX(0.6f, 1f);
			return new Vector3(x,transform.position.y,0);
		}
	}
	
	public void StartAttack()
	{
		StartCoroutine("randomMove");
		StartCoroutine(attackRoutine());
	}
}
