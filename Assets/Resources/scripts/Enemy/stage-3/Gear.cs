using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// boss Gaofu's widget
public class Gear : MonoBehaviour
{
	public float rotateSpeed;
	public GameObject bulletPrefab;
	public Transform[] muzzles;

	public Collider2D openGearCollider;
	public Collider2D closedGearCollider;

	public Sprite openGearImg;
	public Sprite closedGearImg;

	public event System.Action OnGearBroken;

	public int numHitsToBreakHole;

	private bool gearBroken = true;
	private int numHits;
	private float shootInterval;
	private bool isChangingColor = false;

	void Start()
	{
		if (!gearBroken && openGearCollider != null)
		{
			openGearCollider.enabled = false;
		}
	}
	
	public void StartRotateAndShoot(float shootInterval)
	{
		this.shootInterval = shootInterval;
		StartCoroutine("rotateAndShoot");
	}

	public void StopRotateAndShoot()
	{
		StopCoroutine("rotateAndShoot");
	}

	IEnumerator rotateAndShoot()
	{
		var nextShootTime = Time.time;
		while (true)
		{
			transform.eulerAngles += Vector3.forward * rotateSpeed * Time.deltaTime;
			if (Time.time > nextShootTime)
			{
				shoot();
				nextShootTime = Time.time + shootInterval;
			}

			yield return null;
		}
	}

	public void OpenGear()
	{
		transform.eulerAngles = Vector3.zero;
		if (!gearBroken)
		{
			GetComponent<SpriteRenderer>().sprite = openGearImg;
			openGearCollider.enabled = true;
			closedGearCollider.enabled = false;
		}
	}

	public void CloseGear()
	{
		if (!gearBroken)
		{
			GetComponent<SpriteRenderer>().sprite = closedGearImg;
			openGearCollider.enabled = false;
			closedGearCollider.enabled = true;
		}
	}

	void shoot()
	{
		foreach (var muzzle in muzzles)
		{
			Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
		}
	}

//	private void OnTriggerEnter2D(Collider2D other)
//	{
//		if (other.tag.Contains("player:bullet"))
//		{
//			numHits++;
//			if (numHits >= numHitsToBreakHole)
//			{
//				breakGear();
//			}
//			else if (numHits < numHitsToBreakHole)
//			{
//				if (!isChangingColor)
//				{
//					StartCoroutine(ChangeColor());
//				}
//			}
//		}else if (other.tag.Contains("player:burst") && !gearBroken) // when gear is not open, it can prevent burst attack
//		{
//			numHits++;
//			Destroy(other.gameObject);
//		}
//	}

	private void breakGear()
	{
		gearBroken = true;
		closedGearCollider.enabled = false;
		openGearCollider.enabled = true;
		GetComponent<SpriteRenderer>().sprite = openGearImg;
		if (OnGearBroken != null)
		{
			OnGearBroken();
		}
		
	}
	
	IEnumerator ChangeColor()
	{
		isChangingColor = true;	
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer>().color = Color.white;
		isChangingColor = false;
	}
}
