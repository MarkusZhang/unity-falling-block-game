using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour {

	public Color cameraBlinkColor = new Color(0.77f, 0f, 0.28f);
	public static BackgroundCtrl instance;

	void Awake(){
		instance = this;
	}
	
	public void Blink(float blinkTime)
	{
		if (WrapAroundBackground.instance != null && WrapAroundBackground.instance.gameObject.active)
		{
			WrapAroundBackground.instance.Blink(blinkTime);
		}
		else if (getActiveSubStage()!=null)
		{
			// TODO: blink sub stage
		}
		else
		{
			StartCoroutine(flashCameraBackground(blinkTime, cameraBlinkColor));
		}
	}

	IEnumerator flashCameraBackground(float blinkTime, Color color)
	{
		var blinkInterval = 0.1f;
		var startTime = Time.time;
		while (Time.time - startTime < blinkTime)
		{
			Camera.main.backgroundColor = color;
			yield return new WaitForSeconds(blinkInterval);
		
			Camera.main.backgroundColor = Color.black;
			yield return new WaitForSeconds(blinkInterval);
		}
	}

	SubStage getActiveSubStage()
	{
		var subStages = GameObject.FindGameObjectsWithTag("substage");
		foreach (var subStage in subStages)
		{
			if (subStage.active)
			{
				return subStage.GetComponent<SubStage>();
			}
		}

		return null;
	}
}
