using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlockSpawner : MonoBehaviour {

	public float[] spawnTime = {5f,15f,35f,55f};
	public int[] scoreThreshold = { 10, 30, 80, 210 };
	// spawnTime and spawnTypes have to be of the same length
	private int nextSpawnTimeIndex = 0;
	private float startTime;
	Vector2 screenHalfWidth;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		screenHalfWidth = new Vector2 (Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
		Debug.Assert (spawnTime.Length == scoreThreshold.Length, "spawnTime and scoreThreshold should have same length");
		ScoreCtrl.OnScoreChange += CheckSpawnWeapon;
	}
	
	// Update is called once per frame
	void CheckSpawnWeapon () {
		if (CanSpawnNextWeapon()) {
			GameObject prefab = Resources.Load ("Prefabs/changing-weapon-block") as GameObject;
			Vector2 position = new Vector2(Random.Range(-screenHalfWidth.x,screenHalfWidth.x),screenHalfWidth.y);
			Instantiate(prefab, position, Quaternion.identity);

			nextSpawnTimeIndex = nextSpawnTimeIndex + 1;
		}
	}

	bool CanSpawnNextWeapon(){
		if (nextSpawnTimeIndex < spawnTime.Length) {
			// check time threshold
			if (Time.time >= startTime + spawnTime [nextSpawnTimeIndex]) {
				// check score threshold
				return ScoreCtrl.GetScore() >= scoreThreshold[nextSpawnTimeIndex];
			}
		}
		return false;
	}

	WeaponType GetRandomWeaponType(){
		WeaponType[] allTypes = (WeaponType[])System.Enum.GetValues (typeof(WeaponType));
		int typeIndex = Random.Range (0, allTypes.Length);
		return allTypes [typeIndex];
	}
}
