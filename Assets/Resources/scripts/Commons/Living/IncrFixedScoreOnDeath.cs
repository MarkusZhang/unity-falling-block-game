using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class IncrFixedScoreOnDeath : MonoBehaviour
{
	public int scoreToIncr;
	
	// Use this for initialization
	void Start () {
		GetComponent<LivingEntity>().OnDeath += OnDeath;
	}
	
	void OnDeath () {
		ScoreCtrl.AddScore(scoreToIncr);
	}
}
