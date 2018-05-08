using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScoreCtrl{
	public static int score;
	public static event System.Action OnScoreChange;

	public static int[] levelThreshold = {5,10,15,20};
	public static int currentLevel = 0;
	public static event System.Action OnLevelChange;

	public static void AddScore(){
		score += 1;
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
	}

	public static int GetScore(){
		return score;
	}

	public static int GetLevel(){
		return currentLevel;
	}

	// this should be called when game is started over again
	public static void Reset(){
		currentLevel = 0;
		score = 0;
		OnScoreChange = null;
		OnLevelChange = null;
	}
}


