using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class AudioOnDeath : MonoBehaviour
{
	public string audioName;
	// Use this for initialization
	void Start ()
	{
		GetComponent<LivingEntity>().OnDeath += onDeath;
	}

	void onDeath()
	{
		if (audioName != null && audioName != "" && AudioManager.instance !=null) {
			AudioManager.instance.PlaySound (audioName);
		}
	}
}
