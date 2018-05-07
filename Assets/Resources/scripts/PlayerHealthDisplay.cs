using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour {

	Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ();
		player.OnHealthChange += UpdateDisplay;
	}
	
	void UpdateDisplay(){
		int health = player.GetHealth();

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
