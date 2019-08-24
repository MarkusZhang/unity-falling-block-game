using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class AudioOnHit : MonoBehaviour {
	
	public string audioName;
	// Use this for initialization
	void Start () {
		GetComponent<LivingEntity>().OnTakeDamage += onHit;
	}
	
	// Update is called once per frame
	void onHit () {
		if (audioName != null && audioName != "" && AudioManager.instance !=null) {
			AudioManager.instance.PlaySound (audioName);
		}
	}
}
