using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class BuZhang : MonoBehaviour,IControlledAttacker {

	// motion images
	public Sprite idleImage;
	public Sprite leftSoundImage;
	public Sprite rightSoundImage;
	public Sprite middleSoundImage;
	public Sprite jumpImage;
	
	// shadow animation
	public GameObject shadowPrefab;
	public float createShadowInterval;
	
	// weapon
	public GameObject firePrefab;
	public GameObject soundWavePrefab;
	public Transform[] fireMuzzles;
	public Transform leftSoundMuzzle;
	public Transform rightSoundMuzzle;
	public Transform middleSoundMuzzle;
	
	// motion control
	public float fastMoveSpeed;
	public Vector2 jumpSpeed;
	public int numJumpsPerTime;
	public int numSoundWavesPerTime;
	public float soundWaveLength;

	public bool autoStart;
	
	// data track
	private int cumSoundAttacks;
	private int cumFastMoves;
	private int cumJumps;
	private int cumHitsTaken = 0; // number of damages taken continuously
	
	
	// Use this for initialization
	void Start ()
	{
		GetComponent<LivingEntity>().OnTakeDamage += () => { cumHitsTaken++; };
		if (autoStart)
		{
			StartAttack();
		}
	}
	
	// Update is called once per frame
	void doNextAction ()
	{
		var playerRef = GameObject.FindGameObjectWithTag("player");
		float screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		if (cumHitsTaken > 15)
		{
			cumHitsTaken = 0;
			StartCoroutine(fastMove());
		}
		else if (playerRef != null && Mathf.Abs(playerRef.transform.position.x - transform.position.x) > screenHalfWidth
		    && cumFastMoves < 3)
		{
			cumFastMoves++;
			StartCoroutine(fastMove());
		}
		else if (cumSoundAttacks > 5)
		{
			cumSoundAttacks = 0;
			// choose fast move or jump
			if (Random.Range(0, 1f) > 0.5f)
			{
				cumFastMoves++;
				StartCoroutine(fastMove());
			}
			else
			{
				cumJumps++;
				cumFastMoves = 0;
				StartCoroutine(jump());
			}
		}else if (cumJumps > 2)
		{
			cumJumps = 0;
			cumFastMoves = 0;
			cumSoundAttacks++;
			StartCoroutine(soundAttack());
		}
		else
		{
			cumFastMoves = 0;
			// jump or sound attack
			// choose jump or sound attack
			if (Random.Range(0, 1f) > 0.5f)
			{
				cumSoundAttacks++;
				StartCoroutine(soundAttack());
			}
			else
			{
				cumJumps++;
				StartCoroutine(jump());
			}
		}
	}

	IEnumerator jump()
	{
		var jumped = 0;
		switchImage(jumpImage);
		while (jumped < numJumpsPerTime)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (playerRef != null)
			{
				var dir = playerRef.transform.position.x > transform.position.x ? 1 : -1;
				GetComponent<SpriteRenderer>().flipX = (dir > 0); // if facing right, we need to flip image
				
				// shoot fire
				shootFire();
				
				// jump down, set random Y as target
				var targetY = Utils.GetRandomY(-0.8f, -0.7f);
				while (transform.position.y > targetY)
				{
					transform.position += new Vector3(dir * jumpSpeed.x * Time.deltaTime, -jumpSpeed.y * Time.deltaTime,0);
					yield return null;
				}
				
				// fire
				shootFire();
				
				// jump up, set random Y as target
				targetY = Utils.GetRandomY(0.6f, 0.7f);
				while (transform.position.y < targetY)
				{
					transform.position += new Vector3(dir * jumpSpeed.x * Time.deltaTime, jumpSpeed.y * Time.deltaTime,0);
					yield return null;
				}
			}

			jumped++;
			yield return null;
		}
		
		switchImage(idleImage);
		GetComponent<SpriteRenderer>().flipX = false;
		doNextAction();
	}

	// randomly move to another position
	IEnumerator fastMove()
	{
		var dir = transform.position.x > 0 ? -1 : 1;
		var p1 = new Vector3(Utils.GetRandomX(0.7f,0.9f) * dir, Utils.GetRandomY(0,0.5f),0);
		var p2 = new Vector3(Utils.GetRandomX(0.1f,0.4f) * dir, Utils.GetRandomY(0.3f,0.7f),0);
		StartCoroutine("shadowFollow");
		// move to p1
		while (transform.position!=p1)
		{
			transform.position = Vector3.MoveTowards(transform.position, p1, Time.deltaTime * fastMoveSpeed);
			yield return null;
		}
		// move to p2
		while (transform.position!=p2)
		{
			transform.position = Vector3.MoveTowards(transform.position, p2, Time.deltaTime * fastMoveSpeed);
			yield return null;
		}
		StopCoroutine("shadowFollow");
		
		yield return new WaitForSeconds(0.5f);
		doNextAction();
	}

	IEnumerator soundAttack()
	{
		var attackCount = 0;
		while (attackCount < numSoundWavesPerTime)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (playerRef != null)
			{
				if (playerRef.transform.position.x < transform.position.x - 2f)
				{
					switchImage(leftSoundImage);
					Instantiate(soundWavePrefab, leftSoundMuzzle.position, leftSoundMuzzle.rotation);
					playSoundAttackAudio();
					yield return new WaitForSeconds(soundWaveLength);
				}else if (playerRef.transform.position.x > transform.position.x + 2f)
				{
					switchImage(rightSoundImage);
					Instantiate(soundWavePrefab, rightSoundMuzzle.position, rightSoundMuzzle.rotation);
					playSoundAttackAudio();
					yield return new WaitForSeconds(soundWaveLength);
				}
				else
				{
					// player is around in the front
					switchImage(middleSoundImage);
					for (int i = 0; i < 3; i++)
					{
						Instantiate(soundWavePrefab, middleSoundMuzzle.position, middleSoundMuzzle.rotation);
						playSoundAttackAudio();
						yield return new WaitForSeconds(0.2f);
					}
				}
			}
			
			attackCount++;
			yield return null;
		}
		
		switchImage(idleImage);
		yield return new WaitForSeconds(0.6f);
		doNextAction();
	}
	


	IEnumerator shadowFollow()
	{
		while (true)
		{
			Instantiate(shadowPrefab, transform.position, transform.rotation);
			yield return new WaitForSeconds(createShadowInterval);
		}
	}

	void shootFire()
	{
		foreach (var muzzle in fireMuzzles)
		{
			Instantiate(firePrefab, muzzle.position, muzzle.rotation);
		}
	}
	
	void switchImage(Sprite img)
	{
		GetComponent<SpriteRenderer>().sprite = img;
	}

	void playSoundAttackAudio()
	{
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.laba);
		}
	}
	
	public void StartAttack()
	{
		StartCoroutine(fastMove());
	}
}
