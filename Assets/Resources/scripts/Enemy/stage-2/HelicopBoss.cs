using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BossAction
{
	nothing,
	shootLaser,
	sparyShoot,
	shootMissile,
	planeCircle,
}

[RequireComponent(typeof(BulletAttacker))]
public class HelicopBoss : MonoBehaviour,IControlledAttacker
{
	public GameObject subplanePrefab;
	public int numPlanesInCircle;
	public float moveSpeed;
	public float circleRadius;
	public GameObject bossHead;
	
	public bool autoStart = false;

	private BossAction lastAction;
	
	private BulletAttacker laserAttacker;
	private BulletAttacker leftSprayAttacker;
	private BulletAttacker rightSprayAttacker;
	private BulletAttacker leftMissileAttacker;
	private BulletAttacker rightMissileAttacker;

	private bool isStarted = false;
	
	// Use this for initialization
	void Start ()
	{
		//TODO: we might have better way of doing this initialization
		var attackers = GetComponents<BulletAttacker>();
		Debug.Assert(attackers.Length==5,"attackers.Length==5");
		laserAttacker = attackers[0];
		leftSprayAttacker = attackers[1];
		rightSprayAttacker = attackers[2];
		leftMissileAttacker = attackers[3];
		rightMissileAttacker = attackers[4];
		
		if (autoStart)
		{
			StartAttack();
		}
	}

	void doNextAction()
	{
		if (isPlayerUnder())
		{
			lastAction = BossAction.shootLaser;
			StartCoroutine(shootLaserBullet());
		}
		else // decide next action based on last action
		{
			if (lastAction == BossAction.nothing)
			{
				randomTakeAction(new BossAction[]{BossAction.planeCircle,BossAction.shootMissile,BossAction.sparyShoot});
			} else if (lastAction == BossAction.planeCircle)
			{
				randomTakeAction(new BossAction[]{BossAction.shootMissile,BossAction.sparyShoot});
			} else if (lastAction == BossAction.shootLaser || lastAction == BossAction.shootMissile)
			{
				randomTakeAction(new BossAction[]{BossAction.planeCircle,BossAction.sparyShoot});
			} else if (lastAction == BossAction.sparyShoot)
			{
				randomTakeAction(new BossAction[]{BossAction.planeCircle,BossAction.shootMissile, BossAction.shootMissile});
			}
		}
	}

	void randomTakeAction(BossAction[] choices)
	{
		int index = Random.Range(0,choices.Length);
		var action = choices[index];
		lastAction = action;
		
		if (action == BossAction.shootLaser)
		{
			StartCoroutine(shootLaserBullet());
		}else if (action == BossAction.shootMissile)
		{
			StartCoroutine(shootMissile());
		}else if (action == BossAction.sparyShoot)
		{
			StartCoroutine(sprayShoot());
		}
		else // circle plane
		{
			StartCoroutine(spawnPlaneCircle());
		}
	}

	public void StartAttack()
	{
		if (!isStarted)
		{
			isStarted = true;
			StartCoroutine(randomMove());
			lastAction = BossAction.nothing;
			var headShake = bossHead.GetComponent<ShakeAnimation>();
			if (headShake != null)
			{
				headShake.StartAnimation();
			}
			doNextAction();
		}
	}

	IEnumerator shootLaserBullet()
	{
		var numShots = 3;

		for (int i = 0; i < numShots; i++)
		{
			laserAttacker.Attack();
			yield return new WaitForSeconds(0.3f);
		}
		
		yield return new WaitForSeconds(1f);
		
		doNextAction();
	}

	IEnumerator shootMissile()
	{
		var numShots = 5;
		for (int i = 0; i < numShots; i++)
		{
			var randVal = Random.Range(0, 1f);
			if (randVal > 0.5f)
			{
				leftMissileAttacker.Attack();
			}
			else
			{
				rightMissileAttacker.Attack();
			}
			yield return new WaitForSeconds(0.3f);
		}
		
		yield return new WaitForSeconds(1f);
		doNextAction();
	}

	IEnumerator sprayShoot()
	{
		var numShots = 2;
		for (int i = 0; i < numShots; i++)
		{
			if (isPlayerOnLeft())
			{
				leftSprayAttacker.Attack();
			}
			else
			{
				rightSprayAttacker.Attack();
			}
			yield return new WaitForSeconds(0.8f);
		}
		
		yield return new WaitForSeconds(1f);
		doNextAction();
	}

	IEnumerator spawnPlaneCircle()
	{
		var angleInterval = 360f / numPlanesInCircle;
		var planeTime = 5f;
		for (int i = 0; i < numPlanesInCircle; i++)
		{
			var planeObj = Instantiate(subplanePrefab, transform);
			// set plane property
			var plane = planeObj.GetComponent<CirclePlane>();
			if (plane != null)
			{
				plane.initialAngle = angleInterval * i;
				plane.stayTime = planeTime;
				plane.parent = transform;
				plane.radius = circleRadius;
				plane.StartAttack();
			}
		}
		
		yield return new WaitForSeconds(planeTime + 2);
		doNextAction();
	}


	IEnumerator randomMove()
	{
		while (true)
		{
			var x = Utils.GetRandomX(-0.7f, 0.7f);
			var y = Utils.GetRandomY(0.2f, 0.7f);
			var targetPos = new Vector3(x,y,0);
			while (transform.position != targetPos)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}
	
	bool isPlayerUnder()
	{
		var player = GameObject.FindGameObjectWithTag("player");
		if (player != null)
		{
			var diff = Mathf.Abs(transform.position.x - player.transform.position.x);
			if (diff <= 1.6)
			{
				return true;
			}
		}

		return false;
	}

	bool isPlayerOnLeft()
	{
		var player = GameObject.FindGameObjectWithTag("player");
		if (player != null)
		{
			return transform.position.x > player.transform.position.x;
		}

		return false;
	}
}
