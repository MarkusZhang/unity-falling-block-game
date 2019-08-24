using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class LightningGun : Gun
{
	public GameObject lightningSourcePrefab;
	
	// Use this for initialization
	void Start () {
		
	}

	public override void InstantiateBullet(Transform muzzle)
	{
		// instantiate lightning source 
		var lightningSource = Instantiate(lightningSourcePrefab, transform.position, Quaternion.identity);
		var playerRef = GameObject.FindGameObjectWithTag("player");
		if (playerRef != null)
		{
			lightningSource.transform.parent = playerRef.transform;
		}

		// create lightning at the center
		var pos = new Vector3(transform.position.x,transform.position.y + bulletPrefab.GetComponent<SpriteRenderer>().bounds.size.y/2,0);
		Instantiate(bulletPrefab, pos, Quaternion.identity);
		var stayTime = bulletPrefab.GetComponent<LightningBullet>().stayTime;

		// destroy lightning source
		StartCoroutine(destroyLightningSource(lightningSource,stayTime));
	}

	IEnumerator destroyLightningSource(GameObject src, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(src);
	}
}
