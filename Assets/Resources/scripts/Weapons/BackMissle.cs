using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMissle : TimedWeapon {

	public GameObject misslePrefab;
	public float fireInterval = 0.5f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		StartCoroutine (RandomFire ());
	}
	
	IEnumerator RandomFire(){
		float screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		float screenHalfHeight = Camera.main.orthographicSize;
		while (true) {
			float spawnX = Random.Range (-0.9f, 0.9f) * screenHalfWidth;
			Vector3 spawnPos = new Vector3 (spawnX, -screenHalfHeight, 0);
			Instantiate (misslePrefab, spawnPos, Quaternion.identity);
			yield return new WaitForSeconds (fireInterval);
		}
	}

	protected override float TotalAliveTime(){
		return 15f;
	}

}
