using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitConfirmDialog : MonoBehaviour
{

	public GameObject dialog;
	
	public void QuitGame()
	{
		Time.timeScale = 1;
		ScoreCtrl.Reset();
		WeaponStoreCtrl.Reset();
		GunStore.Reset();
		LifeCtrl.Reset();
		StageCtrl.Reset();
		SceneManager.LoadScene("start-scene");
	}

	public void CancelQuit()
	{
		dialog.SetActive(false);
	}

	public void ShowQuitDialog()
	{
		dialog.SetActive(true);
	}
}
