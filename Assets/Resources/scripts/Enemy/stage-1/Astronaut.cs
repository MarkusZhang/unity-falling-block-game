using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : LivingEntity {

	// motion images
	public Sprite idleImage;
	public Sprite shootMissileImage;
	public Sprite massShootImage;
	public Sprite[] runImages;
	public Sprite[] cutGestureImages;
	
	public int numHitsBeforeReact;
	public float runSpeed;
	public float runAnimationInterval; // interval of switching images
	public float idleTime = 1;
	public float massShootInterval;
	public float massShootTime;
	public float massShootBulletRotRange; // rotation range
	public float shootMissileInterval;
	public int maxContinuousMissileShots = 4;

	public GameObject preCutEffectPrefab;
	public float preCutTime; // time for player to prepare for the back cut attack
	public float cutRotation = 45;
	
	// weapon prefabs
	public GameObject cutPrefab;
	public GameObject bulletPrefab;
	public GameObject missilePrefab;
	
	public GameObject[] enemyWaves; // enemy waves that the boss can call to come

	public Transform massShootMuzzle;
	public Transform missileMuzzle;
	
	private int numHits;
	
	// Use this for initialization
	protected override void Start(){
		base.Start();
		doNextAction();
	}

	void doNextAction()
	{
		if (numHits > numHitsBeforeReact)
		{
			numHits = 0;
			var randNum = Random.Range(0, 1f);
			if (randNum < 0.3f)
			{
				StartCoroutine(RunAndCut());
			}
			else if (randNum < 0.7f)
			{
				StartCoroutine(CallEnemyWave());
			}
			else if (randNum < 0.8f)
			{
				StartCoroutine(RunAndMassShoot());
			}
			else // randNum >= 0.8
			{
				StartCoroutine(RunAndShootMissile());
			}

			return;
		}
		
		var randFloat = Random.Range(0, 1f);

		if (randFloat > 0.5f)
		{
			StartCoroutine(RunAndMassShoot());
		}else if (randFloat > 0.05f)
		{
			StartCoroutine(RunAndShootMissile());
		}
		else // 10% chance idle
		{
			StartCoroutine(Idle());
		}
	}

	IEnumerator RunAndCut()
	{
		// locate the player
		var playerObj = GameObject.FindGameObjectWithTag("player");
		if (playerObj != null)
		{
			var playerX = playerObj.transform.position.x;
			if (Mathf.Abs(playerX - transform.position.x) < 2f) // if too close, move away
			{
				var halfScreenWidth = Camera.main.aspect * Camera.main.orthographicSize;
				var smallOffset = 1.5f;
				var targetX = playerX > 0 ? (-halfScreenWidth + smallOffset) : (halfScreenWidth - smallOffset);
				var targetPos = new Vector3(targetX,transform.position.y,transform.position.z);
				int runImgIdx = 0;
				// move to side of screen
				while (transform.position.x != targetX)
				{
					transform.position = Vector3.MoveTowards (transform.position, targetPos, runSpeed * Time.deltaTime);
					SwitchImage(runImages[runImgIdx]);
					runImgIdx = (runImgIdx + 1) % runImages.Length;
					yield return new WaitForSeconds(runAnimationInterval);
				}
			}
			
			// cut attack
			float screenHalfHeight = Camera.main.orthographicSize;
			float d = screenHalfHeight + playerObj.transform.position.y;
			Vector3 spawnPos = new Vector3(playerObj.transform.position.x + d, -screenHalfHeight,0);
			float audioLength = 0.5f;
			
			// pre-attack animation
			StartCoroutine(AnimatePreattackIndication(spawnPos));
			StartCoroutine(AnimateCutGesture());
			if (AudioManager.instance != null)
			{
				AudioManager.instance.PlaySound(AudioStore.instance.bossCut);
			}
			yield return new WaitForSeconds(audioLength);
			
			// animate attack
			float attackLen = 1f;
			Instantiate(cutPrefab, spawnPos, Quaternion.Euler(0, 0, cutRotation));
			yield return new WaitForSeconds(attackLen); // wait for attack animation to finish
		}

		doNextAction();
	}

	IEnumerator CallEnemyWave()
	{
		// pre-attack effect
		float audioLength = 0.8f;
		StartCoroutine(AnimateCutGesture());
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.bossCut);
		}
		yield return new WaitForSeconds(audioLength);
		
		// randomly select an enemy wave
		var waveIdx = Random.Range(0, enemyWaves.Length);
		Instantiate(enemyWaves[waveIdx]);
		
		yield return new WaitForSeconds(2.5f);
		
		doNextAction();
	}

	IEnumerator RunAndMassShoot()
	{
		// locate the player
		var playerObj = GameObject.FindGameObjectWithTag("player");
		if (playerObj != null)
		{
			var playerX = playerObj.transform.position.x;
			if (Mathf.Abs(playerX - transform.position.x) > 2f) // if too far from player, move closer
			{
				var targetPos = new Vector3(playerX,transform.position.y,transform.position.z);
				int runImgIdx = 0;
				// move to close to player
				while (transform.position.x != playerX)
				{
					transform.position = Vector3.MoveTowards (transform.position, targetPos, runSpeed * Time.deltaTime);
					SwitchImage(runImages[runImgIdx]);
					runImgIdx = (runImgIdx + 1) % runImages.Length;
					yield return new WaitForSeconds(runAnimationInterval);
				}
			}
			
			// mass shoot
			if (AudioManager.instance != null)
			{
				AudioManager.instance.PlaySound(AudioStore.instance.bossMassShoot);
			}
			SwitchImage(massShootImage);
			float startShootTime = Time.time;

			while (Time.time - startShootTime < massShootTime)
			{
				float rotation = Random.Range(-massShootBulletRotRange, massShootBulletRotRange);
				Instantiate(bulletPrefab, massShootMuzzle.position, Quaternion.Euler(0, 0, rotation));
				yield return new WaitForSeconds(massShootInterval);
			}
		
			SwitchImage(idleImage);
		}
		
		doNextAction();
	}

	IEnumerator RunAndShootMissile()
	{
		// locate the player
		var playerObj = GameObject.FindGameObjectWithTag("player");
		if (playerObj != null)
		{
			var playerX = playerObj.transform.position.x;
			if (Mathf.Abs(playerX - transform.position.x) < 1.5f) // if too close to player, move away
			{
				var targetPos = new Vector3(-playerX,transform.position.y,transform.position.z);
				int runImgIdx = 0;
				// move to away from player
				while (transform.position.x != targetPos.x)
				{
					transform.position = Vector3.MoveTowards (transform.position, targetPos, runSpeed * Time.deltaTime);
					SwitchImage(runImages[runImgIdx]);
					runImgIdx = (runImgIdx + 1) % runImages.Length;
					yield return new WaitForSeconds(runAnimationInterval);
				}
			}
			
			// shoot missiles
			SwitchImage(shootMissileImage);
		
			int numShots = Random.Range(1, maxContinuousMissileShots);
			for (int i = 0; i < numShots; i++)
			{
				Instantiate(missilePrefab, missileMuzzle.position, Quaternion.identity);
				if (AudioManager.instance != null)
				{
					AudioManager.instance.PlaySound(AudioStore.instance.bossShootMissile);
				}
				yield return new WaitForSeconds(shootMissileInterval);
			}
		
			SwitchImage(idleImage);
		}
		
		doNextAction();
	}

	IEnumerator Idle()
	{
		yield return new WaitForSeconds(idleTime);
		doNextAction();
	}
	
	// animation routine, independent of attack routines
	IEnumerator AnimatePreattackIndication(Vector3 attackPos)
	{
		var startTime = Time.time;
		var preCutEffect = Instantiate(preCutEffectPrefab, attackPos, Quaternion.identity);
		var c = preCutEffect.GetComponent<SpriteRenderer>().color;
		preCutEffect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0);
		
		while (Time.time - startTime < preCutTime)
		{
			var newAlpha = Mathf.Clamp(c.a + 8 * Time.deltaTime,0,1);
			preCutEffect.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, newAlpha);
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(preCutEffect);
	}
	
	// animation routine, independent of attack routines
	IEnumerator AnimateCutGesture()
	{
		int gestureImgIdx = 0;
		while (gestureImgIdx < cutGestureImages.Length)
		{
			SwitchImage(cutGestureImages[gestureImgIdx]);
			gestureImgIdx++;
			yield return new WaitForSeconds(runAnimationInterval);
		}
		// switch back
		SwitchImage(idleImage);
	}
	
	void SwitchImage(Sprite img)
	{
		GetComponent<SpriteRenderer>().sprite = img;
	}
	
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
		numHits++;
	}
}
