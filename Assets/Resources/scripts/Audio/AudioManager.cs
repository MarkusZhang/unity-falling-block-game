using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance; // singleton
	public Transform srcContainer;

	void Awake(){
		instance = this;
	}

	void Start()
	{
		GlobalConfig.OnVolumeChange += UpdateVolume;
		UpdateVolume();
	}

	private void OnDestroy()
	{
		GlobalConfig.OnVolumeChange -= UpdateVolume;
	}

	public void PlaySound(AudioSource src){
		if (src != null)
		{
			src.Play ();
		}
	}

	public void PlaySound(string audioName)
	{
		if (AudioStore.instance != null)
		{
			var audioSrc = AudioStore.instance.GetAudioSourceByName(audioName);
			if (audioSrc != null)
			{
				PlaySound(audioSrc);
			}
		}
	}

	public void StopSound(AudioSource src){
		if (src != null)
		{
			src.Stop ();
		}
	}

	public void SetVolume(float vol)
	{
		for (int i = 0; i < srcContainer.childCount; i++)
		{
			var child = srcContainer.GetChild(i);
			var src = child.gameObject.GetComponent<AudioSource>(); 
			if (src != null)
			{
				src.volume = vol;
			}
		}
	}

	public void UpdateVolume()
	{
		SetVolume(GlobalConfig.volume);
	}
		
}
