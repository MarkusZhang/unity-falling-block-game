using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAppearWave : AbstractEnemyWaveWithReward
{
	public Vector2 xRange;
	public Vector2 yRange;
	public GameObject enemyPrefab;
	public bool fadeIn;
	public float fadeInTime;
	public int numToGen;
	public float genInterval;
	
	// Use this for initialization
	void Start () {
		if (autoStart)
		{
			StartWave();
		}
	}
	

	public override void StartWave()
	{
		totalNumToGen = numToGen;
		StartCoroutine(generateEnemy());
	}

	IEnumerator generateEnemy()
	{
		var numGenerated = 0;
		while (numGenerated < numToGen)
		{
			var targetPos = new Vector3(
				Utils.GetRandomX(xRange.x,xRange.y),
				Utils.GetRandomY(yRange.x,yRange.y),
				0
				);
			var enemyObj = Instantiate(enemyPrefab, targetPos, Quaternion.identity);
			attachEventListener(enemyObj);
			if (fadeIn)
			{
				StartCoroutine(fadeInEnemy(enemyObj));
			}

			numGenerated++;
			yield return new WaitForSeconds(genInterval);
		}
	}

	IEnumerator fadeInEnemy(GameObject enemyObj)
	{
		Utils.SetAlphaValue(enemyObj,0);
		var alpha = 0f;
		while (alpha < 1f)
		{
			alpha += Time.deltaTime / fadeInTime;
			Utils.SetAlphaValue(enemyObj,alpha);
			yield return null;
		}
	}


	void attachEventListener(GameObject enemyObj)
	{
		// listen to death event
		var livingEntity = enemyObj.GetComponent<LivingEntity>();
		if (livingEntity != null)
		{
			livingEntity.OnDeath += onSingleEnemyDestoryed;
			livingEntity.OnDeath += onSingleEnemyKilled;
			// listen to destoryOffScreen event
			var offScr = enemyObj.GetComponent<DestroyWhenGoingOffScreen>();
			if (offScr != null)
			{
				offScr.OnDestoryOffScreen += onSingleEnemyDestoryed;
			}
		}
		else
		{ // if not living entity, we assume it's destroyed right after instantiation
			onSingleEnemyDestoryed();
		}
	}
}
