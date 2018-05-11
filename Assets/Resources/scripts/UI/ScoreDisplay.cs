using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	public Text pointsUI;

	// Use this for initialization
	void Start () {
		ScoreCtrl.OnScoreChange += UpdateUI;
	}
	
	void UpdateUI () {
		int score = ScoreCtrl.GetScore ();
		pointsUI.text = score + " pts";
	}
}
