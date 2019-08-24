using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		LifeCtrl.OnHealthChange += UpdateDisplay;
		UpdateDisplay();
	}
	
	void UpdateDisplay(){
		int health = LifeCtrl.GetCurrentHealth();
		
		for (int i = 0; i < transform.childCount; i++) {
			Transform heart = transform.GetChild (i);
			if (i < health) {
				heart.gameObject.SetActive (true);
			} else {
				heart.gameObject.SetActive (false);
			}
		}
	}
}
