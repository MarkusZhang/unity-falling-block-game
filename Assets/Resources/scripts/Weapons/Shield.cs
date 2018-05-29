using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	public int maxDamage; // max damage it can take

	int damageTaken;

	// Use this for initialization
	void Start () {
		damageTaken = 0;
	}
	
	// Update is called once per frame
	void UpdateSelf () {
		if (damageTaken > maxDamage) {
			Destroy (gameObject);
		} else {
			// increase transparency as being damaged
			float transparency = damageTaken * 1.0f / maxDamage;
			Color c = gameObject.GetComponent<SpriteRenderer> ().color;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, 1 - transparency);
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag.Contains("enemy") &&  !collider.gameObject.tag.Contains("boss")) {
			int damage = GetDamageOfObj (collider.gameObject);
			damageTaken += damage;
			UpdateSelf ();
			Destroy (collider.gameObject);
		}
	}

	int GetDamageOfObj(GameObject obj){
		Damager damager = obj.GetComponent<Damager> ();
		if (damager == null)
			return 1;
		else
			return damager.damage;
	}
}
