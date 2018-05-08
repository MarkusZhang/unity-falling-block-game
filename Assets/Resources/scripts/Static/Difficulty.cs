using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Difficulty{

	static float secondsToMaxDifficulty = 100;

	public static float GetDifficultyPercent() {
		return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
	}

}