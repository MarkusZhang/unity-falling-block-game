using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageFourController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject bossPrefab;
	public GameObject healthBarUIPrefab; // use this in showBoss 
	
	public AudioSource bgm;
	public bool switchBgmOnBossWave = true;

	public SubStage[] subStages;

	private int subStageIdx;
	private bool isBossStage;
	
	void Start ()
	{	
		print("half width: " + Camera.main.aspect * Camera.main.orthographicSize);
		print("half height: " + Camera.main.orthographicSize);
		var playerRef = createPlayer();
		
		// equip side guns
		var numSideGuns = GunStore.GetNumSideGuns();
		for (int i = 0; i < numSideGuns; i++)
		{
			playerRef.GetComponent<Player>().EquipSideGun();
		}
		
		if (AudioManager.instance != null && bgm!=null)
		{
			AudioManager.instance.PlaySound(bgm);
		}

		subStageIdx = 0;
		subStages[subStageIdx].gameObject.active = true;
		subStages[subStageIdx].StartSubStage();
		subStages[subStageIdx].OnSubStageEnd += onSubStageEnd;
		subStages[subStageIdx].OnSubStageBoss += onSubStageBoss;
	}

	void onSubStageBoss()
	{
		if (AudioManager.instance != null && switchBgmOnBossWave)
		{
			AudioManager.instance.StopSound(bgm);
			AudioManager.instance.PlaySound(AudioStore.instance.smallBoss);
		}
	}

	void onSubStageEnd()
	{
		if (AudioManager.instance != null)
		{
			AudioManager.instance.StopSound(AudioStore.instance.smallBoss);
		}
		
		if (subStageIdx == subStages.Length - 1)
		{
			Destroy(subStages[subStageIdx]);
			startBoss();
		}
		else
		{
			if (AudioManager.instance != null) // resume bgm
			{
				AudioManager.instance.PlaySound(bgm);
			}
				
			Destroy(subStages[subStageIdx]);
			subStageIdx++;
			subStages[subStageIdx].OnSubStageEnd += onSubStageEnd;
			subStages[subStageIdx].OnSubStageBoss += onSubStageBoss;
			StartCoroutine(delayAndStartSubStage());
		}
	}

	IEnumerator delayAndStartSubStage()
	{
		yield return new WaitForSeconds(2);
		subStages[subStageIdx].gameObject.active = true;
		subStages[subStageIdx].StartSubStage();
	}
	
	protected virtual void onPlayerDeath()
	{
		if (LifeCtrl.HasLifeLeft())
		{
			LifeCtrl.ConsumeLife();
			StartCoroutine(delayAndRecreatePlayer(2f));
		}
		else
		{
			StartCoroutine(delayAndSwitchScene("game-over", 2f));
		}
	}

	void startBoss()
	{
		// change to boss-stage bgm
		if (AudioManager.instance != null)
		{
			AudioManager.instance.StopSound(bgm);
			AudioManager.instance.PlaySound(AudioStore.instance.bossStage);
		}

		StartCoroutine(showBossRoutine());
	}
	
	// co-routine to recreate player
	IEnumerator delayAndRecreatePlayer(float delay)
	{
		yield return new WaitForSeconds(delay);
		createPlayer();
	}

	// create player at the bottom of the scene
	GameObject createPlayer()
	{
		var playerObj = Instantiate(playerPrefab, new Vector3(0, -Camera.main.orthographicSize + 0.1f, 0),
			Quaternion.identity);
		playerObj.GetComponent<Player>().OnDeath += onPlayerDeath;
		return playerObj;
	}
	
	// utility co-routine which you will need to use on boss death
	IEnumerator delayAndSwitchScene(string sceneName,float seconds){
		yield return new WaitForSeconds (seconds);
		SceneManager.LoadScene (sceneName);
	}
	
	IEnumerator showBossRoutine()
	{		
		GameObject boss = Instantiate(this.bossPrefab);
		boss.transform.position = new Vector3(0, Utils.GetTopY() + 3f, 0);
		boss.GetComponent<Collider2D>().enabled = false;

		var bossFadeInTime = 1.5f;
		var alpha = 0f;
		Utils.SetAlphaValue(boss,0);
		
		while (alpha < 1.0f)
		{
			alpha += Time.deltaTime / bossFadeInTime;
			Utils.SetAlphaValue(boss,alpha);
			yield return null;
		}
		boss.GetComponent<Collider2D>().enabled = true;

		// 3. attach listener
		boss.GetComponent<LivingEntity> ().OnDeath += onBossDeath;
		GameObject healthBar = Instantiate(healthBarUIPrefab) as GameObject;
		healthBar.GetComponent<HealthBar> ().AttachToLivingEntity (boss.GetComponent<LivingEntity> ());
		
		// 4. start attack
		if (boss.GetComponent<IControlledAttacker>() != null)
		{
			boss.GetComponent<IControlledAttacker>().StartAttack();
		}
	}

	void onBossDeath()
	{
		// stop music
		AudioManager.instance.StopSound(AudioStore.instance.bossStage);
		AudioManager.instance.PlaySound(AudioStore.instance.stageClear);
		StartCoroutine (delayAndSwitchScene ("game-win", 6));
		Utils.ResetStaticEventListeners();
	}
}
