using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyWaveWithReward : AbstractEnemyWave {

	// we can award rewards after certain conditions met
	public GameObject awardPrefab;
	public int awardAfterNumEnemiesKilled;
    public float awardTimeLimit; // player has to kill the specified # of enemies within this time to get reward
	// set `awardTimeLimit` to <=0 to indicate no time limit
	
	private int numKilled;
	private object numKilledLock = new Object();
	
	protected void onSingleEnemyKilled()
	{
		lock (numKilledLock)
		{
			if (numKilled < awardAfterNumEnemiesKilled)
			{
				numKilled++;
				if (numKilled == awardAfterNumEnemiesKilled && isTimeConditionMet())
				{
					if (RewardSpawner.instance != null)
					{
						RewardSpawner.instance.SpawnReward(awardPrefab);
					}
				}
			}
		}
	}

	private bool isTimeConditionMet()
	{
		return awardTimeLimit <= 0 || getTimeSinceStart() < awardTimeLimit;
	}

}
