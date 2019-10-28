using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoneHead : LivingEntity, IControlledAttacker
{
	public GameObject stonePrefab;
	public int numStones;
	public float moveSpeed;
	public float leapSpeed = 10;
	public float turnSpeed;
	public Vector2 growDir; // for now, each dimension can only have value -1, 0 or 1

	public GameObject leftFist;
	public GameObject rightFist;
	public Transform leftMuzzle;
	public Transform rightMuzzle;
	public GameObject fistBulletPrefab;
	public float fistRotateSpeed;
	public float fistAttackInterval = 1; // in seconds

	// for spawn-stone attack
	public GameObject smallStonePrefab;
	public int spawnStoneNumber;
	public float[] spawnStoneDists;
	public float spawnStoneInterval;
	public Transform forwardPos;
	
	public bool followPlayer; // if false, then it will move randomly
	public float followPlayerPeriodInSecs = 2;
	public bool autoStart;

	private FollowerStone[] followers;
	
	// pre-attack effect
	public GameObject blinkEffect;
	public Transform[] eyes;

	private void OnDrawGizmos()
	{
		foreach (var dist in spawnStoneDists)
		{
			var pts = getSidePoints(dist, transform.position);
			Gizmos.DrawSphere(pts[0],0.2f);
			Gizmos.DrawSphere(pts[1],0.2f);
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		// generate the stone chain body
		followers = new FollowerStone[numStones];
		var delta = getPosDelta();
		var leader = gameObject;
		for (int i = 0; i < numStones; i++)
		{
			var pos = new Vector3(transform.position.x + (i+1)*delta.x,transform.position.y + (i+1)*delta.y,0);
			var stone = Instantiate(stonePrefab, pos, Quaternion.identity);
			stone.GetComponent<FollowerStone>().SetLeader(leader);
			stone.GetComponent<FollowerStone>().moveSpeed = moveSpeed;
			stone.GetComponent<FollowerStone>().distToLeader = stone.GetComponent<SpriteRenderer>().bounds.size.y;
			followers[i] = stone.GetComponent<FollowerStone>();
			leader = stone;
		}
		followers[0].SetSpeed(moveSpeed);

		// attach listener
		OnDeath += onDeath;
		
		if (autoStart)
		{
			StartAttack();
		}
	}

	void restoreFists()
	{
		leftFist.transform.localRotation = Quaternion.identity;
		rightFist.transform.localRotation = Quaternion.identity;
		leftFist.transform.localPosition = new Vector3(-1.15f,-0.4f,0);
		rightFist.transform.localPosition = new Vector3(1.15f,-0.4f,0);
	}
	
	void doNextAction()
	{
		StopCoroutine("fistAttack");
		restoreFists();
		
		var dice = Random.Range(0f, 1f);
		if (isPlayerInFront())
		{
			if (dice <= 0.4f)
			{
				// leap forward
				StartCoroutine(leapForward());

			}else if (dice < 0.8f)
			{
				// spawn stones
				StartCoroutine(spawnStonesRoutine());
			}
			else
			{
				// follow player
				StartCoroutine("fistAttack");
				StartCoroutine(moveAroundRoutine(followPlayerPeriodInSecs));
			}
		}
		else
		{
			// follow player
			StartCoroutine("fistAttack");
			StartCoroutine(moveAroundRoutine(followPlayerPeriodInSecs));
		}
		
	}

	bool isPlayerInFront()
	{
		var player = GameObject.FindGameObjectWithTag("player");
		if (player == null)
		{
			return false;
		}
		var toPlayer = player.transform.position - transform.position;
		return Vector3.Angle(toPlayer, -transform.up) < 30f;
	}

	Vector2 getPosDelta()
	{
		float width = stonePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
		float height = stonePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
		var deltaX = growDir.x * width;
		var deltaY = growDir.y * height;
		return new Vector2(deltaX,deltaY);
	}

	public void StartAttack()
	{
		foreach (var follower in followers)
		{
			follower.StartFollowing();
		}
		doNextAction();
	}

	void preAttack(float effectTime)
	{
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.bossCut);
		}
		foreach (var eye in eyes)
		{
			var effect = Instantiate(blinkEffect, eye.position, Quaternion.identity);
			effect.transform.parent = transform;
			effect.GetComponent<EyeBlinkEffect>().effectTime = effectTime;
		}
	}
	
	IEnumerator leapForward()
	{
		// face player
		var playerObj = GameObject.FindGameObjectWithTag("player");
		if (playerObj != null)
		{
			var targetPos = playerObj.transform.position;
			float xDiff = playerObj.transform.position.x - transform.position.x;
			float yDiff = playerObj.transform.position.y - transform.position.y;
			var targetDir = (new Vector2(xDiff,yDiff)).normalized;
			targetDir = clampTargetDir(targetDir);
			float targetAngle = 90+Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.forward * targetAngle;

			preAttack(0.5f);
			yield return new WaitForSeconds(0.5f);

			followers[0].SetSpeed(leapSpeed);
			while (transform.position != targetPos)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * leapSpeed);
				yield return null;
			}
			followers[0].SetSpeed(moveSpeed);
		}
		
		doNextAction();
	}

	IEnumerator moveAroundRoutine(float timeinSecs)
	{
		var playerObj = GameObject.FindGameObjectWithTag("player");
		// default to random 
		var targetPos = Utils.GetRandomPos(-0.9f, 0.9f, -0.9f, 0.9f);
		var targetDir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
		var startTime = Time.time;
		while (Time.time - startTime < timeinSecs)
		{
			// follow player if player exists and followPlayer=true
			if (followPlayer && playerObj != null)
			{
				targetPos = playerObj.transform.position;
				float xDiff = playerObj.transform.position.x - transform.position.x;
				float yDiff = playerObj.transform.position.y - transform.position.y;
				targetDir = (new Vector2(xDiff,yDiff)).normalized;
			}
			else
			{
				if (Vector3.Distance(transform.position,targetPos)<0.1f)
				{
					targetPos = Utils.GetRandomPos(-0.9f, 0.9f, -0.9f, 0.9f);
					targetDir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
				}
			}
			
			// move to target
			transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed * Time.deltaTime);
			
			// look to target
			targetDir = clampTargetDir(targetDir);
			float targetAngle = 90+Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
			if (Mathf.Abs (Mathf.DeltaAngle (targetAngle, transform.eulerAngles.z)) > .05f) {
				// turning angle
				float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.z, targetAngle, Time.deltaTime * turnSpeed);
				transform.eulerAngles = new Vector3(0,0,angle);
			}
			
			yield return null;
		}
		
		doNextAction();
	}

	IEnumerator fistAttack()
	{
		bool useLeft = true;
		while (true)
		{
			var startTime = Time.time;
			var t = useLeft ? leftFist.transform : rightFist.transform;
			var axis = useLeft ? Vector3.back : Vector3.forward;
			var muzzle = useLeft ? leftMuzzle : rightMuzzle;
			
			// fist out
			if (fistBulletPrefab != null)
			{
				Instantiate(fistBulletPrefab, muzzle.position, muzzle.rotation);
			}
			while (Time.time - startTime < 0.3f)
			{
				t.RotateAround(transform.position,axis,-fistRotateSpeed * Time.deltaTime);
				yield return null;
			}

			// fist back
			while (Time.time - startTime < 0.6f)
			{
				t.RotateAround(transform.position,axis,fistRotateSpeed * Time.deltaTime);
				yield return null;
			}

			yield return new WaitForSeconds(fistAttackInterval);
			useLeft = !useLeft;
		}
	}

	// clampTargetDir prevent target dir from turning more than 90 degrees from forward direction
	Vector3 clampTargetDir(Vector3 originalDir)
	{
		var forwardDir = transform.position - followers[0].GetComponent<Transform>().position;
		var cosVal = Vector3.Dot(forwardDir.normalized, originalDir.normalized);
		if (cosVal > 0) // this means the originalDir is not turning more than 90 deg from the forwardDir
		{
			return originalDir;
		}
		// we need to clamp at 90 deg turning angle
		// there are two 90 deg turning angles, we need to check which one
		var clampDirs = new Vector3[2];
		if (forwardDir.x == 0)
		{
			clampDirs[0] = new Vector3(1,0,0);
			clampDirs[1] = new Vector3(-1,0,0);
		}else if (forwardDir.y == 0)
		{
			clampDirs[0] = new Vector3(0,1,0);
			clampDirs[1] = new Vector3(0,-1,0);
		}
		else
		{
			var bSq = 1f / (1 + Mathf.Pow(forwardDir.y / forwardDir.x, 2));
			var b = Mathf.Sqrt(bSq);
			clampDirs[0] = new Vector3(-forwardDir.y/forwardDir.x * b,b,0);
			clampDirs[1] = new Vector3(forwardDir.y/forwardDir.x * b,-b,0);
		}

		foreach (var clampDir in clampDirs)
		{
			if (Vector3.Dot(originalDir.normalized, clampDir) > 0) // cos > 0
			{
				return clampDir;
			}
		}
		//TODO: this should not happen
		return clampDirs[0];
	}

	IEnumerator spawnStonesRoutine()
	{
		// pre attack effect
		preAttack(0.5f);
		yield return new WaitForSeconds(0.5f);
		leftFist.transform.localRotation = Quaternion.Euler(0,0,40);
		rightFist.transform.localRotation = Quaternion.Euler(0,0,-40);
		
		// spawn stones
		var points = new Vector3[1+2*spawnStoneDists.Length];
		for (int i = 0; i < spawnStoneDists.Length; i++)
		{
			var dist = spawnStoneDists[i];
			var sidePoints = getSidePoints(dist, transform.position);
			points[2 * i] = sidePoints[0];
			points[2 * i + 1] = sidePoints[1];
		}
		points[points.Length - 1] = transform.position;

		for (int i = 0; i < spawnStoneNumber; i++)
		{
			spawnStones(points);
			yield return new WaitForSeconds(spawnStoneInterval);
		}
		
		yield return new WaitForSeconds(0.5f);
		leftFist.transform.localRotation = Quaternion.Euler(0,0,0);
		rightFist.transform.localRotation = Quaternion.Euler(0,0,00);
		
		doNextAction();
	}

	Vector3[] getSidePoints(float dist, Vector3 refPt)
	{
		var leftPos = transform.TransformPoint(Vector3.left * dist);
		var rightPos = transform.TransformPoint(Vector3.right * dist);
		return new Vector3[]{leftPos,rightPos};
	}

	void spawnStones(Vector3[] points)
	{
		foreach (var point in points)
		{
			var dir = point - forwardPos.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
			Instantiate(smallStonePrefab, point, Quaternion.Euler(Vector3.forward * angle));
		}
	}

	void onDeath()
	{
		if (followers != null && followers.Length > 0)
		{
			followers[0].CascadeDestory();
		}
	}
}
