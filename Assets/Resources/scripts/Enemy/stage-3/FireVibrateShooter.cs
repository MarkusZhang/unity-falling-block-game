using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVibrateShooter : MonoBehaviour
{

	public float maxAngle;
	public float moveDist;
	public float shootInterval;
	public GameObject firePrefab;
	public Transform muzzle;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(randomFire(10));
	}
	
	// Update is called once per frame
	void doNextAction () {
		
	}
	
	IEnumerator randomFire(int numFires)
	{
		var fired = 0;
		while (fired < numFires)
		{
			// move animation
			var degree = Random.Range(-1f, 1f) * maxAngle;
			transform.eulerAngles = Vector3.forward * degree;
			transform.Translate(Vector3.up * moveDist);
			yield return new WaitForSeconds(0.1f);
			transform.Translate(Vector3.down * moveDist);
			
			// fire
			Instantiate(firePrefab, muzzle.position, muzzle.rotation);
			fired++;
			yield return new WaitForSeconds(shootInterval);
		}
		
		doNextAction();
	}
}
