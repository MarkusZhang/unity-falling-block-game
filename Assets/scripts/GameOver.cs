using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public GameObject gameOverScreen;
	public Text finalScoreUI;
	bool isGameOver;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene (0); // reload the scene
			}
		}
	}

	public void OnGameOver(){
		gameOverScreen.SetActive (true);
		isGameOver = true;
		int finalScore = FindObjectOfType<ScoreCtrl> ().getScore ();
		finalScoreUI.text = finalScore.ToString ();
	}
}
