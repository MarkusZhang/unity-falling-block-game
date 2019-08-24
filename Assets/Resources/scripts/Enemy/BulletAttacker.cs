using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttacker : MonoBehaviour, IAttacker
{
	public string attackerName;
	public GameObject bulletPrefab;
	public Transform[] muzzles;
	public float bulletSpeed; // non-positive value is ignored

	public bool allowBulletRotation;
	public float rotationMaxAngle;
	
	// pre-shoot effect
	public GameObject preShootEffect; // we will play preshoot animation if this is not null
	public float preShootTime = 0.5f;
	public float effectFadeOutTime = 0.3f;
	
	public void Attack()
	{
		StartCoroutine(AttackRoutine());
	}

	void shoot()
	{
		foreach (var muzzle in muzzles)
		{
			// set bullet rotation
			var rotation = muzzle.rotation;
			if (allowBulletRotation)
			{
				float angle = Random.Range(-1, 1) * rotationMaxAngle;
				rotation = Quaternion.Euler(Vector3.forward * angle);
			}
			
			GameObject obj = Instantiate(bulletPrefab, muzzle.position, rotation);
			
			// set speed
			FallBlock block = obj.GetComponent<FallBlock>();
			if (block != null && bulletSpeed > 0)
			{
				block.fallingSpeed = bulletSpeed;
			}
		}
	}
	
	IEnumerator AttackRoutine()
	{
		// preshoot animation
		var effects = new GameObject[muzzles.Length];
		if (preShootEffect != null)
		{
			var startTime = Time.time;
			for (int i = 0; i < muzzles.Length; i++)
			{
				var effectObj = Instantiate(preShootEffect, muzzles[i].position, Quaternion.identity);
				effectObj.transform.parent = this.transform;
				effects[i] = effectObj;
			}
			Utils.SetAlphaValues(effects,0);

			var alpha = 0f;
			var deltaAlpha = Time.deltaTime / preShootTime;
			while (Time.time - startTime < preShootTime)
			{
				alpha += deltaAlpha;
				Utils.SetAlphaValues(effects,alpha);
				yield return null;
			}
		}
		
		
		// shoot bullet
		shoot();
		
		// effect fade out
		if (preShootEffect != null)
		{
			var startFadeOutTime = Time.time;
			var deltaAlpha = Time.deltaTime / effectFadeOutTime;
			var alpha = 1f;
			while (Time.time - startFadeOutTime <= effectFadeOutTime)
			{
				alpha -= deltaAlpha;
				Utils.SetAlphaValues(effects,alpha);
				yield return null;
			}

			foreach (var effect in effects)
			{
				Destroy(effect);
			}
		}
		
	}

}
