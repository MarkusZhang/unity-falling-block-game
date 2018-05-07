using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDispatcher : MonoBehaviour {

	public Transform player;
	public GameObject ringProtectorPrefab;
	public GameObject shieldPrefab;
	public GameObject sliderProtectorPrefab;

	public void Init(){
		player = GameObject.FindGameObjectWithTag ("player").transform;
		ringProtectorPrefab = Resources.Load ("Prefabs/ring-protector") as GameObject;
		shieldPrefab = Resources.Load ("Prefabs/shield") as GameObject;
		sliderProtectorPrefab = Resources.Load ("Prefabs/slider") as GameObject;
	}

	public void DispatchWeapon(WeaponType type){
		switch (type){
			case WeaponType.RingProtector:
				Instantiate (ringProtectorPrefab, player);
				break;
			case WeaponType.ScreenBomber:
				BombScreen ();
				break;
			case WeaponType.SolidShield:
				Instantiate (shieldPrefab, player);
				break;
			case WeaponType.SliderProtector:
				Instantiate (sliderProtectorPrefab);
				break;

		}

	}

	void BombScreen(){
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("falling-block");
		foreach (GameObject block in blocks) {
			block.GetComponent<FallingBlock> ().ExplodeAndDestroy ();
			ScoreCtrl.AddScore ();
		}
	}
}
