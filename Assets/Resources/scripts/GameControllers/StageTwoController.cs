using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class StageTwoController : AbstractGameFlowCtrl {
	
	public GameObject[] rewardsUponBossDeath;
	
	protected override void Start ()
	{
		base.Start();
//		TempSettings();
	}

	// for testing and debugging purpose, not used in production
	void TempSettings()
	{
		var gunStatus = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Swing,-1},
			{GunType.Burst,145},
			{GunType.Spray,197},
			{GunType.Wide,141},
			{GunType.Track,50},
		};
		GunStore.LoadGunStoreStatus(gunStatus);
//		ScoreCtrl.currentLevel = 2;
//		ScoreCtrl.score = 535;
//		GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ().OnLevelUp();

//		configs = new WaveConfig[]{configs[configs.Length-1]};
	}
	
	protected override void showBoss()
	{
		StartCoroutine(defaultShowBossRoutine());
	}

	
	protected override void onBossDeath()
	{
		// generate some reward
		if (RewardSpawner.instance != null)
		{
			RewardSpawner.instance.SpawnRewards(rewardsUponBossDeath);
		}
		
		// stop music
		AudioManager.instance.StopSound(AudioStore.instance.bossStage);
		StartCoroutine(delayAndPlayStageClear());
		
		// move on to next stage
		StartCoroutine (delayAndNextStage(7));
	}
	
	IEnumerator delayAndPlayStageClear()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.instance.PlaySound(AudioStore.instance.stageClear);
	}
	
}
