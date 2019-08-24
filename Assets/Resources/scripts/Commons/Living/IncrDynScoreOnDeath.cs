using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// dynamic score is given according to how much health the enemy has and how fast it gets killed
public class IncrDynScoreOnDeath : MonoBehaviour {

	public float multiplier;

	private LivingEntity entity;
	
	// Use this for initialization
	void Start ()
	{
		entity = GetComponent<LivingEntity>();
		entity.OnDeath += OnDeath;
	}
	
	void OnDeath ()
	{
		float totalAliveTime = Time.time - entity.GetStartAliveTime();
		int score = (int)(entity.startingHealth / (1 + totalAliveTime) * multiplier);
		score = score > 0 ? score : 1;
		ScoreCtrl.AddScore(score);
	}
}
