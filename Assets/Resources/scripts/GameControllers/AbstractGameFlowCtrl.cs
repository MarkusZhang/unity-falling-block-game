using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WaveConfig
{
	public float waveTime;
	public bool waitTillFinish;
	public GameObject wavePrefab;
}

// a framework for sequence-of-enemy-wave game flow controller
public abstract class AbstractGameFlowCtrl : MonoBehaviour {

	// for spawning enemy waves
	public WaveConfig[] configs;
	public GameObject playerPrefab;
	public GameObject bossPrefab;
	public GameObject healthBarUIPrefab; // use this in showBoss 
	
	public AudioSource bgm;
	public bool switchBgmOnBossWave = true;

	public bool speedUpScroll;
	public float speedUpPerWave;
	
	protected float maxExtraWaveWaitTime = 5; // to prevent game flow from being stuck
	protected int nextWaveIdx = 0;
	protected float nextWaveTime;
	
	protected bool[] waveFinished;
	protected Object lockWaveFinished = new Object();

	protected bool isBossStage;
	
	protected abstract void showBoss();
	
	protected virtual void Start ()
	{
		nextWaveTime = 0;
		waveFinished = new bool[configs.Length];
		
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
	}
	
	protected virtual void Update () {
		// spawn enemy waves
		if (nextWaveIdx < configs.Length)
		{
			if (Time.time > nextWaveTime && isWaitConditionMet(nextWaveIdx - 1))
			{
				spawnNewWave(nextWaveIdx);

				if (speedUpScroll && WrapAroundBackground.instance!=null)
				{
					WrapAroundBackground.instance.IncrScrollSpeed(speedUpPerWave);
				}
			
				if (nextWaveIdx < configs.Length)
					nextWaveTime = Time.time + configs[nextWaveIdx].waveTime;
				
				nextWaveIdx++;
				
			}
		}
		
		// this is to prevent boss not showing due to errors in last enemy wave
		if (nextWaveIdx == configs.Length && Time.time > nextWaveTime + maxExtraWaveWaitTime && !isBossStage)
		{
			isBossStage = true;
			startBoss();
		}
	}

	protected virtual void spawnNewWave(int waveIdx)
	{
		var newWaveObj = Instantiate(configs[waveIdx].wavePrefab);
		var newWave = newWaveObj.GetComponent<AbstractEnemyWave>();
		Debug.Assert(newWave!=null);
		if (configs[waveIdx].waitTillFinish && waveIdx < configs.Length - 1)
		{
			var i = waveIdx; // need to do this reassign to avoid passing nextWaveIdx by ref
			newWave.OnAllEnemiesDestroyed += () => onSingleWaveFinished(i);
		}

		// if boss wave, we need to play small boss bgm
		if (newWave.GetComponent<BossWave>() != null && switchBgmOnBossWave)
		{
			if (AudioManager.instance != null)
			{
				AudioManager.instance.StopSound(bgm);
				AudioManager.instance.PlaySound(AudioStore.instance.smallBoss);
			}
			newWave.OnAllEnemiesDestroyed += resumeBgm;
		}
	}

	void resumeBgm()
	{
		if (AudioManager.instance != null)
		{
			AudioManager.instance.StopSound(AudioStore.instance.smallBoss);
			AudioManager.instance.PlaySound(bgm);
		}
	}
	
	protected virtual bool isWaitConditionMet(int curWaveIdx)
	{
		if (curWaveIdx < 0)
			return true;
		return !configs[curWaveIdx].waitTillFinish || waveFinished[curWaveIdx];
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

	protected virtual void startBoss()
	{
		// change to boss-stage bgm
		if (AudioManager.instance != null)
		{
			AudioManager.instance.StopSound(bgm);
			AudioManager.instance.PlaySound(AudioStore.instance.bossStage);
		}
		
		showBoss();
	}
	
	// co-routine to recreate player
	protected IEnumerator delayAndRecreatePlayer(float delay)
	{
		yield return new WaitForSeconds(delay);
		createPlayer();
	}

	protected GameObject createPlayer()
	{
		var playerObj = Instantiate(playerPrefab, new Vector3(0, -Camera.main.orthographicSize + 0.1f, 0),
			Quaternion.identity);
		playerObj.GetComponent<Player>().OnDeath += onPlayerDeath;
		return playerObj;
	}
	
	// utility co-routine which you will need to use on boss death
	protected IEnumerator delayAndSwitchScene(string sceneName,float seconds){
		yield return new WaitForSeconds (seconds);
		SceneManager.LoadScene (sceneName);
	}
	
	// co-routine to move on to next stage
	protected IEnumerator delayAndNextStage(float seconds)
	{
		yield return new WaitForSeconds (seconds);
		// save player status
		if (GlobalConfig.IsDevMode)
		{
			print("Saving status: score = " + ScoreCtrl.GetScore() + ", level = " + ScoreCtrl.GetLevel() + ", gunStore = " + GunStore.GetGunStoreStatusStr());
		}
		HistoryStore.WriteHistory(StageCtrl.GetStage(),GunStore.GetGunStoreStatus(),ScoreCtrl.GetLevel(),ScoreCtrl.GetScore());
		// move to next stage
		StageCtrl.NextStage();
		Utils.ResetStaticEventListeners();
		SceneManager.LoadScene ("stage-intro-scene");
	}


	protected IEnumerator defaultShowBossRoutine()
	{
		// 1. dim background
		if (WrapAroundBackground.instance != null)
		{
			WrapAroundBackground.instance.FadeOut(1.5f);
		}
		
		yield return new WaitForSeconds(2f);
		
		// 2. show boss, fade in
		GameObject boss = Instantiate(this.bossPrefab);
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

	protected virtual void onBossDeath()
	{
		print("implement this");
	}
	
	void onSingleWaveFinished(int waveIdx)
	{
		lock (lockWaveFinished)
		{
			Debug.Assert(waveIdx < waveFinished.Length);
			Debug.Assert(waveIdx > 0);
			waveFinished[waveIdx] = true;
		}
	}
}
