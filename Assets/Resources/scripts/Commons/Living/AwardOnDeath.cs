using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class AwardOnDeath : MonoBehaviour
{
	// if an element is set to `30` in `awardPrefabs`,
	// the probability of generating it is 30%
	public int[] awardProbs; 
	public GameObject[] awardPrefabs;
	
	// Use this for initialization
	void Start () {
		Debug.Assert(awardProbs.Length == awardPrefabs.Length);
		Debug.Assert(awardProbs.Length > 0,"awardProbs must not be empty");
		Debug.Assert(awardProbs.Sum() <= 100,"awardProbs' sum can not exceed 100");
		GetComponent<LivingEntity>().OnDeath += OnDeath;
	}

	void OnDeath()
	{
		var awardThresholds = new int[awardProbs.Length];
		awardThresholds[0] = awardProbs[0];
		for (int i = 1; i < awardProbs.Length; i++)
		{
			awardThresholds[i] = awardThresholds[i - 1] + awardProbs[i];
		}
		
		// generate award according to the probability
		var randInt = Random.Range(0, 100);
		for (int i = 0; i < awardThresholds.Length; i++)
		{
			if (randInt < awardThresholds[i])
			{
				Instantiate(awardPrefabs[i],transform.position,Quaternion.identity);
				break;
			}
		}
	}
	
}
