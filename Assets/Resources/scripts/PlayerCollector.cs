using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class PlayerCollector: MonoBehaviour, ICollector{
	
	public void Collect(Collectable c){
		Player player = gameObject.GetComponent<Player> ();
		if (c.type == CollectableType.weapon) {
			player.GetWeapon (WeaponConstants.GetTypeFromName (c.name));

		} else if (c.type == CollectableType.health) {
			int amount = 1;
			Int32.TryParse (c.param, out amount);
			player.AddHealth (amount);

		} else if (c.type == CollectableType.bullet) {
			GunType type = GunConstants.GetTypeFromName (c.name);
			GunStore.AddGun (type, Int32.Parse (c.param));
			player.gunManager.SwitchGun (type);
		} else {
			throw new UnityException (c.type.ToString () + " is not a valid collectable type");
		}

		// play sound
		AudioManager.instance.PlaySound(AudioStore.instance.collection);
	}
}
