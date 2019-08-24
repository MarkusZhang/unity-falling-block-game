using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: rename to StartSceneManager
public class GameStart : MonoBehaviour
{	
	// start normal game
	public void StartNormalGame(){
		GlobalConfig.IsEasyMode = false;
		LifeCtrl.SetLife(2);
		SceneManager.LoadScene ("stage-intro-scene");
	}

	public void StartEasyGame()
	{
		LifeCtrl.SetLife(30);
		var defaultGuns = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Ring,300},
			{GunType.Spray,300},
			{GunType.Wide,300},
			{GunType.Track,200}
		};
		GunStore.LoadGunStoreStatus(defaultGuns);
		GlobalConfig.IsEasyMode = true;
		SceneManager.LoadScene ("stage-intro-scene");
	}

	public void StartEasyFinalStage()
	{
		LifeCtrl.SetLife(20);
		ScoreCtrl.AddScore(3000);
		ScoreCtrl.currentLevel = 3;
		ScoreCtrl.awardThresholdIdx = 2;
		var defaultGuns = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Laser,200},
			{GunType.Lightning,20},
			{GunType.Wide,100},
			{GunType.Spray,500},
			{GunType.Ring,500},
			{GunType.Track,200},
			{GunType.Burst,50},
			{GunType.Swing,1000}
		};
		GlobalConfig.IsEasyMode = true;
		GunStore.LoadGunStoreStatus(defaultGuns);
		GunStore.AddSideGun();
		StageCtrl.SetStage(4);
		SceneManager.LoadScene ("stage-intro-scene");
	}
	
	public void StartNormalFinalStage()
	{
		LifeCtrl.SetLife(3);
		ScoreCtrl.AddScore(3000);
		ScoreCtrl.currentLevel = 3;
		ScoreCtrl.awardThresholdIdx = 2;
		var gunStatus = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Laser,200},
			{GunType.Swing,200},
			{GunType.Spray,150},
			{GunType.Burst,30},
			{GunType.Wide,400},
			{GunType.Track,200},
		};
		GlobalConfig.IsEasyMode = false; // disable some rewards
		GunStore.LoadGunStoreStatus(gunStatus);
		GunStore.AddSideGun();
		StageCtrl.SetStage(4);
		SceneManager.LoadScene ("stage-intro-scene");
	}

	public void StartEasyStageThree()
	{
		LifeCtrl.SetLife(20);
		ScoreCtrl.AddScore(3000);
		ScoreCtrl.currentLevel = 3;
		ScoreCtrl.awardThresholdIdx = 2;
		var defaultGuns = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Spray,500},
			{GunType.Wide,500},
			{GunType.Track,200},
			{GunType.Burst,50},
			{GunType.Swing,1000}
		};
		GlobalConfig.IsEasyMode = true;
		GunStore.LoadGunStoreStatus(defaultGuns);
		GunStore.AddSideGun();
		StageCtrl.SetStage(3);
		SceneManager.LoadScene ("stage-intro-scene");
	}
	
	public void StartNormalStageThree()
	{
		LifeCtrl.SetLife(3);
		ScoreCtrl.AddScore(3000);
		ScoreCtrl.currentLevel = 3;
		ScoreCtrl.awardThresholdIdx = 2;
		var gunStatus = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Swing,200},
			{GunType.Spray,150},
			{GunType.Burst,30},
			{GunType.Wide,400},
			{GunType.Track,200},
		};
		GlobalConfig.IsEasyMode = false; // disable some rewards
		GunStore.LoadGunStoreStatus(gunStatus);
		GunStore.AddSideGun();
		StageCtrl.SetStage(3);
		SceneManager.LoadScene ("stage-intro-scene");
	}

	public void StartWarmupGame()
	{
		SceneManager.LoadScene ("practice-scene");
	}

	public void GoToHelp(){
		SceneManager.LoadScene ("help-scene");
	}

	public void BackToMain(){
		SceneManager.LoadScene ("start-scene");
	}
}
