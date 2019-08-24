using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform[] muzzles;
	public float defaultShootInterval;

	public int shootLimit; // negative number indicates no limit
	public event System.Action OnShootLimitReached;

	public string shootAudioName;

	private float lastShootTime;
	float shootInterval;
	public float bulletScale = 1;
	public int bulletDamageMultiplier = 1;

	// Use this for initialization
	protected virtual void Start () {
		lastShootTime = Time.time;
		bool isShootIntervalAssigned = shootInterval != 0;
		if (!isShootIntervalAssigned) {
			shootInterval = defaultShootInterval;
		}
	}

	// return true if the shot is made
	public virtual bool Shoot(){
		if (Time.time - lastShootTime > shootInterval) {
			lastShootTime = Time.time;
			foreach (Transform muzzle in muzzles) {
				InstantiateBullet(muzzle);
			}

			// shoot sound
			if (shootAudioName != null && shootAudioName != "" && AudioManager.instance != null) {
				AudioManager.instance.PlaySound (shootAudioName);
			}
			// check shoot limit
			if (shootLimit > 0) {
				shootLimit--;
			}

			if (shootLimit == 0) {
				if (OnShootLimitReached != null) {
					OnShootLimitReached ();
				}
			}

			return true;
		} else {
			return false;
		}
	}

	public virtual void InstantiateBullet(Transform muzzle)
	{
		GameObject bullet = Instantiate (bulletPrefab, muzzle.position, muzzle.rotation);
		bullet.transform.localScale = bulletScale * bullet.transform.localScale;
		var bulletDamager = bullet.GetComponent<Damager>();
		if (bulletDamager != null)
		{
			bulletDamager.damage *= bulletDamageMultiplier;
		}
	}

	public void ChangeShootSpeedByRatio(float ratio){
		Debug.Assert (ratio > 0, "ratio must be positive");
		shootInterval = defaultShootInterval / ratio;
	}
}
