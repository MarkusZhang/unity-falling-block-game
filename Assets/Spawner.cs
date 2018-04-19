using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject blockPrefab;
	private Vector2 screenHalfWidth;
	private float secondsBetweenSpawns = 1;
	private float nextSpawnTime;

	// Use this for initialization
	void Start () {
		
		screenHalfWidth = new Vector2 (Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
		//blockPrefab = GameObject.FindGameObjectWithTag ("FallingBlockPrefab");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSpawnTime) {
			secondsBetweenSpawns = (float)Random.Range (0.2f, 0.5f);
			nextSpawnTime = Time.time + secondsBetweenSpawns;
			// spawn a new falling block
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth.x,screenHalfWidth.x),screenHalfWidth.y);
			Instantiate (blockPrefab, position, Quaternion.identity);
		}
	}
}
