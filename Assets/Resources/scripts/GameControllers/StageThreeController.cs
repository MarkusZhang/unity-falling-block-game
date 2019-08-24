using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageThreeController : AbstractGameFlowCtrl {

	// Use this for initialization
	protected override void showBoss()
	{
		StartCoroutine(defaultShowBossRoutine());
	}

	protected override void onBossDeath()
	{
		// stop music
		AudioManager.instance.StopSound(AudioStore.instance.bossStage);
		AudioManager.instance.PlaySound(AudioStore.instance.stageClear);
		StartCoroutine (delayAndSwitchScene ("game-win", 6));
		Utils.ResetStaticEventListeners();
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
	
}
