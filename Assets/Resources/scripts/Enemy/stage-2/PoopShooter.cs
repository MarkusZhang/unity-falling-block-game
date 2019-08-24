using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopShooter : LivingEntity, IControlledAttacker
{
	public float changeForceSpeed;
	public float shootInterval;
	public Transform muzzle;
	public GameObject bulletPrefab;
	public float maxAbsForce;

	public bool attackOnStart = false;
	
	protected override void Start () {
		base.Start();
		if (attackOnStart)
		{
			StartAttack();
		}
	}
	
	public void StartAttack()
	{
		StartCoroutine(shootRoutine());
	}

	IEnumerator shootRoutine()
	{
		var dir = 1;
		float force = 0;
		while (true)
		{
			force = updateForce(force, dir);
			if (Mathf.Abs(force) >= maxAbsForce)
			{
				dir = -dir; // reverse direction
			}

			var bulletObj = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
			var bulletRgd = bulletObj.GetComponent<Rigidbody2D>();
			bulletRgd.AddForce(new Vector2(force,0));
			yield return new WaitForSeconds(shootInterval);
		}
	}

	float updateForce(float f,float dir)
	{
		return f + dir * changeForceSpeed;
	}


}
