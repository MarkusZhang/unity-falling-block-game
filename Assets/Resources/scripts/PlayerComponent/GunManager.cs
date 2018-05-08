using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

	// about current gun
	Gun currentGun;
	GunType currentGunType;
	GameObject currentGunObject;

	Dictionary<GunType,int> allGuns;
	Transform parentTransform;
	float relativeShootSpeed = 1;

	public void Init(Transform playerTransform){
		parentTransform = playerTransform;
		allGuns = new Dictionary<GunType,int> ();
		allGuns.Add (GunType.Default, -1);
		allGuns.Add (GunType.Spray, 100);
		allGuns.Add (GunType.Wide, 100);
		// use default gun
		SetCurrentGun(GunType.Default);
	}
	
	public void Shoot(){
		currentGun.Shoot ();
	}

	// switch to next gun in `allGuns`
	public void SwitchGun(){
		GunType[] allTypes = new List<GunType> (allGuns.Keys).ToArray();

		for (int i = 0; i < allTypes.Length; i++) {
			if (allTypes[i] == currentGunType) {
				GunType newGunType = allTypes [(i + 1) % allTypes.Length];
				SwitchGun (newGunType);
				break;
			}
		}
	}

	public void SwitchGun(GunType type){
		if (allGuns.ContainsKey (type)) {
			// do switch
			// save shots left for current gun
			allGuns[currentGunType] = currentGun.GetNumShotsLeft();
			Destroy (currentGunObject);
			// instantiate next gun
			SetCurrentGun(type);
			// config the gun
			int numShotsLeft;
			allGuns.TryGetValue(type, out numShotsLeft);
			currentGun.SetNumShotLeft (numShotsLeft);
			currentGun.OnShootLimitReached += OnShootLimitReached;
		}
	}

	void OnShootLimitReached(){
		allGuns.Remove (currentGunType);
		SetCurrentGun(GunType.Default);
	}

	void SetCurrentGun(GunType type){
		currentGunObject = TypeToGunObject(type,parentTransform);
		currentGun = currentGunObject.GetComponent<Gun> ();
		currentGun.ChangeShootSpeedByRatio (relativeShootSpeed);
		currentGunType = type;
	}

	GameObject TypeToGunObject(GunType type, Transform parent){
		GameObject gunObject;
		if (type == GunType.Default) {
			gunObject = Resources.Load ("Prefabs/guns/default-gun") as GameObject;
		} else if (type == GunType.Spray) {
			gunObject = Resources.Load ("Prefabs/guns/spray-gun") as GameObject;
		} else { // assume wide gun
			gunObject = Resources.Load ("Prefabs/guns/wide-gun") as GameObject;
		}
		return Instantiate (gunObject, parent);
	}

	public void SetRelativeShootSpeed(float value){
		relativeShootSpeed = value;
		currentGun.ChangeShootSpeedByRatio (relativeShootSpeed);
	}
}
