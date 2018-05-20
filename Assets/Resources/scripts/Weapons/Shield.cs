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
		float transparency = (Time.time - startTime) / TotalAliveTime ();
		Color c = gameObject.GetComponent<SpriteRenderer> ().color;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, 1 - transparency);
	}

	protected override float TotalAliveTime(){
		return 10f;
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag.Contains("enemy") &&  !collider.gameObject.tag.Contains("boss")) {
			Destroy (collider.gameObject);
		}
	}
}
