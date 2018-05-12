using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GunStore {

	public static GunType currentGunType = GunType.Default;
	public static int numBulletsLeft = -1; // negative number indicates infinite bullet

	public static event System.Action OnBulletLimitReached;
	public static event System.Action OnConsumeBullet;
	public static event System.Action OnSwitchGun;

	static Dictionary<GunType,int> allGuns = new Dictionary<GunType, int> (); //TODO: add default gun as default value

	// switch current gun and return new guntype
	public static GunType SwitchGun(){
		GunType[] allTypes = new List<GunType> (allGuns.Keys).ToArray();

		for (int i = 0; i < allTypes.Length; i++) {
			if (allTypes[i] == currentGunType) {
				allGuns [currentGunType] = numBulletsLeft;

				GunType newGunType = allTypes [(i + 1) % allTypes.Length];
				currentGunType = newGunType;
				allGuns.TryGetValue (currentGunType, out numBulletsLeft);

				if (OnSwitchGun != null) {
					OnSwitchGun ();
				}

				return currentGunType;
			}
		}

		throw new UnityException (currentGunType.ToString () + " doesn't exist in allGuns, unexpected!");
	}

	public static void ConsumeBullet(){
		bool isBulletInfinite = numBulletsLeft < 0;

		if (!isBulletInfinite) {
			numBulletsLeft--;

			if (numBulletsLeft == 0) {

				Debug.Assert (allGuns.ContainsKey (GunType.Default), "Default gun type missing in allGuns!");
				allGuns.Remove (currentGunType);
				// switch back to default
				currentGunType = GunType.Default;
				allGuns.TryGetValue (currentGunType, out numBulletsLeft);

				// call event listener
				if (OnBulletLimitReached != null) {
					OnBulletLimitReached ();
				}
				if (OnSwitchGun != null) {
					OnSwitchGun ();
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
			} // otherwise we already have infinite bullets
		} else {
			allGuns.Add (type, bullets);
		}
	}

	public static void Reset(){
		OnBulletLimitReached = null;
		OnConsumeBullet = null;
		OnSwitchGun = null;
		allGuns = new Dictionary<GunType, int> ();
		currentGunType = GunType.Default;
		numBulletsLeft = -1;
	}

}
