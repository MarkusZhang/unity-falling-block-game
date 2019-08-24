using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttacker : MonoBehaviour, IAttacker
{

	public GameObject preShootEffect;
	public float preShootLightupSpeed;
	public float preShootTime;

	public GameObject laserPrefab;

	public float laserStayTime;
	public float laserSpeed;

	public Transform[] muzzles;

	public void Attack()
	{
		StartCoroutine(AttackRoutine());
	}

	IEnumerator AttackRoutine()
	{
		// preshoot animation
		var startTime = Time.time;
		var effectRenderers = new SpriteRenderer[muzzles.Length];
		for (int i = 0; i < muzzles.Length; i++)
		{
			var effectObj = Instantiate(preShootEffect, muzzles[i].position, Quaternion.identity);
			effectObj.transform.parent = this.transform;
			effectRenderers[i] = effectObj.GetComponent<SpriteRenderer>();
			var c = effectRenderers[i].color;
			effectRenderers[i].color = new Color(c.r, c.g, c.b, 0);
		}

		while (Time.time - startTime < preShootTime)
		{
			for (int i = 0; i < effectRenderers.Length; i++)
			{
				// increase opacity
				var c = effectRenderers[i].color;
				var newAlpha = Mathf.Clamp(c.a + preShootLightupSpeed * Time.deltaTime,0,1);
				effectRenderers[i].color = new Color(c.r, c.g, c.b, newAlpha);
			}
			yield return new WaitForSeconds(0.05f);
		}
		
		// shoot laser
		for (int i = 0; i < muzzles.Length; i++)
		{
			var laserObj = Instantiate(laserPrefab, muzzles[i].position, muzzles[i].rotation);
			Laser laser = laserObj.GetComponent<Laser>();
			Debug.Assert(laser!=null);
			laser.laserStayTime = laserStayTime;
			laser.expandSpeed = laserSpeed;
		}
		
		yield return new WaitForSeconds(laserStayTime + 0.7f);
		
		// destroy preshoot effects
		foreach (var effectRenderer in effectRenderers)
		{
			Destroy(effectRenderer.gameObject);
		}
	}
}
