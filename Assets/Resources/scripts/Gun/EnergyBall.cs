using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
	public int maxEnergyVal;
	public float incrEnergyTimeInterval = 0.6f;
	
	private int energyVal;
	private object lockEnergyVal = new Object();
	private Vector3 originalScale;

	void Start()
	{
		originalScale = transform.localScale;
		StartCoroutine(incrEnergyAlongTime());
	}
	
	// absorb bullet
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag.Contains("bullet"))
		{
			addEnergy(2);
			Destroy(collider.gameObject);
		}
	}

	IEnumerator incrEnergyAlongTime()
	{
		while (energyVal < maxEnergyVal)
		{
			yield return new WaitForSeconds(incrEnergyTimeInterval);
			addEnergy(1);
		}
	}

	void addEnergy(int val)
	{
		lock (lockEnergyVal)
		{
			if (energyVal < maxEnergyVal)
			{
				energyVal += val;
				updateBallSize();
			}
		}
	}

	void updateBallSize()
	{
		transform.localScale = originalScale + 0.3f * energyVal * Vector3.one;
	}

	public int getEnergyVal()
	{
		return energyVal;
	}
}
