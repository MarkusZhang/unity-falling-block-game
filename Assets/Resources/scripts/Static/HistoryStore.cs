using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusRecord
{
	public Dictionary<GunType, int> storedGuns;
	public int level;
	public int score;
}


public static class HistoryStore
{
	private static Dictionary<int, StatusRecord> records;
	
	// write the status after winning `stage`
	public static void WriteHistory(int stage,Dictionary<GunType, int> guns,int level,int score)
	{
		if (records == null)
		{
			records = new Dictionary<int, StatusRecord>();
		}
		records[stage] = new StatusRecord();
		records[stage].score = score;
		records[stage].level = level;
		records[stage].storedGuns = new Dictionary<GunType, int>();
		foreach(KeyValuePair<GunType, int> entry in guns)
		{
			records[stage].storedGuns[entry.Key] = entry.Value;
		}
	}

	// read the status after winning `stage`
	public static StatusRecord GetHistory(int stage)
	{
		if (records == null || !records.ContainsKey(stage))
		{
			throw new UnityException("GetHistory receives invalid stage: " + stage);
		}

		return records[stage];
	}

	public static bool HasHistory(int stage)
	{
		return records != null && records.ContainsKey(stage);
	}
}
