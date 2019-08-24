using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// object that shows up for a short time and then disappear
public class FlashOver : MonoBehaviour {

	public float totalShowTime;
	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - startTime >= totalShowTime) {
			Destroy (gameObject);
		}
	}
}
