using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawn a wave of enemies that move along a certain path 
// with simple point-to-point movement
public class PathEnemyWave : AbstractEnemyWaveWithReward
{

	public int numEnemies;
	public float generateEnemyInterval;
	public float enemyMoveSpeed; // 0 value will be ignored
	public bool destroyEnemyAfterPath;
	public GameObject enemyPrefab;
	public Transform[] pathPoints;
	
	public bool useRandomColor;
	public bool useCustomColor;
	public Color customColor;
	
	public int enemyhealth; // value <=0 will get ignored

	private bool isStarted;
	
	// Use this for initialization
	void Start ()
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
			totalNumToGen = numEnemies;
			Debug.Assert(pathPoints.Length > 0, "enemy wave must have a path");
			StartCoroutine(GenerateEnemies());
		}
	}

	// draw the path
	private void OnDrawGizmos()
	{
		if (pathPoints.Length > 0)
		{
			for (int i = 0; i < pathPoints.Length - 1; i++)
			{
				Gizmos.DrawLine(pathPoints[i].position,pathPoints[i+1].position);
			}
		}
	}

	IEnumerator GenerateEnemies()
	{
		int numGenerated = 0;
		while (numGenerated < numEnemies)
		{
			var enemyObj = Instantiate(enemyPrefab, pathPoints[0].position, Quaternion.identity);
			var enemy = enemyObj.GetComponent<FollowPather>();
			Debug.Assert(enemy!=null,"the enemy should be instance of FollowPathEnemy");
			enemy.SetPath(pathPoints);
			
			enemy.moveSpeed = (enemyMoveSpeed > 0) ? enemyMoveSpeed : enemy.moveSpeed;
			enemy.destroyAfterPath = destroyEnemyAfterPath;

			setEnemyColor(enemyObj.GetComponent<SpriteRenderer>());
			setEnemyHealth(enemyObj);

			enemy.GetComponent<LivingEntity>().OnDeath += onSingleEnemyKilled;
			enemy.GetComponent<LivingEntity>().OnDeath += onSingleEnemyDestoryed;
			enemy.OnFollowPatherDestroyed += onSingleEnemyDestoryed;
			
			numGenerated++;
			yield return new WaitForSeconds(generateEnemyInterval);
		}
	}
	
	void setEnemyColor(SpriteRenderer enemyRender)
	{
		if (useRandomColor)
		{
			Color newColor = new Color( Random.value, Random.value, Random.value, 1.0f );
			enemyRender.color = newColor;
		}else if (useCustomColor)
		{
			enemyRender.color = customColor;
		}
	}

	void setEnemyHealth(GameObject enemyObj)
	{
		var livingEnemy = enemyObj.GetComponent<LivingEntity>();
		Debug.Assert(livingEnemy!=null);
		if (enemyhealth > 0)
		{
			livingEnemy.startingHealth = enemyhealth;
		}
	}
}
