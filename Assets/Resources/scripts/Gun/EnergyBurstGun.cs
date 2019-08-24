using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBurstGun : Gun
{
	public GameObject energyBallPrefab;

	private GameObject eneryBall;
	
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		eneryBall = Instantiate(energyBallPrefab, transform);
		Debug.Assert(eneryBall.GetComponent<EnergyBall>()!=null,"energy ball prefab must have EnergyBall component");
	}

	public override void InstantiateBullet(Transform muzzle)
	{
		// create burst
		var burstObj = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
		var burst = burstObj.GetComponent<EnergyBurst>();
		Debug.Assert(burst!=null);
		var burstStayTime = getBurstStayTime();
		burst.stayTime = burstStayTime;
		burst.scale = getBurstScale();
		
		// destroy and recreate energy ball
		Destroy(eneryBall);
		eneryBall = Instantiate(energyBallPrefab, transform);
		
		// blink the background
		if (BackgroundCtrl.instance != null)
		{
			BackgroundCtrl.instance.Blink(burstStayTime);
		}
	}

	float getBurstStayTime()
	{
		if (eneryBall!=null && eneryBall.GetComponent<EnergyBall>() != null)
		{
			var energyVal = eneryBall.GetComponent<EnergyBall>().getEnergyVal();
			var maxEnergyVal = eneryBall.GetComponent<EnergyBall>().maxEnergyVal;
			return 1f + energyVal * 1.8f / maxEnergyVal;
		}

		return 1f;
	}

	float getBurstScale()
	{
		if (eneryBall != null && eneryBall.GetComponent<EnergyBall>() != null)
		{
			var energyVal = eneryBall.GetComponent<EnergyBall>().getEnergyVal();
			var maxEnergyVal = eneryBall.GetComponent<EnergyBall>().maxEnergyVal;
			return 0.8f + energyVal * 1.2f / maxEnergyVal;
		}

		return 0.8f;
	}
}
