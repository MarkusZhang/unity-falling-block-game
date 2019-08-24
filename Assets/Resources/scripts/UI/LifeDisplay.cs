using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour {

	public Text lifeUI;

	// Use this for initialization
	void Start () {
		LifeCtrl.OnLivesLeftChange += UpdateUI;
		UpdateUI();
	}
	
	void UpdateUI () {
		int lives = LifeCtrl.GetLifeLeft();
		lifeUI.text = "x" + lives;
	}
}
