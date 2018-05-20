using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class Damager : MonoBehaviour {

	public int damage;
	public bool limitedNumDamages; // whether it is allowed to infinite number of damages
	public int numDamages; // number of times it can do damage before destroying itself
	public string[] targetTags;
	public string[] exceptTags;
	// only objects that contain all strings in targetTags, and not any from exceptTags will be damaged

	public GameObject destroyEffect;

	void Start(){
		if (limitedNumDamages) {
			Debug.Assert (numDamages > 0, gameObject.name + " numDamages must be positive");
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (isDamageable (collider.gameObject) && isDamageTarget (collider.gameObject)) {
			collider.gameObject.GetComponent<LivingEntity> ().TakeDamage (damage);
			numDamages--;
			if (limitedNumDamages && numDamages <= 0) {
				DestroySelf ();
			}
		}
	}

	public void DestroySelf(){
		if (destroyEffect != null) {
			Instantiate (destroyEffect, transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}

	bool isDamageable(GameObject o){
		return o.GetComponent<LivingEntity> () != null;
	}

	public bool isDamageTarget(GameObject o){
		if (o.tag == null) {
			if (targetTags.Length == 0)
				return true;
			else
				return false;
		}

		foreach (string targetTag in targetTags) {
			if (!o.tag.Contains (targetTag))
				return false;
		}
		foreach (string exceptTag in exceptTags) {
			if (o.tag.Contains (exceptTag))
				return false;
		}
		return true;
	}
}
