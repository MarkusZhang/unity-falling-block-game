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
	}
	
	// Update is called once per frame
	void UpdateIcon () {
		GunType type = GunStore.currentGunType;
		string iconImageName = TypeToImageName (type);
		gunIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(iconImageName);
	}

	void UpdateBulletsRemaining(){
		bool isBulletInfinite = GunStore.numBulletsLeft < 0;
		if (isBulletInfinite) {
			numBulletsRemaining.text = "";
		} else {
			numBulletsRemaining.text = "" + GunStore.numBulletsLeft;
		}

	}

	string TypeToImageName(GunType type){
		if (type == GunType.Default) {
			return "images/icons/icon-default-gun";
		} else if (type == GunType.Spray) {
			return "images/icons/icon-spray-gun";
		} else if (type == GunType.Wide) {
			return "images/icons/icon-wide-gun";
		} else if (type == GunType.Ring) {
			return "images/icons/icon-ring-gun";
		} else {
			throw new UnityException (type.ToString () + " is not a valid gun type");
		}
	}

	void OnDestroy(){
		GunStore.OnConsumeBullet -= UpdateBulletsRemaining;
		GunStore.OnSwitchGun -= UpdateIcon;
		GunStore.OnSwitchGun -= UpdateBulletsRemaining;
	}

}


