using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour {

	public Text finalScoreDisplay;

	// Use this for initialization
	void Start () {
		finalScoreDisplay.text = "" + ScoreCtrl.GetScore ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			ResetStaticComponents ();
			SceneManager.LoadScene ("start-scene");
		}
	}

	void ResetStaticComponents(){
		ScoreCtrl.Reset ();
		WeaponStoreCtrl.Reset ();
		GunStore.Reset ();
	}
}
