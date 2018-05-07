using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWeapon : MonoBehaviour {

	protected float startTime;

	// Use this for initialization
	protected virtual void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (Time.time - startTime > TotalAliveTime()) {
			Destroy (gameObject);
		}
	}

	protected virtual float TotalAliveTime(){
		return 0f;
	}
}
