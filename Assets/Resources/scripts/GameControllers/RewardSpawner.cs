using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{

	public static RewardSpawner instance;

	private void Awake()
	{
		instance = this;
	}

	public void SpawnReward(GameObject rewardPrefab)
	{
		if (rewardPrefab != null)
		{
			var pos = Utils.GetRandomPos(-0.9f, 0.9f, 1.1f, 1.2f);
			Instantiate(rewardPrefab, pos, Quaternion.identity);
		}
	}

	public void SpawnRewards(GameObject[] rewardPrefabs)
	{
		var interval = 2f / (rewardPrefabs.Length + 0.0001f);
		for (int i = 0; i < rewardPrefabs.Length; i++)
		{
			var x = Utils.GetRandomX(-1 + interval * i, -1 + interval * (i + 1));
			var y = Utils.GetRandomY(0.8f, 1f);
			Instantiate(rewardPrefabs[i], new Vector3(x, y, 0), Quaternion.identity);
		}
	}
}
