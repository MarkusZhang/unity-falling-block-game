 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorConfig
{
	public bool useCustomColor;
	public Color color;
}

public class FollowCurveWave : AbstractEnemyWaveWithReward
{

	public GameObject enemyPrefab;
	public BezierCurve curve;
	public int numToGen;
	public float genInterval;
	public float moveInterval;
	public float moveStepSize = 0.01f;
	public bool rotateWithPath = true;
	public ColorConfig colorConfig;
	
	private int numGenerated;

	
	void Start()
	{
		if (autoStart)
		{
			StartWave();
		}
	}
	
	public override void StartWave()
	{
		totalNumToGen = numToGen;
		StartCoroutine(waveRoutine());
	}


	IEnumerator waveRoutine()
	{
		while (numGenerated < numToGen)
		{
			var instanPos = curve.GetStartPoint();
			var enemyObj = Instantiate(enemyPrefab, instanPos, Quaternion.identity);
			processEnemyObj(enemyObj);
			
			numGenerated++;
			yield return new WaitForSeconds(genInterval);
		}
	}

	
	void processEnemyObj(GameObject enemyObj)
	{
		SetEnemyColor(enemyObj);
		
		// listen to death event
		var livingEntity = enemyObj.GetComponent<LivingEntity>();
		Debug.Assert(livingEntity!=null,"enemy must be living entity");
		livingEntity.OnDeath += onSingleEnemyDestoryed;
		livingEntity.OnDeath += onSingleEnemyKilled;
		
		// listen to destoryOffScreen event
		var offScr = enemyObj.GetComponent<DestroyWhenGoingOffScreen>();
		if (offScr != null)
		{
			offScr.OnDestoryOffScreen += onSingleEnemyDestoryed;
		}
		
		// make sure enemyObj is a curve follower
		if (enemyObj.GetComponent<CurveFollower>() == null)
		{
			var follower = enemyObj.AddComponent<CurveFollower>();
			follower.curve = curve;
			follower.moveInterval = moveInterval;
			follower.stepSize = moveStepSize;
			follower.rotateWithPath = rotateWithPath;
			follower.StartPath();
		}
	}


	void SetEnemyColor(GameObject enemyObj)
	{
		if (colorConfig != null && colorConfig.useCustomColor == true)
		{
			enemyObj.GetComponent<SpriteRenderer>().color = colorConfig.color;
		}
	}
}
