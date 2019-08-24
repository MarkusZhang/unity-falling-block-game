using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyWaveTrigger instantiates an enemy wave when the trigger comes into the screen
public class EnemyWaveTrigger : MonoBehaviour
{
	public GameObject enemyWave;
	public Vector3 relativePos;
	public bool useRelativePos; // instantiate the wave at some position relative to the trigger
	
	void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position,0.3f);
		if (useRelativePos)
		{
			Gizmos.DrawCube(transform.position+relativePos,0.3f * Vector3.one);
		}
	}

	private void OnBecameVisible()
	{
		Instantiate(enemyWave);
		Destroy(gameObject);
	}
}
