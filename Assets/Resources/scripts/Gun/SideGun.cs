using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SideGun : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform muzzle;
	public float shootInterval;
	public string[] enemyTags = new string[] {"enemy:tracking-missile","enemy:boss","enemy:alien-ship","enemy:guard","enemy:falling-block"};

	public bool autoStart;
	public int side; // -1 means left side, 1 means right side
	
	private Transform parent;
	private float radius;

	void Start()
	{
		if (autoStart)
		{
			SetParent(transform.parent);
			StartShoot();
		}
	}
	
	public void SetParent(Transform parent)
	{
		this.parent = parent;
		radius = (transform.position - parent.position).magnitude;
	}

	public void StartShoot()
	{
		StartCoroutine("trackEnemy");
		StartCoroutine("shootBullet");
	}
	
	
	// Update is called once per frame
	IEnumerator trackEnemy ()
	{
		while (true)
		{
			var enemyRef = getTargetEnemy();
			if (enemyRef != null)
			{
				var posDiff = enemyRef.transform.position - transform.position;
				var degree = Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg - 90;
				transform.eulerAngles = Vector3.forward * degree;
				transform.position = getNewPosition(enemyRef.transform.position);
			}
			else
			{
				transform.eulerAngles = Vector3.zero;
				transform.position = getDefaultPosition();
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator shootBullet()
	{
		while (true)
		{
			Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
			yield return new WaitForSeconds(shootInterval);
		}
	}
	
	GameObject getTargetEnemy()
	{
		var minDist = 100f;
		GameObject closestEnemy = null;
		foreach (string tag in enemyTags)
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
			foreach (var enemy in enemies)
			{
				var dist = Vector3.Distance(enemy.transform.position, transform.position);
				if (enemy != null && dist < minDist)
				{
					minDist = dist;
					closestEnemy = enemy;
				}
			}
		}
		return closestEnemy;
	}

	Vector3 getDefaultPosition()
	{
		var x = side > 0 ? parent.position.x + radius : parent.position.x - radius;
		return new Vector3(x,parent.position.y,0);
	}
	
	Vector3 getNewPosition(Vector3 enemyPos)
	{
		var x0 = parent.position.x;
		var y0 = parent.position.y;
		var x1 = enemyPos.x;
		var y1 = enemyPos.y;

		if (Mathf.Abs(x0-radius - x1) < 1E-4)
		{
			return new Vector3(x0 - radius,y0,0);
		}
		
//		if (Mathf.Abs(x0+radius - x1) < 1E-4)
//		{
//			return new Vector3(x0 + radius,y0,0);
//		}
		
		var a = Mathf.Pow(x0 - x1, 2) - radius * radius; // this should be > 0
		if (a < 1E-4)
		{
			return new Vector3(x0 - radius,y0,0);
		}
		var b = 2 * (x0 - x1) * (y1 - y0);
		var c = Mathf.Pow(y1 - y0, 2) - radius * radius;

		var delta = b * b - 4 * a * c;
		var sol_1 = (-b + Mathf.Sqrt(delta)) / (2 * a);
		var sol_2 = (-b - Mathf.Sqrt(delta)) / (2 * a);
		var k = side < 0 ? Mathf.Min(sol_1, sol_2) : Mathf.Max(sol_1,sol_2);

		return getPoint(x0, y0, x1, y1, k);
	}

	Vector3 getPoint(float x0, float y0, float x1, float y1, float k)
	{
		var u = y1 - k * x1 - y0;
		var a = k * k + 1;
		var b = 2 * k * u - 2 * x0;
		var c = u * u + x0 * x0 - radius * radius;
		var delta = b * b - 4 * a * c;
		// assume delta should be zero
		var x = -b / (2 * a);
		var y = k * x + (y1 - k * x1);
		return new Vector3(x,y,0);
	}

	// absorb enemy bullet
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Contains("enemy:bullet"))
		{
			Destroy(other.gameObject);
		}
	}
}
