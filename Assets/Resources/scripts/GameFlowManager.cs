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

	public float bossStageTime;
	public GameObject bossPrefab;

	float startTime;
	State currentState;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		currentState = State.Start;
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
	}

	void OnBossDeath(){
		currentState = State.End;
		SceneManager.LoadScene ("game-win");
	}

	void OnPlayerDeath(){
		currentState = State.End;
		SceneManager.LoadScene ("game-over");
	}
}
