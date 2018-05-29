using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ShipState{
	moving,
	shooting
};

public class AlienShip : LivingEntityWithAudio {

	public float stayTime;
	public float initialY;
	public float shootInterval;
	public float escapeSpeed; // speed of changing position after being hit
	public float shootAngleMax = 60;

	public GameObject bulletPrefab;
	public Transform muzzle;

	float startTime;
	ShipState state;
	Vector3 moveTo;
	Color originalColor;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		originalColor = GetComponent<SpriteRenderer> ().color;
		StartCoroutine (MoveDown ());
	}

	// move to `initialY`
	IEnumerator MoveDown(){
		Vector3 targetPosition = new Vector3 (transform.position.x, initialY, 0);
		float moveSpeed = 10f;
		while (true) {
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, moveSpeed * Time.deltaTime);
			if (transform.position == targetPosition) {
				state = ShipState.shooting;
				startTime = Time.time;
				StartCoroutine (AttackRoutine ());
				break;
			}
			yield return null;
		}
	}

	// keep attacking until being hit
	IEnumerator AttackRoutine(){
		while (Time.time - startTime < stayTime) {
			if (state == ShipState.shooting) {
				RandomShoot ();
				yield return new WaitForSeconds (shootInterval);
			} else if (state == ShipState.moving) {
				transform.position = Vector3.MoveTowards (transform.position, moveTo, escapeSpeed * Time.deltaTime);
				if (transform.position == moveTo) {
					state = ShipState.shooting;
				}
				yield return null;
			} else {
				throw new UnityException ("invalid state: " + state.ToString ());
			}
		}
		StartCoroutine (MoveOut ());

	}

	IEnumerator MoveOut(){
		float screenHalfHeight = Camera.main.orthographicSize;
		float targetY = screenHalfHeight + transform.localScale.y;
		float speed = 10;
		while (transform.position.y < targetY) {
			transform.Translate (Vector2.down * speed * Time.deltaTime);
			yield return null;
		}
		Destroy (gameObject);
	}

	void RandomShoot(){
		GameObject player = GameObject.FindGameObjectWithTag ("player");
		Debug.Assert (player != null, "player not found!");
		// set random angle
		if (player != null) {
			float shootAngle = Random.Range (0, shootAngleMax);
			if (player.transform.position.x < transform.position.x) {
				shootAngle = -shootAngle;
			}
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler(Vector3.forward * shootAngle));
		}
	}

	public override void TakeDamage(int damage){
		base.TakeDamage (damage);
		StartCoroutine (DamageAnimation ());
		if (!dead && state != ShipState.moving) {
			state = ShipState.moving;
			moveTo = GetRandomEscapePosition ();
		}
	}

	public override void Die(){
		AudioManager.instance.PlaySound (AudioStore.instance.spaceshipDeath);
		base.Die ();
	}

	Vector3 GetRandomEscapePosition(){
		float screenHalfHeight = Camera.main.orthographicSize;
		float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		float targetX = Random.Range (0.3f, 0.7f) * screenHalfWidth;
		if (transform.position.x > 0)
			targetX = -targetX;
		float targetY = screenHalfHeight * Random.Range (0.3f, 0.7f);
		return new Vector3 (targetX, targetY, 0);
	}

	IEnumerator DamageAnimation(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = originalColor;
	}
}
