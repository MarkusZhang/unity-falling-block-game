using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance; // singleton

	void Awake(){
		instance = this;
	}

	public void PlaySound(AudioSource src){
		src.Play ();
	}

	public void StopSound(AudioSource src){
		src.Stop ();
	}
		
}
