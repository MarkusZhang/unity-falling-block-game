using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour {

	public Text finalScoreDisplay;

	// Use this for initialization
	void Start () {
		finalScoreDisplay.text = "" + ScoreCtrl.GetScore ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("play-scene");
		}
	}

}
