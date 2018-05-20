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

	int alienShipIdx = 0;
	float startTime;
	State currentState;
	float screenHalfWidth;
	float screenHalfHeight;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		currentState = State.Start;
		screenHalfHeight = Camera.main.orthographicSize;
		screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ().OnDeath += OnPlayerDeath;
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
			boss.GetComponent<LivingEntity> ().OnDeath += OnBossDeath;
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
		Vector3 deathPosition = shipTransform.position;
		float deviateX = 3f;
		if (deathPosition.x + deviateX > screenHalfWidth)
			deviateX = screenHalfWidth - deathPosition.x;
		Instantiate (bulletBlockPrefab, deathPosition + new Vector3 (deviateX, 0, 0), Quaternion.identity);
		if (deathPosition.x - deviateX < - screenHalfWidth) 
			deviateX = screenHalfWidth + deathPosition.x;
		Instantiate (healthBlockPrefab, deathPosition + new Vector3 (-deviateX, 0, 0), Quaternion.identity);
	}

	void OnBossDeath(){
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
