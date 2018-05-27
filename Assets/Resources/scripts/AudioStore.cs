using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// utility class for storing all audio references
public class AudioStore : MonoBehaviour {

	public static AudioStore instance;

	public AudioClip collection;
	public AudioClip bossDeath;
	public AudioClip spaceshipDeath;
	public AudioClip hitPlayer;


	// Use this for initialization
	void Awake () {
		instance = this;
	}

}
