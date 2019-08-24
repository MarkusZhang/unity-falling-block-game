using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class GameOver : MonoBehaviour {

	public Text finalScoreDisplay;
	public bool backToStartScene = true;

	// Use this for initialization
	void Start () {
		finalScoreDisplay.text = "" + ScoreCtrl.GetScore ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {

			if (backToStartScene)
			{
				ResetStaticComponents();
				StageCtrl.Reset();
				SceneManager.LoadScene("start-scene");
			}
			else
			{
				ResetStaticComponents ();
				// load history if we have
				if (HistoryStore.HasHistory(StageCtrl.GetStage() - 1))
				{
					if (GlobalConfig.IsDevMode)
					{
						print("Loading player status aft stage " + (StageCtrl.GetStage() - 1));
					}
					StatusRecord record = HistoryStore.GetHistory(StageCtrl.GetStage() - 1);
					ScoreCtrl.currentLevel = record.level;
					ScoreCtrl.score = record.score;
					GunStore.LoadGunStoreStatus(record.storedGuns);
				}
				SceneManager.LoadScene ("stage-intro-scene");
			}
			
		}
	}

	void ResetStaticComponents(){
		ScoreCtrl.Reset();
		WeaponStoreCtrl.Reset();
		GunStore.Reset();
		LifeCtrl.Reset();
	}
}
