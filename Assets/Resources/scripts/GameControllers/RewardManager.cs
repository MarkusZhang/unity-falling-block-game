using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardConfig
{
	public int scoreThreshold;
	public float timeThreshold;
	public GameObject spawnPrefab;
}

// RewardManager is the object for instantiating rewards
// just like a GameFlowCtrl
public class RewardManager : MonoBehaviour {
	
	public RewardConfig[] rewardConfigs; // config of when to spawn reward blocks
	public bool easyModeOnly;
	private int startScore;
	private float startTime;
	private int nextSpawnIdx;
	private bool enabled = true;
	
	// Use this for initialization
	void Start () {
		startScore = ScoreCtrl.GetScore();
		nextSpawnIdx = 0;
		startTime = Time.time;
		if (!(easyModeOnly && !GlobalConfig.IsEasyMode))
		{
			StartCoroutine(genRewards());
		}
	}

	public void Enable()
	{
		enabled = true;
	}

	public void Disable()
	{
		enabled = false;
	}
	
	IEnumerator genRewards () {
		// spawn conditional rewards
		while (true)
		{
			if (nextSpawnIdx < rewardConfigs.Length && ScoreCtrl.GetScore() >= rewardConfigs[nextSpawnIdx].scoreThreshold + startScore &&
			    Time.time - startTime > rewardConfigs[nextSpawnIdx].timeThreshold)
			{
				if (RewardSpawner.instance != null)
				{
					RewardSpawner.instance.SpawnReward(rewardConfigs[nextSpawnIdx].spawnPrefab);
				}
				nextSpawnIdx++;
			}
			yield return new WaitForSeconds(1);
		}
		
	}
}
