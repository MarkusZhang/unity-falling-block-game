using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is for small boss in the middle of the stage
public class BossWave : AbstractEnemyWave
{
	public bool hideBackground;
	public bool fadeInBoss;
	public float bossFadeInTime;
	public GameObject bossPrefab;
	public bool showBackgroundAft;
	public bool changeBackgroundAft;
	public string newBackgroundImg;
	public GameObject[] rewards;
	
	void Start()
	{
		if (autoStart)
		{
			StartWave();
		}
	}
	
	public override void StartWave()
	{
		StartCoroutine(showBoss());
	}

	IEnumerator showBoss()
	{
		// hide background
		if (hideBackground && WrapAroundBackground.instance!=null)
		{
			var fadeTime = 1.5f;
			WrapAroundBackground.instance.FadeOut(fadeTime);
			yield return new WaitForSeconds(fadeTime);
		}
		
		yield return new WaitForSeconds(1f);

		// show boss
		var boss = Instantiate(bossPrefab);
		boss.GetComponent<Collider2D>().enabled = false;
		if (fadeInBoss)
		{
			var alpha = 0f;
			Utils.SetAlphaValue(boss,0);
		
			while (alpha < 1.0f)
			{
				alpha += Time.deltaTime / bossFadeInTime;
				Utils.SetAlphaValue(boss,alpha);
				yield return null;
			}
		}
		
		boss.GetComponent<Collider2D>().enabled = true;
		
		var bossAttacker = boss.GetComponent<IControlledAttacker>();
		if (bossAttacker != null)
		{
			bossAttacker.StartAttack();
		}
		
		// attach death listener
		var bossLivingEntity = boss.GetComponent<LivingEntity>();
		if (bossLivingEntity != null)
		{
			bossLivingEntity.OnDeath += onBossDeath;
		}
	}

	void onBossDeath()
	{
		StartCoroutine(postWaveRoutine());
	}

	IEnumerator postWaveRoutine()
	{
		// generate reward
		if (rewards!=null && rewards.Length > 0)
		{
			var interval = 2f / rewards.Length;
			for (int i = 0; i < rewards.Length; i++)
			{
				var x = Utils.GetRandomX(-1 + interval * i, -1 + interval * (i + 1));
				var y = Utils.GetRandomY(0.5f, 0.9f);
				var pos = new Vector3(x,y,0);
				Instantiate(rewards[i], pos, Quaternion.identity);
			}
		}
		
		yield return new WaitForSeconds(2f);
		
		notifyAllEnemiesDestroyed();
		
		// change background
		if (changeBackgroundAft && WrapAroundBackground.instance!=null && newBackgroundImg!="")
		{
			WrapAroundBackground.instance.ChangeImage(newBackgroundImg);
		}
		
		// show background
		var fadeInTime = 1.5f;
		if (showBackgroundAft && WrapAroundBackground.instance != null)
		{
			WrapAroundBackground.instance.FadeIn(fadeInTime);
		}
		yield return new WaitForSeconds(fadeInTime + 2);
		
	}
}
