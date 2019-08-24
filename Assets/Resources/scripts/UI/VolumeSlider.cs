using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour {

	void Start()
	{
		GetComponent<Slider>().value = GlobalConfig.volume;
	}
	
	public void UpdateVolume()
	{
		GlobalConfig.SetVolume(GetComponent<Slider>().value);
	}

	public void ToggleVisibility()
	{
		if (gameObject.active)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}
	}
}
