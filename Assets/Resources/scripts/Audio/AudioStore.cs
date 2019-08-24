using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// utility class for storing all audio references
public class AudioStore : MonoBehaviour {

	public static AudioStore instance;

	public AudioSource background;
	public AudioSource bossStage;
	public AudioSource smallBoss;

	public AudioSource collection;
	public AudioSource bossDeath;
	public AudioSource stageClear;
	public AudioSource spaceshipDeath;
	public AudioSource hitPlayer;
	public AudioSource hitEnemy;
	public AudioSource enemyDead;
	public AudioSource playerDeath;

	public AudioSource ringBullet;
	public AudioSource defaultBullet;
	public AudioSource wideBullet;
	public AudioSource sprayBullet;
	public AudioSource trackBullet;
	public AudioSource lightningBullet;
	public AudioSource energyBurst;

	public AudioSource bossMassShoot;
	public AudioSource bossCut;
	public AudioSource bossShootMissile;
	public AudioSource bossLaugh;

	public AudioSource laba;
	
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
			{"track-bullet",trackBullet},
			{"energy-burst",energyBurst},
			{"lightning-bullet",lightningBullet},

			{"boss-death",bossDeath},
			{"stage-clear",stageClear},
			{"player-death",playerDeath},
			{"alienship-death",spaceshipDeath},
			{"player-hit",hitPlayer},
			{"enemy-hit",hitEnemy},
			{"enemy-dead",enemyDead},
			
			{"boss-mass-shoot",bossMassShoot},
			{"boss-cut",bossCut},
			{"boss-shoot-missile",bossShootMissile}
			
		};
	}

	// returns null if no audioSource found
	public AudioSource GetAudioSourceByName(string name){
		AudioSource src;
		nameToSrc.TryGetValue (name, out src);
		return src;
	}

}
