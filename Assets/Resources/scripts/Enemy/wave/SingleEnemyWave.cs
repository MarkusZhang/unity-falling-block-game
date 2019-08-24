using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorPoint
{
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight
}

// enemy wave that only spawns one enemy
public class SingleEnemyWave : AbstractEnemyWaveWithReward
{
	public GameObject enemyPrefab;
	public AnchorPoint anchor;
	public Vector2 offset;
	public bool spawnAtPlayerPos = false;

	public bool moveAftSpawn;
	// if moveAftSpawn is true, you need to set the following
	// make sure the enemy obj doesn't move itslef on start
	public Vector2 moveToOffset;
	public float moveSpeed;

	private bool isWaveStarted = false;

	private void Start()
	{
		if (autoStart)
		{
			StartWave();
		}
	}

	public override void StartWave()
	{
		totalNumToGen = 1;
		if (!isWaveStarted)
		{
			isWaveStarted = true;
			var enemyObj = Instantiate(enemyPrefab, getSpawnPos(), Quaternion.identity);
			attachListener(enemyObj);
			if (moveAftSpawn)
			{
				StartCoroutine(move(enemyObj));
			}
			else
			{
				startEnemyAttack(enemyObj);
			}
		}
		
	}

	IEnumerator move(GameObject enemyObj)
	{
		var anchorPos = getAnchorPos();
		var targetPos = anchorPos + new Vector3(moveToOffset.x, moveToOffset.y, 0);
		while (enemyObj.transform.position!=targetPos)
		{
			enemyObj.transform.position = Vector3.MoveTowards (enemyObj.transform.position, targetPos, moveSpeed * Time.deltaTime);
			yield return null;
		}
		startEnemyAttack(enemyObj);
	}

	Vector3 getSpawnPos()
	{
		if (spawnAtPlayerPos)
		{
			var player = GameObject.FindGameObjectWithTag("player");
			if (player != null)
			{
				return player.transform.position;
			}
		}
		var anchorPos = getAnchorPos();
		return anchorPos + new Vector3(offset.x, offset.y, 0);
	}

	Vector3 getAnchorPos()
	{
		float halfHeight = Camera.main.orthographicSize;
		float halfWidth = Camera.main.aspect * halfHeight;
		switch (anchor)
		{
				case AnchorPoint.BottomLeft: return new Vector3(-halfWidth,-halfHeight,0);
				case AnchorPoint.BottomRight: return new Vector3(halfWidth,-halfHeight,0);
				case AnchorPoint.TopLeft: return new Vector3(-halfWidth,halfHeight,0);
				case AnchorPoint.TopRight: return new Vector3(halfWidth,halfHeight,0);
				default: throw new UnityException("unsupported anchor");
				
		}
	}

	void attachListener(GameObject enemyObj)
	{
		// listen to death event
		var livingEntity = enemyObj.GetComponent<LivingEntity>();
		if (livingEntity != null)
		{
			livingEntity.OnDeath += onSingleEnemyDestoryed;
			livingEntity.OnDeath += onSingleEnemyKilled;
		}
		
		// listen to destoryOffScreen event
		var offScr = enemyObj.GetComponent<DestroyWhenGoingOffScreen>();
		if (offScr != null)
		{
			offScr.OnDestoryOffScreen += onSingleEnemyDestoryed;
		}
	}

	void startEnemyAttack(GameObject enemyObj)
	{
		var attacker = enemyObj.GetComponent<IControlledAttacker>();
		if (attacker != null)
		{
			attacker.StartAttack();
		}
	}

	private void OnDrawGizmos()
	{
		var pos = getSpawnPos();
		Gizmos.DrawSphere(pos,0.2f);

		if (moveAftSpawn)
		{
			var anchorPos = getAnchorPos();
			var targetPos = anchorPos + new Vector3(moveToOffset.x, moveToOffset.y, 0);
			Gizmos.DrawCube(targetPos,0.2f * Vector3.one);
		}
	}
}
