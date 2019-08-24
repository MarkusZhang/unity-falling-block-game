using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombinedEnemyWaveConfig
{
	public float waveTime;
	public bool waitTillFinish; //TODO: so far we don't support this
	public GameObject wavePrefab;
	public Vector2 spawnPos;

	public bool isWave; // if not, then we assume it's controllable enemy
}


public class CombinedEnemyWave : AbstractEnemyWave
{
	public CombinedEnemyWaveConfig[] configs;
	
	public bool startAuto = false; // turn to true to test
	
	private Object lockWaveFinished = new Object();
	private int numWaveFinished;
	private bool isStarted = false;
	
	void Start()
	{
		if (startAuto)
		{
			StartWave();
		}
	}
	
	private void OnDrawGizmos()
	{
		foreach (var config in configs)
		{
			Gizmos.DrawSphere(config.spawnPos,0.2f);
		}
	}
	
	// Use this for initialization
	IEnumerator spawnWaves()
	{
		var nextWaveTime = Time.time;
		int nextWaveIdx = 0;

		while (nextWaveIdx < configs.Length)
		{
			var config = configs[nextWaveIdx]; 
			if (Time.time > nextWaveTime)
			{
				// spawn one wave
				var waveObj = Instantiate(config.wavePrefab, new Vector3(config.spawnPos.x, config.spawnPos.y, 0),
					Quaternion.identity);

				if (config.isWave)
				{
					initEnemyWave(waveObj);
				}
				else
				{
					initSingleEnemy(waveObj);
				}
				

				// proceed to next wave
				nextWaveTime = Time.time + config.waveTime;
				nextWaveIdx++;
			}
			
			yield return new WaitForSeconds(0.1f);
		}
		
		//TODO: we may need to do some loop to keep the object alive
	}

	public override void StartWave()
	{
		if (!isStarted)
		{
			totalNumToGen = configs.Length;
			isStarted = true;
			numWaveFinished = 0;
			StartCoroutine(spawnWaves());
		}
	}

	void initEnemyWave(GameObject waveObj)
	{
		var wave = waveObj.GetComponent<AbstractEnemyWave>();
		Debug.Assert(wave!=null);
		wave.StartWave();
		wave.OnAllEnemiesDestroyed += onSingleEnemyDestoryed;
	}

	void initSingleEnemy(GameObject enemyObj)
	{
		// TODO: here we assume off screen event and death event will not be triggered at the same time
		var livingEntity = enemyObj.GetComponent<LivingEntity>();
		if (livingEntity != null)
		{
			livingEntity.OnDeath += onSingleEnemyDestoryed;
		}

		var offScreen = enemyObj.GetComponent<DestroyWhenGoingOffScreen>();
		if (offScreen != null)
		{
			offScreen.OnDestoryOffScreen += onSingleEnemyDestoryed;
		}

		var controllableAttacker = enemyObj.GetComponent<IControlledAttacker>();
		if (controllableAttacker != null)
		{
			controllableAttacker.StartAttack();
		}
	}
}
