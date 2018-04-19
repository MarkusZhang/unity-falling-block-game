using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCtrl: MonoBehaviour{

	public Text pointsUI;
	private int score  = 0;

	public void addScore(){
		score += 1;
		pointsUI.text = score + " pts";
	}

	public int getScore(){
		return score;
	}
}
