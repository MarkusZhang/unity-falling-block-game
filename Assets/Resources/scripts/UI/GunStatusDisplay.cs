using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunStatusDisplay : MonoBehaviour {

	public GameObject gunIcon;
	public Text numBulletsRemaining;

	GunManager gunManager;

	// attach listener
	void Start () {
		Player p = GameObject.Find ("player").GetComponent<Player> ();
		gunManager = GameObject.Find ("player").GetComponent<Player> ().gunManager;
		gunManager.OnShoot += UpdateBulletsRemaining;
		gunManager.OnSwitchGun += UpdateIcon;
		gunManager.OnSwitchGun += UpdateBulletsRemaining;
		print ("GunStatusDisplay.Start() finishes");
	}
	
	// Update is called once per frame
	void UpdateIcon () {
		GunType type = gunManager.currentGunType;
		string iconImageName = TypeToImageName (type);
		gunIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>(iconImageName);
	}

	void UpdateBulletsRemaining(){
		GunType type = gunManager.currentGunType;
		if (type == GunType.Default) {
			numBulletsRemaining.text = "";
		} else {
			Gun gun = gunManager.currentGun;
			numBulletsRemaining.text = "" + gun.GetNumShotsLeft ();
		}

	}

	string TypeToImageName(GunType type){
		if (type == GunType.Default) {
			return "images/icons/icon-default-gun";
		} else if (type == GunType.Spray) {
			return "images/icons/icon-spray-gun";
		} else { // Wide
			return "images/icons/icon-wide-gun";
		}
	}
}
