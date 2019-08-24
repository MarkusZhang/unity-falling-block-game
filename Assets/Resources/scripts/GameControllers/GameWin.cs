using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 1. show win image
// 2. display score (count up)
// 3. display play again
public class GameWin : MonoBehaviour {

	public Text finalScoreDisplay;
	public Image winImage;
	public Text yourScoreIs;
	public Text pressSpace;

	public float imageFillSpeed;

	// Use this for initialization
	void Start () {
		StartCoroutine (ShowWinImage ());
	}

	IEnumerator ShowWinImage(){
		while (winImage.fillAmount < 1) {
			winImage.fillAmount += imageFillSpeed * Time.deltaTime;
			yield return null;
		}
		StartCoroutine (DisplayScore ());
	}

	IEnumerator DisplayScore(){
		yourScoreIs.gameObject.SetActive (true);
		finalScoreDisplay.gameObject.SetActive (true);

		// count up animation
		int score = 0;
//		int finalScoreToDisplay = 300;
		int finalScoreToDisplay = ScoreCtrl.GetScore ();

		while (score < finalScoreToDisplay - 10) {
			finalScoreDisplay.text = "" + score;
			score += 10;
			yield return null;
		}

		while (score < finalScoreToDisplay) {
			score += 1;
			finalScoreDisplay.text = "" + score;
			yield return null;
		}

		finalScoreDisplay.color = Color.yellow;

		// display press space
		pressSpace.gameObject.SetActive(true);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			ResetStaticComponents ();
			SceneManager.LoadScene ("start-scene");
		}
	}

	void ResetStaticComponents(){
		ScoreCtrl.Reset ();
		WeaponStoreCtrl.Reset ();
		GunStore.Reset ();
		LifeCtrl.Reset();
		StageCtrl.Reset();
	}

}
