using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemy : MonoBehaviour
{

	public GameObject electricity;
	public Transform[] leftMuzzles;
	public Transform[] rightMuzzles;
	public float attackProb;
	public float attackInterval;
	public Sprite attackImg;
	public Sprite nonAttackImg;
	public float moveSpeed;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(moveDown());
		StartCoroutine(attackRoutine());
	}

	IEnumerator attackRoutine()
	{
		var lastAttackTime = Time.time;
		while (true)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (playerRef != null)
			{
				if (Mathf.Abs(playerRef.transform.position.y - transform.position.y) < 3 &&
				    Time.time - lastAttackTime > attackInterval &&
				    Random.Range(0, 1f) < attackProb)
				{
					// attack
					GetComponent<SpriteRenderer>().sprite = attackImg;
					if (playerRef.transform.position.x < transform.position.x) // player is on left
					{
						GetComponent<SpriteRenderer>().flipX = true;
						shoot(leftMuzzles);
					}
					else
					{
						shoot(rightMuzzles);
					}
					
					yield return new WaitForSeconds(1.5f);
					
					GetComponent<SpriteRenderer>().sprite = nonAttackImg;
					GetComponent<SpriteRenderer>().flipX = false;
				}
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	void shoot(Transform[] muzzles)
	{
		foreach (var m in muzzles)
		{
			Instantiate(electricity, m.position, m.rotation);
		}
	}
	
	IEnumerator moveDown()
	{
		while (true)
		{
			transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
			yield return null;
		}
	}
}
