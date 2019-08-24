using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGun : Gun {

	public override void InstantiateBullet(Transform muzzle)
	{
		GameObject bulletContainer = Instantiate (bulletPrefab, muzzle.position, muzzle.rotation);
		var bullet = bulletContainer.transform.GetChild(0).gameObject;
		Debug.Assert(bullet!=null,"Can not find the ring bullet");
		bullet.transform.localScale = bulletScale * bullet.transform.localScale;
		var bulletDamager = bullet.GetComponent<Damager>();
		Debug.Assert(bulletDamager!=null);
		bulletDamager.damage *= bulletDamageMultiplier;
	}
}
