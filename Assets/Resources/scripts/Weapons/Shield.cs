using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : TimedWeapon {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		//TODO: change transparency as time passes
	}

	protected override float TotalAliveTime(){
		return 10f;
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "falling-block") {
			Destroy (collider.gameObject);
		}
	}
}
