using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GunStore {

	public static event System.Action OnBulletLimitReached;
	public static event System.Action OnConsumeBullet;
	public static event System.Action OnSwitchGun;

	public static Dictionary<GunType,int> defaultGuns = new Dictionary<GunType, int>
	{
		{GunType.Default,-1},
		{GunType.Ring,300},
		{GunType.Burst,30},
		{GunType.Laser,100},
		{GunType.Spray,150},
		{GunType.Wide,150},
		{GunType.Track,100}
	};

	// Gun State vars
	public static GunType currentGunType = GunType.Default;
	private static int numSideGuns = 0;
	private static Dictionary<GunType, int> allGuns = defaultGuns;

	public static void AddSideGun()
	{
		if (numSideGuns < 2) // at most two side guns
		{
			numSideGuns++;
			if (numSideGuns == 1)
			{
				LifeCtrl.OnLifeLost += () => { numSideGuns = 0; }; // side guns should be destroyed on player death
			}
		}
	}

	public static int GetNumSideGuns()
	{
		return numSideGuns;
	}
	
	// switch current gun and return new guntype
	public static GunType SwitchGun(){
		GunType[] allTypes = new List<GunType> (allGuns.Keys).ToArray();

		for (int i = 0; i < allTypes.Length; i++) {
			if (allTypes[i] == currentGunType) {
				GunType newGunType = allTypes [(i + 1) % allTypes.Length];
				currentGunType = newGunType;

				if (OnSwitchGun != null) {
					OnSwitchGun ();
				}

				return currentGunType;
			}
		}

		throw new UnityException (currentGunType.ToString () + " doesn't exist in allGuns, unexpected!");
	}

	// switch to a specific type of gun
	public static GunType SwitchGun(GunType type){
		if (allGuns.ContainsKey (type)) {
			currentGunType = type;

			if (OnSwitchGun != null) {
				OnSwitchGun ();
			}
			return type;
		} else {
			throw new UnityException (type.ToString () + " doesn't exist in gun store");
		}
	}

	public static void ConsumeBullet(){
		bool isBulletInfinite = allGuns[currentGunType] < 0;

		if (!isBulletInfinite) {
			allGuns[currentGunType]--;

			if (allGuns[currentGunType] == 0) {

				GunType typeToRemove = currentGunType;

				GunStore.SwitchGun();
				allGuns.Remove (typeToRemove);

				// call event listener
				if (OnBulletLimitReached != null) {
					OnBulletLimitReached ();
				}
				
			} else {

				if (OnConsumeBullet != null) {
					OnConsumeBullet ();
				}

			}
		}
	}

	public static void AddGun(GunType type, int bullets){
		if (allGuns.ContainsKey (type)) {
			int currentBulletsLeft;
			allGuns.TryGetValue (type, out currentBulletsLeft);
			bool isBulletInfinite = currentBulletsLeft < 0;

			if (! isBulletInfinite) {
				allGuns [type] = currentBulletsLeft + bullets;
				if (OnConsumeBullet != null) {
					OnConsumeBullet ();
				}
			} // otherwise we already have infinite bullets
		} else {
			allGuns.Add (type, bullets);
		}
	}

	public static void Reset(){
		RemoveEventListeners();
		allGuns = new Dictionary<GunType, int>();
		foreach(KeyValuePair<GunType, int> entry in defaultGuns)
		{
			allGuns[entry.Key] = entry.Value;
		}
		currentGunType = GunType.Default;
		numSideGuns = 0;
	}

	public static void RemoveEventListeners()
	{
		OnBulletLimitReached = null;
		OnConsumeBullet = null;
		OnSwitchGun = null;
	}


	public static Dictionary<GunType, int> GetGunStoreStatus()
	{
		return allGuns;
	}
	
	// for debugging purpose
	public static string GetGunStoreStatusStr()
	{
		var desc = "";
		foreach (KeyValuePair<GunType, int> kvp in allGuns)
		{
			desc += string.Format("{0}:{1},", kvp.Key.ToString(), kvp.Value);
		}

		return desc;
	}

	public static void LoadGunStoreStatus(Dictionary<GunType, int> status)
	{
		allGuns = new Dictionary<GunType, int>();
		foreach(KeyValuePair<GunType, int> entry in status)
		{
			allGuns[entry.Key] = entry.Value;
		}
	}
}
