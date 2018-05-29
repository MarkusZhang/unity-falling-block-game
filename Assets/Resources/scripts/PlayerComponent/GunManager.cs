using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: change this to a singleton class
public class GunManager : MonoBehaviour {

	// about current gun
	public Gun currentGun;
	public GunType currentGunType;
	public GameObject currentGunObject;

	Transform parentTransform;
	float relativeShootSpeed = 1;

	void Start(){
		GunStore.AddGun (GunType.Default, -1);
		GunStore.AddGun (GunType.Ring, 25);
		GunStore.AddGun (GunType.Spray, 25);
		GunStore.AddGun (GunType.Wide, 25);
		// use default gun
		SetCurrentGun(GunType.Default);
	}
	
	public void Shoot(){
		if (currentGun.Shoot ()) {
			GunStore.ConsumeBullet ();
		}
	}

	// switch to next gun in `allGuns`
	public void SwitchGun(){
		GunType type = GunStore.SwitchGun ();
		_SwitchGun (type);
	}

	public void SwitchGun(GunType type){
		GunStore.SwitchGun (type);
		_SwitchGun (type);
	}

	// internal switching procedure
	void _SwitchGun(GunType type){
		if (type != currentGunType) {
			Destroy (currentGunObject);
			SetCurrentGun (type);
			AudioManager.instance.PlaySound (AudioStore.instance.switchGun);
		}
	}

	void OnShootLimitReached(){
		Destroy (currentGunObject);
		SetCurrentGun(GunStore.currentGunType);
	}

	void SetCurrentGun(GunType type){
		currentGunObject = TypeToGunObject(type);
		currentGun = currentGunObject.GetComponent<Gun> ();
		currentGun.ChangeShootSpeedByRatio (relativeShootSpeed);
		currentGunType = type;
		GunStore.OnBulletLimitReached += OnShootLimitReached;
	}

	GameObject TypeToGunObject(GunType type){
		GameObject gunObject;
		Transform parent = GameObject.Find("player").transform;
		Debug.Assert (parent != null, "GunManager: cannot find the player");
		if (type == GunType.Default) {
			gunObject = Resources.Load ("Prefabs/guns/default-gun") as GameObject;
		} else if (type == GunType.Spray) {
			gunObject = Resources.Load ("Prefabs/guns/spray-gun") as GameObject;
		} else if (type == GunType.Wide) { // assume wide gun
			gunObject = Resources.Load ("Prefabs/guns/wide-gun") as GameObject;
		} else if (type == GunType.Ring) {
			gunObject = Resources.Load ("Prefabs/guns/ring-gun") as GameObject;
		} else {
			throw new UnityException (type + " is not a valid guntype");
		}
		return Instantiate (gunObject, parent);
	}

	public void SetRelativeShootSpeed(float value){
		relativeShootSpeed = value;
		currentGun.ChangeShootSpeedByRatio (relativeShootSpeed);
	}
}
