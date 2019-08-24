using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSourceConfig
{
	public GameObject enemyPrefab;
	public Transform sourceTrans; // this includes position and rotation
	public int numToGenerate;
	public float generateInterval;

	public float fallSpeed; // this only applies when enemyPrefab has fall-block component
}

public class MultiSourceEnemyWave : AbstractEnemyWaveWithReward
{
	public WaveSourceConfig[] configs;
	public bool isStarted;
	
	void Start()
	{
		if (autoStart)
		{
			StartWave();
		}
	}
	
	public override void StartWave()
	{
		if (!isStarted)
		{
			isStarted = true;
			Debug.Assert(configs.Length>0);
			foreach (var config in configs)
			{
				totalNumToGen += config.numToGenerate;
				StartCoroutine(runOneWave(config));
			}
		}
		
	}

	IEnumerator runOneWave(WaveSourceConfig config)
	{
		int numGenerated = 0;
		while (numGenerated < config.numToGenerate)
		{
			var enemyObj = Instantiate(config.enemyPrefab, config.sourceTrans.position, config.sourceTrans.rotation);
			// set speed
			var fallBlk = enemyObj.GetComponent<FallBlock>();
			if (fallBlk != null)
			{
				fallBlk.fallingSpeed = config.fallSpeed;
			}
			
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

			numGenerated++;
			yield return new WaitForSeconds(config.generateInterval);
		}
	}
	
}
