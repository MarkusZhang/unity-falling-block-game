using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	float masterVolumePercent = 1f;
	float sfxVolumePercent = 10;
	AudioSource backgroundAudio;

	public static AudioManager instance; // singleton

	void Awake(){
		instance = this;
		backgroundAudio = new AudioSource ();
	}
	
	public void PlaySound(AudioClip clip, Vector3 pos) {
		if (clip != null) {
			AudioSource.PlayClipAtPoint (clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}

	public void PlayMusic(AudioClip clip) {
		if (clip != null) {
			GameObject newMusicSource = new GameObject ("Music BGM");
			backgroundAudio = newMusicSource.AddComponent<AudioSource> ();
			backgroundAudio.clip = clip;
			backgroundAudio.Play ();
			backgroundAudio.volume = 0.05f;
			newMusicSource.transform.parent = transform;
		}
	}
}
