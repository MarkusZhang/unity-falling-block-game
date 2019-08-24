using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConfig
{
	public static bool IsDevMode = true;
	public static bool LogLivingEntity = false;
	public static bool IsEasyMode = false;

	public static float volume = 1f;
	public static event System.Action OnVolumeChange;

	public static void SetVolume(float val)
	{
		volume = val;
		if (OnVolumeChange != null)
		{
			OnVolumeChange();
		}
	}
}