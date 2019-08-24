using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStage : MonoBehaviour
{
	public float scrollSpeed;
	public float scrollDist;
	public GameObject bossWave;

	public event System.Action OnSubStageEnd;
	public event System.Action OnSubStageBoss;

	public bool autoStart; // turn on for testing
	
	private float startY;
	
	// Use this for initialization
	void Start ()
	{
		if (autoStart)
		{
			StartSubStage();
		}
	}

	public void StartSubStage()
	{
		startY = transform.position.y;
		StartCoroutine(scroll());
	}

	IEnumerator scroll()
	{
		while (startY - transform.position.y < scrollDist)
		{
			transform.Translate(Vector3.down * Time.deltaTime * scrollSpeed);
			yield return null;
		}

		if (bossWave == null)
		{
			if (OnSubStageEnd != null)
			{
				OnSubStageEnd();
			}
			yield break;
		}
		
		// if bossWave is there
		var wave = Instantiate(bossWave);
		if (wave.GetComponent<AbstractEnemyWave>() != null)
		{
			// boss music
			if (OnSubStageBoss != null)
			{
				OnSubStageBoss();
			}
			wave.GetComponent<AbstractEnemyWave>().OnAllEnemiesDestroyed += () =>
			{
				if (OnSubStageEnd != null)
				{
					OnSubStageEnd();
				}
			};
			wave.GetComponent<AbstractEnemyWave>().StartWave();
		}
	}
}
