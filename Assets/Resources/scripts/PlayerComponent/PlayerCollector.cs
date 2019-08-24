using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class PlayerCollector: MonoBehaviour, ICollector{
	
	public void Collect(Collectable c){
		Player player = gameObject.GetComponent<Player> ();
		if (c.type == CollectableType.weapon)
		{
			var type = WeaponConstants.GetTypeFromName(c.cname);
			if (type == WeaponType.SideGun && player!=null)
			{
				player.EquipSideGun();
				GunStore.AddSideGun();
			}
			else
			{
				player.GetWeapon (type);
			}
			
		} else if (c.type == CollectableType.health) {
			int amount = 1;
			Int32.TryParse (c.param, out amount);
			player.AddHealth (amount);

		} else if (c.type == CollectableType.bullet) {
			GunType type = GunConstants.GetTypeFromName (c.cname);
			GunStore.AddGun (type, Int32.Parse (c.param));
			GunStore.SwitchGun (type);
		}else if (c.type == CollectableType.bulletPower)
		{
			var timeLimit = Int32.Parse(c.param);
			player.gunManager.BoostBulletPowerWithTimeLimit(1,timeLimit);
		} 
		else {
			throw new UnityException (c.type.ToString () + " is not a valid collectable type");
		}

		// play sound
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.collection);
		}
	}
}
