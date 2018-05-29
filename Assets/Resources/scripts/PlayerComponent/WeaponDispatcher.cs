using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDispatcher : MonoBehaviour {

	public static WeaponDispatcher instance;

	public Transform player;
	public GameObject ringProtectorPrefab;
	public GameObject shieldPrefab;
	public GameObject sliderProtectorPrefab;
	public GameObject backMisslePrefab;

	void Awake(){
		instance = this;
		player = GameObject.FindGameObjectWithTag ("player").transform;
	}

	public void DispatchWeapon(WeaponType type){
		switch (type){
			case WeaponType.RingProtector:
				Instantiate (ringProtectorPrefab, player);
				break;
			case WeaponType.BackMissle:
				Instantiate (backMisslePrefab);
					break;
			case WeaponType.SolidShield:
				Instantiate (shieldPrefab, player);
				break;
			case WeaponType.SliderProtector:
				Instantiate (sliderProtectorPrefab);
				break;

		}

	}

}
