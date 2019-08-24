using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public static class ScoreCtrl{
	// state vars
	public static int score;
	public static int currentLevel = 0;
	public static int awardThresholdIdx = 0;
	
	public static event System.Action OnScoreChange;
	public static event System.Action OnLevelChange;
	
	// static configs
	public static int[] levelThreshold = {50,250,1100,4500};
	public static float[] playerMoveSpeeds = new float[] {8, 9, 10, 11, 12}; 
	public static float[] gunRelativeSpeeds = new float[]{1f,1.5f,1.7f,2f,2.5f};
	public static int[] awardLifeThresholds = new int[] {1000, 2500, 4500,6500};
	
	public static void AddScore(){
		AddScore (1);
	}

	public static void AddScore(int amount){
		score += amount;
		if (OnScoreChange != null) {
			OnScoreChange ();
		}

		// check and update level
		if (currentLevel < levelThreshold.Length && score > levelThreshold [currentLevel]) {
			currentLevel++;
			if (OnLevelChange != null) {
				OnLevelChange ();
			}
		}
		
		// check and award life
		if (awardThresholdIdx < awardLifeThresholds.Length && score > awardLifeThresholds[awardThresholdIdx])
		{
			LifeCtrl.AddLife();
			awardThresholdIdx++;
		}
	}

	public static int GetScore(){
		return score;
	}

	public static int GetLevel(){
		return currentLevel;
	}

	public static float GetLevelMoveSpeed()
	{
		return playerMoveSpeeds[currentLevel];
	}

	public static float GetLevelGunSpeed()
	{
		return gunRelativeSpeeds[currentLevel];
	}

	// this should be called when game is started over again
	public static void Reset(){
		currentLevel = 0;
		score = 0;
		awardThresholdIdx = 0;
		RemoveEventListeners();
	}

	public static void RemoveEventListeners()
	{
		OnScoreChange = null;
		OnLevelChange = null;
	}
}


