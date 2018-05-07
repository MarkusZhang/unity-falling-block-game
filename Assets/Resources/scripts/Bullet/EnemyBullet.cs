using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

	public float speed = 15;
	public int damage = 1;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.down * speed * Time.deltaTime);

		if (transform.position.y < -Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "player") {
			other.gameObject.GetComponent<Player> ().TakeDamage (damage);
			Destroy (gameObject);
		} else if (other.tag == "weapon" && IsSolidShield(other.gameObject)) {
			Destroy (gameObject);
		}
	}

	bool IsSolidShield(GameObject obj){
		return obj.GetComponent<Shield> () != null;
	}
}
