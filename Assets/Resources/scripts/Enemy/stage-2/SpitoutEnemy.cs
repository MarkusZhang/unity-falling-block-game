using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this enemy goes down, spit out bullets and then go back
// it should be instantiated upper outside the screen
[RequireComponent(typeof(LivingEntity),typeof(BulletAttacker),typeof(DestroyWhenGoingOffScreen))]
public class SpitoutEnemy : MonoBehaviour,IControlledAttacker
{
	public float moveSpeed;
	public float squeezeSpeed;

	public bool attackOnStart;
	
	private float originalScaleY;
	
	// Use this for initialization
	void Start ()
	{
		originalScaleY = transform.localScale.y;
		if (attackOnStart)
		{
			StartAttack();
		}
	}

	IEnumerator mainRoutine()
	{
		// randomly choose target y position and move
		var targetY = Utils.GetRandomY(0, 0.5f);
		var targetPos = new Vector3(transform.position.x,targetY,0);
		StartCoroutine("squeezeY");
		while (transform.position.y != targetY)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			yield return null;
		}
		StopCoroutine("squeezeY");
		
		// attack
		var attackers = GetComponents<BulletAttacker>();
		foreach (var attacker in attackers)
		{
			attacker.Attack();
		}
		
		// move back
		StartCoroutine("expandY");
		while (true)
		{
			transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator squeezeY()
	{
		yield return new WaitForSeconds(0.3f);
		while (transform.localScale.y > 0)
		{
			var newY = transform.localScale.y - squeezeSpeed * Time.deltaTime;
			transform.localScale = new Vector3(transform.localScale.x,newY,1);
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator expandY()
	{
		while (transform.localScale.y < originalScaleY)
		{
			var newY = transform.localScale.y + squeezeSpeed * Time.deltaTime;
			transform.localScale = new Vector3(transform.localScale.x,newY,1);
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void StartAttack()
	{
		StartCoroutine(mainRoutine());
	}
}
