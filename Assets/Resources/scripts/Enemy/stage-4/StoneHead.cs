using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHead : LivingEntity, IControlledAttacker
{
	public GameObject stonePrefab;
	public int numStones;
	public float moveSpeed;
	public float turnSpeed;
	public Vector2 growDir; // for now, each dimension can only have value -1, 0 or 1

	public GameObject leftFist;
	public GameObject rightFist;
	public Transform leftMuzzle;
	public Transform rightMuzzle;
	public GameObject fistBulletPrefab;
	public float fistRotateSpeed;
	public float fistAttackInterval = 1; // in seconds
	
	public bool followPlayer; // if false, then it will move randomly
	public bool autoStart;

	private FollowerStone[] followers;
	
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

		// attach listener
		OnDeath += onDeath;
		
		if (autoStart)
		{
			StartAttack();
		}
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
		StartCoroutine(moveAroundRoutine());
		StartCoroutine(fistAttack());
	}

	IEnumerator moveAroundRoutine()
	{
		var playerObj = GameObject.FindGameObjectWithTag("player");
		// default to random 
		var targetPos = Utils.GetRandomPos(-0.9f, 0.9f, -0.9f, 0.9f);
		var targetDir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
		while (true)
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
				t.RotateAround(transform.position,axis,fistRotateSpeed * Time.deltaTime);
				yield return null;
			}

			// fist back
			while (Time.time - startTime < 0.6f)
			{
				t.RotateAround(transform.position,axis,-fistRotateSpeed * Time.deltaTime);
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

	void onDeath()
	{
		if (followers != null && followers.Length > 0)
		{
			followers[0].CascadeDestory();
		}
	}
}
