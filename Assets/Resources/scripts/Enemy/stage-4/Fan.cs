using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Fan : LivingEntity,IControlledAttacker
{
	public float maxRaidus;
	public float minRadius;
	public float maxSpinTime;
	
	public GameObject barPrefab;
	public float rotateSpeed;
	public float radiusChangeSpeed;
	public int numBars;

	public bool randomMove;
	public float randomMoveSpeed;
	public Vector2 randomMoveRange;

	public bool autoStart;
	
	private float[] angles;
	private float radius;
	private bool isRadiusIncr;
	private GameObject[] bars;
	private int barsKilled;
	private Vector3 startPos;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		startPos = transform.position;
		
		if (autoStart)
		{
			StartAttack();
		}
	}

	public void StartAttack()
	{
		// generate bars
		bars = new GameObject[numBars];
		radius = (maxRaidus + minRadius) / 2;
		float angleInterval = 360f / numBars;
		for (int i = 0; i < numBars; i++)
		{
			var angle = angleInterval * i;
			var bar = Instantiate(barPrefab, getPosFromAngle(angle), Quaternion.Euler(Vector3.fwd * angle));
			bars[i] = bar;
			bar.GetComponent<LivingEntity>().OnDeath += onBarDeath;
		}
		
		// attach death event listener
		OnDeath += onDeath;
		StartCoroutine(rotateBars());
		if (randomMove)
		{
			StartCoroutine(randomMoveRoutine());
		}
	}

	IEnumerator randomMoveRoutine()
	{
		while (true)
		{
			var x = Random.Range(startPos.x-randomMoveRange.x / 2,startPos.x+randomMoveRange.x / 2);
			var y = Random.Range(startPos.y - randomMoveRange.y / 2, startPos.y + randomMoveRange.y / 2);
			var targetPos = new Vector3(x,y,0);
			while (transform.position != targetPos)
			{
				transform.position = Vector3.MoveTowards(transform.position,targetPos,Time.deltaTime * randomMoveSpeed);
				yield return null;
			}
		}
	}
	
	IEnumerator rotateBars()
	{
		var startTime = Time.time;
		while (Time.time - startTime < maxSpinTime)
		{
			adjustRadius();
			// rotate the bars
			var deltaAngle = rotateSpeed * Time.deltaTime;
			foreach (var bar in bars)
			{
				if (bar == null)
				{
					continue;
				}
				var newAngle = bar.transform.rotation.eulerAngles.z + deltaAngle;
				var newPos = getPosFromAngle(newAngle);
				bar.transform.position = newPos;
				bar.transform.rotation = Quaternion.Euler(Vector3.forward * newAngle);
			}

			yield return null;
		}

		// shoot out all the bars
		foreach (var bar in bars)
		{
			if (bar != null)
			{
				bar.GetComponent<FanBar>().ShootOut();
			}
		}
		
		Destroy(gameObject);
	}

	void onDeath()
	{
		// destroy all the bars when the core dies
		foreach (var bar in bars)
		{
			Destroy(bar);
		}
	}

	void onBarDeath()
	{
		// destroy the core when all bars die
		Interlocked.Add(ref barsKilled, 1);
		if (barsKilled == numBars)
		{
			if (gameObject != null)
			{
				Destroy(gameObject);
			}
		}
	}

	void adjustRadius()
	{
		if (isRadiusIncr)
		{
			if (radius <= maxRaidus) // continue to increase
			{
				radius += radiusChangeSpeed * Time.deltaTime;
			}
			else // start to decrease
			{
				isRadiusIncr = false;
			}
		}
		else // radius is decreasing
		{
			if (radius >= minRadius) // continue to decrease
			{
				radius -= radiusChangeSpeed * Time.deltaTime;
			}
			else
			{
				isRadiusIncr = true;
			}
		}
	}
	
	Vector3 getPosFromAngle(float angle)
	{
		var deltaX = radius * Mathf.Sin(-angle * Mathf.Deg2Rad);
		var deltaY = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
		return new Vector3(transform.position.x + deltaX,transform.position.y+deltaY, transform.position.z);
	}
}
