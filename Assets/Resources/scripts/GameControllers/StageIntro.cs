using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageIntro : MonoBehaviour
{

	public Text stageTitle;
	public Text stageDesc;

	public float FadeInTime;
	public float FadeOutTime;
	public float StayTime;
	
	private int stageNum;
	
	// Use this for initialization
	void Start ()
	{
		stageNum = StageCtrl.GetStage();
		Debug.Assert(stageTitle!=null);
		stageTitle.text = "Stage " + stageNum;
		stageDesc.text = StageCtrl.GetStageIntro(stageNum);
		StartCoroutine(FadeInFadeOut(FadeInTime, FadeOutTime, StayTime, new Text[] {stageTitle, stageDesc}));
	}
	
	IEnumerator FadeInFadeOut(float fadeInTime, float fadeOutTime, float stayTime, Text[] texts)
	{
		Debug.Assert(texts.Length>0);
		var alpha = 0f;
		setAlphaValue(texts,0);
		
		while (alpha < 1.0f)
		{
			alpha += Time.deltaTime / fadeInTime;
			setAlphaValue(texts,alpha);
			yield return null;
		}
		
		yield return new WaitForSeconds(stayTime);
		
		while (alpha > 0f)
		{
			alpha -= Time.deltaTime / fadeInTime;
			setAlphaValue(texts,alpha);
			yield return null;
		}
		
		StartCoroutine(DelayAndSwitchScene(stageNum));
	}

	IEnumerator DelayAndSwitchScene(int stageNum)
	{
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(StageCtrl.GetPlaySceneName(stageNum));
	}

	void setAlphaValue(Text[] texts, float alpha)
	{
		foreach (Text t in texts)
		{
			t.color = new Color(t.color.r, t.color.g, t.color.b, alpha);
		}
	}
}
