using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	public Text pointsUI;

	// Use this for initialization
	void Start () {
		if (pointsUI == null) {
			pointsUI = GameObject.Find ("score-display").GetComponent<Text>();
		}
		ScoreCtrl.OnScoreChange += UpdateUI;
	}
	
	void UpdateUI () {
		int score = ScoreCtrl.GetScore ();
		pointsUI.text = score + " pts";
	}
}
