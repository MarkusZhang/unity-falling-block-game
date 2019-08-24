using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enemy that launches attack at certain interval and probability
[RequireComponent(typeof(IAttacker))]
public class RandomAttackEnemy : MonoBehaviour, IControlledAttacker
{

	public int[] attackProbs; // each prob is between 1 to 100
	public float attackInterval; // time to pause after one attack
	public float nonattackInterval = 0.05f;

	private IAttacker[] myAttackers;

	public bool attackOnStart = true;
	
	// Use this for initialization
	void Start ()
	{
		IAttacker[] attackers = GetComponents<IAttacker>();
		Debug.Assert(attackers.Length == attackProbs.Length,"# attackers should match length of attackProbs");
		myAttackers = attackers;
		if (attackOnStart)
		{
			StartAttack();
		}
	}
	
	public void StartAttack()
	{
		StartCoroutine(AttackCoroutine());
	}
	
	IEnumerator AttackCoroutine()
	{
		var attackThresholds = new int[attackProbs.Length];
		attackThresholds[0] = attackProbs[0];
		for (int i = 1; i < attackProbs.Length; i++)
		{
			attackThresholds[i] = attackThresholds[i - 1] + attackProbs[i];
		}
		
		while (true)
		{
			// choose an attack
			var randInt = Random.Range(1, 100);
			for (int i = 0; i < attackThresholds.Length; i++)
			{
				if (randInt < attackThresholds[i])
				{
					myAttackers[i].Attack();
					yield return new WaitForSeconds(attackInterval);
					break;
				}
			}

			yield return new WaitForSeconds(nonattackInterval);
		}
	}

	
}
