using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballChain : MonoBehaviour {

	public int numFireballs; // how many fireballs to fire
	public float generateInterval;
	public GameObject fireballPrefab;

	// Use this for initialization
	void Start () {
		StartCoroutine (GenerateFireball ());
	}
	
	IEnumerator GenerateFireball(){
		float screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		float screenHalfHeight = Camera.main.orthographicSize;
		float genX = Random.Range (-0.6f, 0.6f) * screenHalfWidth;

		int numGenerated = 0;
		while (numGenerated < numFireballs) {
			Instantiate (fireballPrefab, new Vector3 (genX, -screenHalfHeight, 0), Quaternion.identity);
			numGenerated++;
			yield return new WaitForSeconds (generateInterval);
		}

		Destroy (gameObject);
	}
}
