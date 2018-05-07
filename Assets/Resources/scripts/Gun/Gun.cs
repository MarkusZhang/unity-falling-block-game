using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform[] muzzles;
	public float defaultShootInterval;

	public int shootLimit; // negative number indicates no limit
	public event System.Action OnShootLimitReached;

	private float lastShootTime;
	float shootInterval;

	// Use this for initialization
	void Start () {
		lastShootTime = Time.time;
		shootInterval = defaultShootInterval;
	}
	
	public void Shoot(){
		if (Time.time - lastShootTime > shootInterval) {
			lastShootTime = Time.time;
			foreach (Transform muzzle in muzzles) {
				GameObject bullet = Instantiate (bulletPrefab, muzzle.position, muzzle.rotation);
			}

			if (shootLimit > 0) {
				shootLimit--;
			}

			if (shootLimit == 0) {
				if (OnShootLimitReached != null) {
					OnShootLimitReached ();
				}
			}
		}
	}

	public int GetNumShotsLeft(){
		return shootLimit;
	}

	public void SetNumShotLeft(int value){
		shootLimit = value;
	}

	public void ChangeShootSpeedByRatio(float ratio){
		Debug.Assert (ratio > 0, "ratio must be positive");
		shootInterval = defaultShootInterval / ratio;
	}
}
