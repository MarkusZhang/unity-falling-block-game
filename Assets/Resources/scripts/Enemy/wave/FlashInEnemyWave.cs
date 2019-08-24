using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashInEnemyWave : AbstractEnemyWaveWithReward
{
	public GameObject enemyPrefab;
	public Transform[] points;
	public GameObject flashPrefab; // just a static image object
	public float flashInTime; // total time taken to flash in the enemies
	
	// draw the path
	private void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position,0.2f*Vector3.one);
		if (points!=null && points.Length > 0)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Gizmos.DrawSphere(points[i].position,0.2f);
			}
		}
	}
	
	// Use this for initialization
	public override void StartWave()
	{
		StartCoroutine(flashInEnemies());
	}

	void Start () {
		if (autoStart)
		{
			StartWave();
		}
	}

	IEnumerator flashInEnemies()
	{
		// generate the effects and enemies, but hide them first
		var effects = new GameObject[points.Length];
		var enemies = new GameObject[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			var effect = Instantiate(flashPrefab, points[i].transform.position, points[i].transform.rotation);
			Utils.SetAlphaValue(effect,0);
			effects[i] = effect;

			var enemy = Instantiate(enemyPrefab, points[i].transform.position, points[i].transform.rotation);
			Utils.SetAlphaValue(enemy,0);
			enemy.GetComponent<Collider2D>().enabled = false;
			enemy.GetComponent<LivingEntity>().OnDeath += onSingleEnemyKilled;
			//TODO: should track those enemies destroyed when going off screen
			enemies[i] = enemy;
		}

		// show flash
		var inc = 2 * Time.deltaTime / flashInTime;
		for (float i = 0; i < 1; i+=inc)
		{
			Utils.SetAlphaValues(effects,i);
			yield return null;
		}
		
		// dim the flash and show enemy
		for (float i = 0; i < 1; i+=inc)
		{
			Utils.SetAlphaValues(effects,1-i);
			Utils.SetAlphaValues(enemies,i);
			yield return null;
		}
		
		// destroy the effects and activate the enemy
		foreach (var effect in effects)
		{
			Destroy(effect);
		}

		foreach (var enemy in enemies)
		{
			enemy.GetComponent<Collider2D>().enabled = true;
			if (enemy.GetComponent<IControlledAttacker>() != null)
			{
				enemy.GetComponent<IControlledAttacker>().StartAttack();
			}
		}
	}
}
