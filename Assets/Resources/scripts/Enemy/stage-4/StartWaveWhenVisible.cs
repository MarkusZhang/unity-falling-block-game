using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbstractEnemyWave),typeof(SpriteRenderer))]
public class StartWaveWhenVisible : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position,0.3f*Vector3.one);
	}
	
	private void OnBecameVisible()
	{
		GetComponent<AbstractEnemyWave>().StartWave();
	}
}
