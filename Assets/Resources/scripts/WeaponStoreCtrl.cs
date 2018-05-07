using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// static class for storing collected weapons
public static class WeaponStoreCtrl {

	public static Hashtable storedWeapons = new Hashtable();

	public static event System.Action OnWeaponStoreChange;

	public static void StoreWeapon(WeaponType type, int num){
		if (storedWeapons.ContainsKey (type)) {
			storedWeapons [type] = (int)storedWeapons [type] + num;
		} else {
			storedWeapons [type] = num;
		}

		if (OnWeaponStoreChange != null) {
			OnWeaponStoreChange ();
		}
	}

	public static bool TakeWeapon(WeaponType type){

		if (storedWeapons.ContainsKey (type) && (int)storedWeapons [type] > 0) {
			storedWeapons [type] = (int)storedWeapons [type] - 1;

			if (OnWeaponStoreChange != null) {
				OnWeaponStoreChange ();
			}

			return true;
		}
		return false;
	}

	public static int GetWeaponCount(WeaponType type){
		if (!storedWeapons.ContainsKey (type)) {
			return 0;
		} else {
			return (int)storedWeapons [type];
		}
	}
}
