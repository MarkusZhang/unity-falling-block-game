using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject blockPrefab;
	public GameObject bonusPrefab;
	public GameObject addProjectilePrefab;
	public float secondsBetweenSpawnsMin = 0.1f;
	public float secondsBetweenSpawnsMax = 0.6f;
	public float spawnAngleRangeMax = 45;

	private Vector2 screenHalfWidth;

	private float secondsBetweenSpawns;
	private float nextSpawnTime;
	private int bonus1Threshold = 15;
	private bool bonus1given = false;
	private int bonus2Threshold = 30;
	private bool bonus2given = false;

	// Use this for initialization
	void Start () {		
		screenHalfWidth = new Vector2 (Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSpawnTime) {
			float diff = Difficulty.GetDifficultyPercent ();
			secondsBetweenSpawns = Mathf.Lerp (secondsBetweenSpawnsMax, secondsBetweenSpawnsMin, diff);
			nextSpawnTime = Time.time + secondsBetweenSpawns;
			// spawn a new falling block
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth.x,screenHalfWidth.x),screenHalfWidth.y);
			// select random rotation angle
			float spawnAngleMax = Mathf.Lerp (0, spawnAngleRangeMax, diff);
			float spawnAngle = Random.Range (-spawnAngleMax, spawnAngleMax);
			Instantiate (blockPrefab, position,  Quaternion.Euler(Vector3.forward * spawnAngle));
		}

		// if player reaches certain points, then give bonus
		int playerScore = FindObjectOfType<ScoreCtrl>().getScore();
		if (playerScore == bonus1Threshold && !bonus1given) {
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth.x,screenHalfWidth.x),screenHalfWidth.y);
			Instantiate (bonusPrefab, position, Quaternion.identity);
			bonus1given = true;
		}

		if (playerScore == bonus2Threshold && ! bonus2given) {
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth.x,screenHalfWidth.x),screenHalfWidth.y);
			Instantiate (addProjectilePrefab, position, Quaternion.identity);
			bonus2given = true;
		}
	}
}
