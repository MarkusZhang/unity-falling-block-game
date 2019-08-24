using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBar : LivingEntity
{
	public float shootOutSpeed;
	
	private bool isShotOut = false;
	
	public void ShootOut()
	{
		isShotOut = true;
		StartCoroutine(shootOut());
	}

	IEnumerator shootOut()
	{
		while (true)
		{
			transform.Translate(Vector3.up * shootOutSpeed * Time.deltaTime);
			yield return null;
		}
	}

	void OnBecameInvisible()
	{
		if (isShotOut)
		{
			Destroy(gameObject);
		}
	}
}
