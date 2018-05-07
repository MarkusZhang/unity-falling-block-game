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
			GameObject newBlock = Instantiate (blockPrefab, position,  Quaternion.Euler(Vector3.forward * spawnAngle));
			// set a random color
			Color newColor = new Color( Random.value, Random.value, Random.value, 1.0f );
			newBlock.GetComponent<SpriteRenderer> ().color = newColor;
		}

	}
}
