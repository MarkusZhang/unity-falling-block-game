using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GaofuState
{
	start,
	rotate,
	openMouth,
	bumpAround
}

// GaofuV2 is only attackable after gear is broken
public class GaofuV2 : LivingEntity,IControlledAttacker
{
	public Gear gear;
	public Jumper jumper;
	public float gearShootInterval;
	public float moveSpeed;

	public Sprite openMouthImg;
	public Sprite closedMouthImg;

	public GameObject mouthBullet;
	public float mouthShootInterval;
	public Transform[] mouthMuzzles;
	public int numMovesInFollow; // how many moves to make to follow player

	public float bumpAroundTime;
	public float openGearTime;

	public bool autoStart;

	private GaofuState state = GaofuState.start;
	private int numRotates = 0;
	private bool isGearBroken = true; //TODO: this is temporarily set to true to make it easier
	private bool isMouthOpen = false;
	private int numContinousHit = 0;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		gear.OnGearBroken += () => { isGearBroken = true; };
		GetComponent<LivingEntity>().OnTakeDamage += () => { numContinousHit++; };
		if (autoStart)
		{
			StartAttack();
		}
	}

	IEnumerator followPlayer()
	{
		for (int i = 0; i < numMovesInFollow; i++)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (playerRef != null)
			{
				var targetPos = new Vector3(playerRef.transform.position.x,Utils.GetRandomY(0,0.7f),0);
				gear.StartRotateAndShoot(gearShootInterval);
				while (transform.position!=targetPos)
				{
					transform.position = Vector3.MoveTowards(transform.position,targetPos,Time.deltaTime * moveSpeed);
					yield return null;
				}
				gear.StopRotateAndShoot();
			}
			yield return new WaitForSeconds(0.2f);
		}
		
		doNextAction();
	}

	IEnumerator openMouthShoot()
	{
		gear.OpenGear();
		GetComponent<SpriteRenderer>().sprite = openMouthImg;
		isMouthOpen = true;
		var start = Time.time;
		while (Time.time - start < openGearTime)
		{
			var muzzleIdx = Random.Range(0, mouthMuzzles.Length);
			var muzzle = mouthMuzzles[muzzleIdx];
			Instantiate(mouthBullet, muzzle.position, muzzle.rotation);
			yield return new WaitForSeconds(mouthShootInterval);
		}

		gear.CloseGear();
		isMouthOpen = false;
		GetComponent<SpriteRenderer>().sprite = closedMouthImg;
		
		doNextAction();
	}

	IEnumerator bumpAround()
	{
		jumper.StartJumpAround();
		yield return new WaitForSeconds(bumpAroundTime);
		jumper.StopJumpAround();
		// restore rotation
		transform.eulerAngles = Vector3.zero;
		// restore position
		var targetPos = new Vector3(Utils.GetRandomX(-0.7f,0.7f),Utils.GetRandomY(0.2f,0.7f),0);
		while (transform.position != targetPos)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 10);
			yield return null;
		}
		
		doNextAction();
	}

	void doNextAction()
	{
		
		if (state == GaofuState.start)
		{
			StartCoroutine(followPlayer());
			state = GaofuState.rotate;
		}else if (numContinousHit > 15 && state != GaofuState.bumpAround) // if hit too much, we need to retaliate
		{
			numContinousHit = 0;
			StartCoroutine(bumpAround());
			state = GaofuState.bumpAround;
		}		
		else if (state == GaofuState.rotate)
		{
			processRotateState();
		}else if (state == GaofuState.openMouth)
		{
			// randomly choose rotate or bump
			if (Random.Range(0, 1f) > 0.8f)
			{
				StartCoroutine(followPlayer());
				state = GaofuState.rotate;
			}
			else
			{
				StartCoroutine(bumpAround());
				state = GaofuState.bumpAround;
			}
		}
		else // state == bumpAround
		{
			if (isPlayerUnder())
			{
				StartCoroutine(openMouthShoot());
				state = GaofuState.openMouth;
			}
			else
			{
				StartCoroutine(followPlayer());
				state = GaofuState.rotate;
			}
		}
	}


	void processRotateState()
	{
		if (numRotates >= 2)
		{
			numRotates = 0;
			// if player is roughly under, then open mouth
			if (isPlayerUnder())
			{
				StartCoroutine(openMouthShoot());
				state = GaofuState.openMouth;
			}
			else
			{
				// randomly choose between rotating and bump
				if (Random.Range(0f, 1f) > 0.9f)
				{
					StartCoroutine(followPlayer());
					numRotates++;
				}
				else
				{
					StartCoroutine(bumpAround());
					state = GaofuState.bumpAround;
				}
			}
		}
		else // continue to rotate
		{
			StartCoroutine(followPlayer());
			numRotates++;
		}
	}

	bool isPlayerUnder()
	{
		var playerRef = GameObject.FindGameObjectWithTag("player");
		if (playerRef != null && playerRef.transform.position.y < transform.position.y
		                      && Mathf.Abs(playerRef.transform.position.x - transform.position.x) < 3)
		{
			return true;
		}

		return false;
	}
	
	public override void TakeDamage(int damage){
		if (isMouthOpen || isGearBroken)
		{
			base.TakeDamage (damage);
		}
	}

	public void StartAttack()
	{
		doNextAction();
	}
}
