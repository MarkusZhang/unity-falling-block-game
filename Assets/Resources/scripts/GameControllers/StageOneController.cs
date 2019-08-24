using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class StageOneController : AbstractGameFlowCtrl
{
	public GameObject bossImage;
	public GameObject[] rewardsUponBossDeath;
	
	// Use this for initialization
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
			{GunType.Ring,145},
			{GunType.Spray,197},
			{GunType.Wide,141},
			{GunType.Track,50}
		};
		GunStore.LoadGunStoreStatus(gunStatus);
		ScoreCtrl.currentLevel = 2;
		ScoreCtrl.score = 950;
		GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ().OnLevelUp();
	}

	protected override void showBoss()
	{
		if (AudioManager.instance != null && AudioStore.instance!=null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.bossLaugh);
		}
		
		StartCoroutine(ShowBoss());
	}

	void onBossDeath(GameObject boss)
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

	protected override bool isWaitConditionMet(int curWaveIdx)
	{
		var cannotWaitFurther = Time.time > nextWaveTime + maxExtraWaveWaitTime;
		if (cannotWaitFurther)
		{
			return true;
		}

		return base.isWaitConditionMet(curWaveIdx);
	}

	IEnumerator delayAndPlayStageClear()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.instance.PlaySound(AudioStore.instance.stageClear);
	}
	
	IEnumerator ShowBoss()
	{
		// pre-appear animation
		var y = 3;
		GameObject bossImg = Instantiate(bossImage, new Vector3(0, 3, 0), Quaternion.identity);
		var flashTimes = 20;
		for (int i = 0; i < flashTimes; i++)
		{
			var screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
			var x = Random.Range(-screenHalfWidth, screenHalfWidth);
			bossImg.transform.position = new Vector3(x,y,0);
			yield return new WaitForSeconds(0.1f);
		}
		Destroy(bossImg);
		
		// show boss
		GameObject boss = Instantiate(this.bossPrefab);
		boss.GetComponent<LivingEntity> ().OnDeath += () => onBossDeath(boss);
		// show boss health bar
		GameObject healthBar = Instantiate(healthBarUIPrefab) as GameObject;
		healthBar.GetComponent<HealthBar> ().AttachToLivingEntity (boss.GetComponent<LivingEntity> ());
	}
}
