using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunStatusDisplay : MonoBehaviour {

	public GameObject gunIcon;
	public Text numBulletsRemaining;

	// attach listener
	void Start () {
		GunStore.OnConsumeBullet += UpdateBulletsRemaining;
		GunStore.OnSwitchGun += UpdateIcon;
		GunStore.OnSwitchGun += UpdateBulletsRemaining;
		UpdateIcon();
		UpdateBulletsRemaining();
	}
	
	// Update is called once per frame
	void UpdateIcon () {
		GunType type = GunStore.currentGunType;
		string iconImageName = TypeToImageName (type);
		gunIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(iconImageName);
	}

	void UpdateBulletsRemaining()
	{
		var bulletsLeft = GunStore.GetGunStoreStatus()[GunStore.currentGunType];
		bool isBulletInfinite = bulletsLeft < 0;
		if (isBulletInfinite) {
			numBulletsRemaining.text = "";
		} else {
			numBulletsRemaining.text = "" + bulletsLeft;
		}

	}

	string TypeToImageName(GunType type)
	{
		return GunConstants.typeToIconName[type];
	}

	void OnDestroy(){
		GunStore.OnConsumeBullet -= UpdateBulletsRemaining;
		GunStore.OnSwitchGun -= UpdateIcon;
		GunStore.OnSwitchGun -= UpdateBulletsRemaining;
	}

}


