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
	public AudioClip backgroundMusic;

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
		AudioManager.instance.PlayMusic(backgroundMusic);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == State.Start && Time.time - startTime >= bossStageTime) {
			currentState = State.Boss;
			// stop spawning blocks
			Destroy (GameObject.Find ("Spawner"));
			Destroy (GameObject.Find ("WeaponBlockSpawner"));
			// show boss
			GameObject boss = Instantiate(bossPrefab);
			boss.GetComponent<LivingEntity> ().OnDeath += () => OnBossDeath(boss);
			// show boss health bar
			GameObject healthBar = Instantiate(healthBarUIPrefab) as GameObject;
			healthBar.GetComponent<HealthBar> ().AttachToLivingEntity (boss.GetComponent<LivingEntity> ());
		}

		// spawn alien ship
		if (alienShipIdx < alienShipTimes.Length && Time.time - startTime >= alienShipTimes [alienShipIdx]) {
			alienShipIdx++;
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth,screenHalfWidth),screenHalfHeight + alienshipPrefab.transform.localScale.y);
			GameObject ship = Instantiate (alienshipPrefab, position, Quaternion.identity);
			ship.GetComponent<LivingEntity> ().OnDeath += () => OnAlienShipDeath(ship.transform);
		}
	}

	void OnAlienShipDeath(Transform shipTransform){
		// calculate points to award
		LivingEntity shipEntity = shipTransform.gameObject.GetComponent<LivingEntity>();
		float shipAliveTime = Time.time - shipEntity.GetStartAliveTime();
		ScoreCtrl.AddScore((int)(shipEntity.startingHealth * 20 / shipAliveTime));

		// generate reward blocks
		Vector3 deathPosition = shipTransform.position;
		float deviateX = 3f;
		if (deathPosition.x + deviateX > screenHalfWidth)
			deviateX = screenHalfWidth - deathPosition.x;
		Instantiate (bulletBlockPrefab, deathPosition + new Vector3 (deviateX, 0, 0), Quaternion.identity);
		if (deathPosition.x - deviateX < - screenHalfWidth) 
			deviateX = screenHalfWidth + deathPosition.x;
		Instantiate (healthBlockPrefab, deathPosition + new Vector3 (-deviateX, 0, 0), Quaternion.identity);
	}

	void OnBossDeath(GameObject boss){
		// calculate points to award
		LivingEntity bossEntity = boss.GetComponent<LivingEntity>();
		float aliveTime = Time.time - bossEntity.GetStartAliveTime();
		ScoreCtrl.AddScore ((int)(bossEntity.startingHealth * 100 / aliveTime));

		AudioManager.instance.PlaySound (AudioStore.instance.bossDeath, transform.position);
		StartCoroutine (DelayAndSwitchScene ("game-win", 2));
	}

	void OnPlayerDeath(){
		StartCoroutine (DelayAndSwitchScene ("game-over", 2));
	}

	IEnumerator DelayAndSwitchScene(string sceneName,float seconds){
		yield return new WaitForSeconds (seconds);
		currentState = State.End;
		SceneManager.LoadScene (sceneName);
	}
}
