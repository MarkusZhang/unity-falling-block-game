using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// utility class for storing all audio references
public class AudioStore : MonoBehaviour {

	public static AudioStore instance;

	public AudioSource background;
	public AudioSource bossStage;

	public AudioSource collection;
	public AudioSource bossDeath;
	public AudioSource spaceshipDeath;
	public AudioSource hitPlayer;
	public AudioSource hitEnemy;
	public AudioSource playerDeath;

	public AudioSource ringBullet;
	public AudioSource defaultBullet;
	public AudioSource wideBullet;
	public AudioSource sprayBullet;

	public AudioSource switchGun;

	Dictionary<string,AudioSource> nameToSrc;

	// Use this for initialization
	void Awake () {
		instance = this;
		nameToSrc = new Dictionary<string,AudioSource> {
			{"default-bullet",defaultBullet},
			{"ring-bullet",ringBullet},
			{"wide-bullet",wideBullet},
			{"spray-bullet",sprayBullet},

			{"boss-death",bossDeath},
			{"player-death",playerDeath},
			{"alienship-death",spaceshipDeath},
			{"player-hit",hitPlayer},
			{"enemy-hit",hitEnemy},
		};
	}

	public AudioSource GetAudioSourceByName(string name){
		AudioSource src;
		nameToSrc.TryGetValue (name, out src);
		if (src == null) {
			throw new UnityException (name + " is not a valid audio source name");
		}
		return src;
	}

}
