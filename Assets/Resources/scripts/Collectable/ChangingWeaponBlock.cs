using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a weapon block that keeps changing its type
[RequireComponent(typeof(Collectable))]
public class ChangingWeaponBlock : MonoBehaviour {

	public float typeSwitchInterval = 1f; // seconds between weapon switching

	// Use this for initialization
	void Start () {
		WeaponType type = GetRandomType ();
		SetCollectableType (type);
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (type));
		StartCoroutine (SwitchType ());
	}


	IEnumerator SwitchType(){
		while (true) {
			WeaponType type = GetRandomType ();
			//WeaponType type = WeaponType.SliderProtector;
			SetCollectableType (type);
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (type));
			yield return new WaitForSeconds (typeSwitchInterval);
		}
	}

	// 30% probability for bomb, slider, ring, 10% for shield
	WeaponType GetRandomType(){
		float rand = Random.Range (0, 1f);
		if (rand <= 0.3f)
			return WeaponType.BackMissle;
		else if (rand <= 0.6f)
			return WeaponType.SliderProtector;
		else if (rand <= 0.9f)
			return WeaponType.RingProtector;
		else
			return WeaponType.SolidShield;
	}

	string TypeToImageName(WeaponType type)
	{
		return WeaponConstants.typeToBlockImgName[type];
	}

	void SetCollectableType(WeaponType type){
		Collectable c = gameObject.GetComponent<Collectable> ();
		c.cname = type.ToString ();
		c.param = "1";
	}
}
