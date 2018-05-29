using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum State{
	Start,
	Boss,
	End
};

public class GameFlowManager : MonoBehaviour {

	public float[] alienShipTimes;
	public float bossStageTime;
	public GameObject bossPrefab;
	public GameObject alienshipPrefab;

	public GameObject healthBlockPrefab;
	public GameObject bulletBlockPrefab;
	public GameObject healthBarUIPrefab;

	// audios
	public AudioSource bgm;

	int alienShipIdx = 0;
	float startTime;
	State currentState;
	float screenHalfWidth;
	float screenHalfHeight;

	// Use this for initialization
	void Start () {
		Screen.fullScreen = false;

		startTime = Time.time;
		currentState = State.Start;
		screenHalfHeight = Camera.main.orthographicSize;
		screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ().OnDeath += OnPlayerDeath;

		// start playing background audio
		bgm = AudioStore.instance.background;
		AudioManager.instance.PlaySound(bgm);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == State.Start && Time.time - startTime >= bossStageTime) {
			currentState = State.Boss;
			StartBossStage ();
		}

		// spawn alien ship
		if (alienShipIdx < alienShipTimes.Length && Time.time - startTime >= alienShipTimes [alienShipIdx]) {
			alienShipIdx++;
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth,screenHalfWidth),screenHalfHeight + alienshipPrefab.transform.localScale.y);
			GameObject ship = Instantiate (alienshipPrefab, position, Quaternion.identity);
			ship.GetComponent<LivingEntity> ().OnDeath += () => OnAlienShipDeath(ship.transform);
		}
	}

	void StartBossStage(){
		// stop spawning blocks
		Destroy (GameObject.Find ("Spawner"));
		Destroy (GameObject.Find ("WeaponBlockSpawner"));
		// show boss
		GameObject boss = Instantiate(bossPrefab);
		boss.GetComponent<LivingEntity> ().OnDeath += () => OnBossDeath(boss);
		// show boss health bar
		GameObject healthBar = Instantiate(healthBarUIPrefab) as GameObject;
		healthBar.GetComponent<HealthBar> ().AttachToLivingEntity (boss.GetComponent<LivingEntity> ());

		// switch background music
		AudioManager.instance.StopSound(bgm);
		bgm = AudioStore.instance.bossStage;
		AudioManager.instance.PlaySound(bgm);
	}

	void OnAlienShipDeath(Transform shipTransform){
		// calculate points to award
		LivingEntity shipEntity = shipTransform.gameObject.GetComponent<LivingEntity>();
		float shipAliveTime = Time.time - shipEntity.GetStartAliveTime();
		ScoreCtrl.AddScore((int)(shipEntity.startingHealth * 20 / shipAliveTime));

		// randomly generate reward blocks
		Vector3 deathPosition = shipTransform.position;
		float deviateX = 3f;
		float timeForUnitDamage = shipAliveTime / shipEntity.startingHealth;
		if (timeForUnitDamage < 2f) {
			if (deathPosition.x + deviateX > screenHalfWidth)
				deviateX = screenHalfWidth - deathPosition.x;
			Instantiate (bulletBlockPrefab, deathPosition + new Vector3 (deviateX, 0, 0), Quaternion.identity);
		}

		if (timeForUnitDamage < 0.5f) {
			if (deathPosition.x - deviateX < -screenHalfWidth)
				deviateX = screenHalfWidth + deathPosition.x;
			Instantiate (healthBlockPrefab, deathPosition + new Vector3 (-deviateX, 0, 0), Quaternion.identity);
		}
	}

	void OnBossDeath(GameObject boss){
		// calculate points to award
		LivingEntity bossEntity = boss.GetComponent<LivingEntity>();
		float aliveTime = Time.time - bossEntity.GetStartAliveTime();
		ScoreCtrl.AddScore ((int)(bossEntity.startingHealth * 100 / aliveTime));
		StartCoroutine (DelayAndSwitchScene ("game-win", 4));

		// stop music
		AudioManager.instance.StopSound(bgm);
	}

	void OnPlayerDeath(){
		StartCoroutine (DelayAndSwitchScene ("game-over", 4));
	}

	IEnumerator DelayAndSwitchScene(string sceneName,float seconds){
		yield return new WaitForSeconds (seconds);
		currentState = State.End;
		SceneManager.LoadScene (sceneName);
	}
}
