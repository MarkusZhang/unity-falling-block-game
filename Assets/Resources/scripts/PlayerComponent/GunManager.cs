using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: change this to a singleton class
public class GunManager : MonoBehaviour {

	// about current gun
	public Gun currentGun;
	public GunType currentGunType;
	public GameObject currentGunObject;

	public int defaultBulletPowerLevel = 1;
	
	Transform parentTransform;
	
	float relativeShootSpeed = 1;
	private int bulletPowerLevel; // how big is the bullet, and how much damage power

	private float rollbackBulletPowerTime;
	private bool isInBoostedPower;

	void Start()
	{
		GunStore.OnSwitchGun += switchGun;
	}
	
	public void Initialize(Transform parent)
	{
		parentTransform = parent;
		isInBoostedPower = false;
		bulletPowerLevel = defaultBulletPowerLevel;
		SetCurrentGun(GunStore.currentGunType);
	}

	public void Shoot()
	{
		if (isInBoostedPower && Time.time > rollbackBulletPowerTime)
		{
			SetBulletPowerLevel(defaultBulletPowerLevel);
			isInBoostedPower = false;
		}

		if (currentGun.Shoot())
		{
			GunStore.ConsumeBullet();
		}
	}

	// internal switching procedure
	void switchGun()
	{
		var type = GunStore.currentGunType; 
		if (type != currentGunType) {
			Destroy (currentGunObject);
			SetCurrentGun (type);
			if (AudioManager.instance != null)
			{
				AudioManager.instance.PlaySound (AudioStore.instance.switchGun);
			}
		}
	}

	void OnShootLimitReached(){
		Destroy (currentGunObject);
		SetCurrentGun(GunStore.currentGunType);
	}

	void SetCurrentGun(GunType type){
		currentGunObject = TypeToGunObject(type);
		if (currentGunObject != null)
		{
			currentGun = currentGunObject.GetComponent<Gun> ();
			currentGun.ChangeShootSpeedByRatio (relativeShootSpeed);
			SetBulletPowerLevel(bulletPowerLevel);
			currentGunType = type;
			GunStore.OnBulletLimitReached += OnShootLimitReached;
		}
	}

	GameObject TypeToGunObject(GunType type){
		GameObject gunObject;
		var prefabName = GunConstants.typeToPrefabName[type];
		gunObject = Resources.Load (prefabName) as GameObject;

		if (parentTransform != null)
		{
			return Instantiate (gunObject, parentTransform);
		}
		
		return null;
	}

	public void SetRelativeShootSpeed(float value){
		relativeShootSpeed = value;
		if (currentGun != null)
		{
			currentGun.ChangeShootSpeedByRatio (relativeShootSpeed);
		}
	}

	public void SetBulletPowerLevel(int value)
	{
		Debug.Assert(value >= 1);
		currentGun.bulletDamageMultiplier = value;
		currentGun.bulletScale = 1 + 0.5f * (value-1);
		bulletPowerLevel = value;
	}

	public void BoostBulletPowerWithTimeLimit(int boost, float timeLimit)
	{
		// new power-up
		bulletPowerLevel += boost;
		SetBulletPowerLevel(bulletPowerLevel);
		isInBoostedPower = true;
		rollbackBulletPowerTime = Time.time + timeLimit;
	}

	private void OnDestroy()
	{
		GunStore.OnSwitchGun -= switchGun;
	}
}
