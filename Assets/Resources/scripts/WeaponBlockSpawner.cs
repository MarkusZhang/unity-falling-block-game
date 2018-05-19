using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlockSpawner : MonoBehaviour {

	float[] spawnTime = {5f,10f,15f,25f,35f,45f,60f};
	int[] scoreThreshold = { 5, 15, 30, 40, 55, 60, 70 };
	WeaponType?[] spawnTypes = {null, 
		WeaponType.SolidShield, WeaponType.ScreenBomber,WeaponType.SliderProtector, null, null, null};
	// spawnTime and spawnTypes have to be of the same length
	private int nextSpawnTimeIndex = 0;
	private float startTime;
	Vector2 screenHalfWidth;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		screenHalfWidth = new Vector2 (Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
		Debug.Assert (spawnTime.Length == scoreThreshold.Length, "spawnTime and scoreThreshold should have same length");
		Debug.Assert (spawnTime.Length == spawnTypes.Length, "spawnTime and spawnTypes should have same length");
	}
	
	// Update is called once per frame
	void Update () {
		if (CanSpawnNextWeapon()) {
			// spawn new weapon block
//			WeaponType type;
//			if (spawnTypes [nextSpawnTimeIndex] == null) {
//				type = GetRandomWeaponType ();
//			} else {
//				type = (WeaponType)spawnTypes [nextSpawnTimeIndex];
//			}
//			string prefabName = GetPrefabName (type);
//			GameObject prefab = Resources.Load ("Prefabs/weapon-blocks/" + prefabName) as GameObject;
//			Debug.Assert (prefab != null, "prefab: '" + prefabName + "' is null");
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

	string GetPrefabName(WeaponType type){
		if (type == WeaponType.RingProtector)
			return "ring-protector";
		else if (type == WeaponType.ScreenBomber)
			return "screen-bomber";
		else if (type == WeaponType.SliderProtector)
			return "slider-protector";
		else if (type == WeaponType.SolidShield)
			return "solid-shield";
		else
			throw new UnityException ("invalid type");
	}
}
