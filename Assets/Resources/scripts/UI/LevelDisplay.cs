using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour {

	public Text levelUI;

	// Use this for initialization
	void Start () {
		if (levelUI == null) {
			levelUI = GameObject.Find ("level-display").GetComponent<Text>();
		}
		ScoreCtrl.OnLevelChange += UpdateUI;
	}

	void UpdateUI () {
		int level = ScoreCtrl.GetLevel ();
		levelUI.text = "L " + level;
	}

}
