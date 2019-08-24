using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity),typeof(IAttacker))]
public class CirclePlane : MonoBehaviour, IControlledAttacker
{

	public Transform parent;
	public float initialAngle; // angle in degree
	public float radius;
	public float stayTime;
	public float moveOutSpeed;
	public float moveInSpeed;
	public float rotationSpeed;

	public float attackInterval;
	public float attackProb; // 0 ~ 1

	public bool autoStart;
	
	// Use this for initialization
	void Start () {
		if (autoStart)
		{
			StartAttack();
		}
	}

	IEnumerator initialMove()
	{
		var xDiff = radius * Mathf.Cos(initialAngle * Mathf.Deg2Rad);
		var yDiff = radius * Mathf.Sin(initialAngle * Mathf.Deg2Rad);
		var initialTargetPos = new Vector3(parent.position.x + xDiff,parent.position.y + yDiff,0);

		while (transform.position!=initialTargetPos)
		{
			transform.position = Vector3.MoveTowards(transform.position,initialTargetPos,Time.deltaTime * moveOutSpeed);
			yield return null;
		}

		StartCoroutine("attackRoutine");
		StartCoroutine(moveInCircle());
	}

	IEnumerator moveInCircle()
	{
		var startTime = Time.time;
		var currentAngle = initialAngle;
		// move in circle, while doing attack
		while (Time.time - startTime < stayTime)
		{
			float x = radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
			float y = radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
			currentAngle = (currentAngle + Time.deltaTime * rotationSpeed) % 360;
			transform.position = new Vector2(parent.position.x + x, parent.position.y + y);
			yield return null;
		}

		StopCoroutine("attackRoutine");
		
		// move in circle, back
		while (radius >0)
		{
			radius -= Time.deltaTime * moveInSpeed;
			float x = radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
			float y = radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
			currentAngle = (currentAngle + Time.deltaTime * rotationSpeed) % 360;
			transform.position = new Vector2(parent.position.x + x, parent.position.y + y);
			yield return null;
		}
		
		Destroy(gameObject);
	}

	IEnumerator attackRoutine()
	{
		while (true)
		{
			var randVal = Random.Range(0, 1f);
			if (randVal < attackProb)
			{
				foreach (var attacker in gameObject.GetComponents<IAttacker>())
				{
					attacker.Attack();
				}
			}
			yield return new WaitForSeconds(attackInterval);
		}
	}

	public void StartAttack()
	{
		StartCoroutine(initialMove());
	}
}
