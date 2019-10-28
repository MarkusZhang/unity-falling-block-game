using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum LeiyibuState
{
	idle,
	shiwanVolt,
	rotateLightning,
	fastMove
}

public class Leiyibu : LivingEntity,IControlledAttacker
{
	// below are for rotating lightning attack
	public int numLightning; // we assume that this is always odd
	public float lightningAngle; // the angle between lightnings
	public GameObject lightningPrefab;
	public float[] rotateSpeed;
	public float[] rotateTime; // in seconds

	// below are for fast move
	public GameObject fastMoveEffect;
	public float fastMoveEffectFlashTime = 0.5f;
	public int numFastMoves = 3;
	
	// below are for shiwan volt
	public float shiwanVoltInterval = 0.4f; // in seconds
	public GameObject voltPrefab; // should be a laser object
	
	public LeiyibuHead head;

	public bool autoStart = false;
	
	// state vars
	private int cumulativeDamage = 0;
	private LeiyibuState lastState;
	
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		OnTakeDamage += () => { cumulativeDamage += 1; };
		if (autoStart)
		{
			StartAttack();
		}
	}

	void doNextAction()
	{
		if (cumulativeDamage >= 6)
		{
			cumulativeDamage = 0;
			lastState = LeiyibuState.fastMove;
			StartCoroutine(fastMove());
		} else if (lastState == LeiyibuState.fastMove)
		{
			if (isPlayerUnder())
			{
				if (Random.Range(0f, 1f) < 0.8f)
				{
					lastState = LeiyibuState.shiwanVolt;
					StartCoroutine(shiwanVolt());
				}
				else
				{
					lastState = LeiyibuState.rotateLightning;
					StartCoroutine(rotateLightningAttack());
				}
			}
			else
			{
				if (Random.Range(0f, 1f) < 0.7f)
				{
					lastState = LeiyibuState.rotateLightning;
					StartCoroutine(rotateLightningAttack());
				}
				else
				{
					lastState = LeiyibuState.shiwanVolt;
					StartCoroutine(shiwanVolt());
				}
			}
		} else if (lastState == LeiyibuState.shiwanVolt)
		{
			if (isPlayerUnder())
			{
				if (Random.Range(0f, 1f) < 0.3f)
				{
					lastState = LeiyibuState.shiwanVolt;
					StartCoroutine(shiwanVolt());
				}
				else
				{
					lastState = LeiyibuState.rotateLightning;
					StartCoroutine(rotateLightningAttack());
				}
			}
			else
			{
				if (Random.Range(0f, 1f) < 0.5f)
				{
					lastState = LeiyibuState.fastMove;
					StartCoroutine(fastMove());
				}
				else
				{
					lastState = LeiyibuState.rotateLightning;
					StartCoroutine(rotateLightningAttack());
				}

			}
		} else if (lastState == LeiyibuState.rotateLightning)
		{
			if (isPlayerUnder())
			{
				if (Random.Range(01, 1f) < 0.3f)
				{
					lastState = LeiyibuState.rotateLightning;
					StartCoroutine(rotateLightningAttack());
				}
				else
				{
					lastState = LeiyibuState.shiwanVolt;
					StartCoroutine(shiwanVolt());
				}
			}
			else
			{
				if (Random.Range(0f, 1f) < 0.5f)
				{
					lastState = LeiyibuState.fastMove;
					StartCoroutine(fastMove());
				}
				else
				{
					lastState = LeiyibuState.rotateLightning;
					StartCoroutine(rotateLightningAttack());
				}
			}
		}
		else
		{
			lastState = LeiyibuState.fastMove;
			StartCoroutine(fastMove());
		}
	}
	
	// flash in lightning attack first in front, then on two sides, then back in front
	IEnumerator shiwanVolt()
	{
		head.PreAttack();
		yield return new WaitForSeconds(0.5f);
		
		var y = Utils.GetTopY();
		// volt in front
		Instantiate(voltPrefab, new Vector3(transform.position.x, y, 0), Quaternion.identity);
		yield return new WaitForSeconds(shiwanVoltInterval);
		
		// volt on left, make it  0.6 * screenwidth to the left
		var w = Utils.GetRightX();
		var margin = 0.7f * w;
		var x = transform.position.x - margin;
		x = (x < -w) ? (2 * w + x) : x; // wrap around
		Instantiate(voltPrefab, new Vector3(x, y, 0), Quaternion.identity);
		yield return new WaitForSeconds(shiwanVoltInterval);
		
		// volt on right, make it 0.6 * screenwidth to the right
		x = transform.position.x + margin;
		x = (x > w) ? (-2 * w + x) : x;
		Instantiate(voltPrefab, new Vector3(x, y, 0), Quaternion.identity);
		yield return new WaitForSeconds(shiwanVoltInterval);
		
		// volt in front 
		Instantiate(voltPrefab, new Vector3(transform.position.x, y, 0), Quaternion.identity);
		yield return new WaitForSeconds(shiwanVoltInterval);
		
		head.PostAttack();
		doNextAction();
	}

	IEnumerator fastMove()
	{
		var numMoves = numFastMoves;
		
		hideSelf();
		var effect = Instantiate(fastMoveEffect,transform.position,Quaternion.identity);
		effect.transform.parent = transform;
		Utils.SetAlphaValue(effect,0);
	
		for (int i = 0; i < numMoves; i++)
		{
			var alpha = 0f;
			// fade in flash effect
			while (alpha < 1)
			{
				alpha += Time.deltaTime * 2 / fastMoveEffectFlashTime;
				Utils.SetAlphaValue(effect,alpha);
				yield return null;
			}
			
			// fade out flash effect
			while (alpha > 0)
			{
				alpha -= Time.deltaTime * 2 / fastMoveEffectFlashTime;
				Utils.SetAlphaValue(effect,alpha);
				yield return null;
			}
			
			// change position
			var flashPos = Utils.GetRandomPos(-0.9f, 0.9f, 0.3f, 0.7f);
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (i == numMoves - 1 && playerRef != null)
			{
				flashPos = new Vector3(playerRef.transform.position.x,Utils.GetRandomY(0.5f,0.7f),0);
			}

			transform.position = flashPos;
		}
		
		Destroy(effect);
		showSelf();
		
		doNextAction();
	}
	
	IEnumerator rotateLightningAttack()
	{
		head.PreAttack();
		yield return new WaitForSeconds(0.5f);
		
		var lightnings = spawnLightnings();
		var angleChangeDir = 1;
		for (int i = 0; i < rotateSpeed.Length; i++)
		{
			var deltaAngle = 0f;
			var startTime = Time.time;
			while (Time.time - startTime < rotateTime[i])
			{
				deltaAngle += angleChangeDir * rotateSpeed[i] * Time.deltaTime;
				rotateLightnings(lightnings,deltaAngle);
				yield return null;
			}

			angleChangeDir = -angleChangeDir;
		}

		destroyLightnings(lightnings);
		
		head.PostAttack();
		doNextAction();
	}

	GameObject[] spawnLightnings()
	{
		// assume numLightning is odd number
		var lightnings = new GameObject[numLightning];
		
		// instantiate the middle lightning first
		var pos = getPosFromAngle(0f);
		lightnings[0] = Instantiate(lightningPrefab, pos, Quaternion.identity);
		lightnings[0].transform.parent = transform;
		
		// instantiate the rest
		for (int i = 0; i < (numLightning-1)/2; i++)
		{
			// left lightning
			var angle = lightningAngle * (i + 1);
			lightnings[2 * i + 1] = Instantiate(lightningPrefab, getPosFromAngle(angle),
				Quaternion.Euler(angle * Vector3.forward));
			lightnings[2 * i + 1].transform.parent = transform;

			// right lightning
			angle = - lightningAngle * (i + 1);
			lightnings[2 * i + 2] = Instantiate(lightningPrefab,getPosFromAngle(angle),Quaternion.Euler(angle * Vector3.forward));
			lightnings[2 * i + 2].transform.parent = transform;
		}

		return lightnings;
	}

	void destroyLightnings(GameObject[] lightnings)
	{
		foreach (var lightning in lightnings)
		{
			Destroy(lightning);
		}
	}

	void rotateLightnings(GameObject[] lightnings,float deltaAngle)
	{
		foreach (var lightning in lightnings)
		{
			if (lightning.gameObject != null)
			{
				var newAngle = lightning.transform.rotation.eulerAngles + deltaAngle * Vector3.forward;
				lightning.transform.rotation = Quaternion.Euler(newAngle);
				lightning.transform.position = getPosFromAngle(newAngle.z);
			}
		}
	}

	Vector3 getPosFromAngle(float angle)
	{
		var halfHeight = lightningPrefab.GetComponent<Renderer>().bounds.size.y / 2;
		var x = transform.position.x + halfHeight * Mathf.Sin(Mathf.Deg2Rad * angle);
		var y = transform.position.y - halfHeight * Mathf.Cos(Mathf.Deg2Rad * angle);
		return new Vector3(x,y,0);
	}

	void hideSelf()
	{
		Utils.SetAlphaValue(gameObject,0);
		Utils.SetAlphaValue(head.gameObject,0);
		gameObject.GetComponent<Collider2D>().enabled = false;
		head.GetComponent<Collider2D>().enabled = false;
	}

	void showSelf()
	{
		Utils.SetAlphaValue(gameObject,1);
		Utils.SetAlphaValue(head.gameObject,1);
		gameObject.GetComponent<Collider2D>().enabled = true;
		head.GetComponent<Collider2D>().enabled = true;
	}
	
	bool isPlayerUnder()
	{
		var player = GameObject.FindGameObjectWithTag("player");
		if (player != null)
		{
			var diff = Mathf.Abs(transform.position.x - player.transform.position.x);
			if (diff <= 1)
			{
				return true;
			}
		}

		return false;
	}

	public void StartAttack()
	{
		doNextAction();
	}
	
}
