using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageThreeController : AbstractGameFlowCtrl {

	public GameObject[] rewardsUponBossDeath;
	
	// Use this for initialization
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

	void TempSettings()
	{
		var gunStatus = new Dictionary<GunType, int>
		{
			{GunType.Default,-1},
			{GunType.Swing,212},
			{GunType.Spray,138},
			{GunType.Burst,30},
			{GunType.Wide,410},
			{GunType.Track,214},
		};
		GunStore.LoadGunStoreStatus(gunStatus);
		
		ScoreCtrl.currentLevel = 3;
		ScoreCtrl.score = 3335;
		GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ().OnLevelUp();
		GunStore.AddSideGun();
		GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ().EquipSideGun();
//		configs = new WaveConfig[]{configs[configs.Length-1]};
	}
	
	protected override void Start ()
	{
		base.Start();
//		TempSettings();
	}
	
	IEnumerator delayAndPlayStageClear()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.instance.PlaySound(AudioStore.instance.stageClear);
	}
	
}
