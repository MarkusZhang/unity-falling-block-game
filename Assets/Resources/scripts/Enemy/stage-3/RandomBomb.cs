using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBomb : LivingEntity
{

	public float rotateSpeed;
	public float fallSpeed;
	public Transform[] muzzles;
	public GameObject bullet;
	public GameObject explosionEffect;
	
	private float explodeY;
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
		var distToBottom = transform.position.y + Camera.main.orthographicSize;
		explodeY = Random.Range(-Camera.main.orthographicSize, transform.position.y - distToBottom * 0.2f);
		StartCoroutine(fall());
		StartCoroutine(rotate());
	}

	IEnumerator fall()
	{
		while (transform.position.y > explodeY)
		{
			transform.position += Vector3.down * Time.deltaTime * fallSpeed;
			yield return null;
		}

		Instantiate(explosionEffect, transform.position, transform.rotation);
		foreach (var muzzle in muzzles)
		{
			Instantiate(bullet, muzzle.position, muzzle.rotation);
		}
		TakeDamage(health);
	}

	IEnumerator rotate()
	{
		while (true)
		{
			transform.eulerAngles += Vector3.forward * rotateSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
