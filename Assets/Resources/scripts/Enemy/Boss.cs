using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : LivingEntity {

//	bool isLaunchingAttack = false;
	float screenHalfWidth;

	public Transform blockMuzzleContainer; // muzzles for shooting falling block
	public Transform BulletMuzzleContainer;
	public GameObject blockPrefab;
	public GameObject bulletPrefab;
	public GameObject hitEffect;
	public GameObject attackDownWeapon;

	public int attackDownInterval; // # attacks taken before attacking down

	int numConsecDamagesTaken;
	Color originalColor;
	float startY = 3.2f; // the initial Y position boss will move to

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		transform.position = new Vector2(0,Camera.main.orthographicSize+2);
		originalColor = GetComponent<SpriteRenderer> ().color;
		StartCoroutine (MoveToStage ());
		StartCoroutine (AttackCoroutine ());
	}

	IEnumerator MoveToStage(){
		float moveSpeed = 4;
		while (transform.position.y >  startY) {
			transform.Translate (Vector2.down * moveSpeed * Time.deltaTime);
			yield return null;
		}
		StartCoroutine (MoveLeftRight ());

	}

	IEnumerator MoveLeftRight(){
		float moveSpeed = 2;
		Vector2 moveDir = Vector2.left;
		while (!dead) {
			if (transform.position.x < -screenHalfWidth) {
				moveDir = Vector2.right;
			} else if (transform.position.x > screenHalfWidth) {
				moveDir = Vector2.left;
			}
			transform.Translate (moveDir * moveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator AttackCoroutine(){
		while (!dead) {
			// at each call, randomly decide whether to do attack
			int randNum = Random.Range (0, 6); // 0,1,2,3,4,5

			if (randNum >= 1 && randNum <= 3) {
				ShootFallingBlocks ();
				yield return new WaitForSeconds (2);
			} else if (randNum >= 4) {
				ShootBullets ();
				yield return new WaitForSeconds (2);
			} else {
				yield return new WaitForSeconds (0.5f);
			}

		}
	}

	// drop to the bottom of the screen
	IEnumerator AttackDown(){
		float moveDownSpeed = 10;
		float moveUpSpeed = 12;
		float targetY = - Camera.main.orthographicSize + transform.localScale.y;

		attackDownWeapon.SetActive (true);

		// move straight down
		while (transform.position.y >  targetY) {
			transform.Translate (Vector2.down * moveDownSpeed * Time.deltaTime);
			yield return null;
		}

		attackDownWeapon.SetActive (false);

		// move back up
		while (transform.position.y < startY) {
			transform.Translate (Vector2.up * moveUpSpeed * Time.deltaTime);
			yield return null;
		}
	}

	void ShootFallingBlocks(){
		foreach (Transform muzzle in blockMuzzleContainer) {
			Instantiate (blockPrefab, muzzle.position, muzzle.rotation);
		}
	}

	void ShootBullets(){
		foreach (Transform muzzle in BulletMuzzleContainer) {
			Instantiate (bulletPrefab, muzzle.position, muzzle.rotation);
		}
	}

	public override void TakeDamage(int damage){
		StartCoroutine (DamageAnimation ());
		base.TakeDamage (damage);
		numConsecDamagesTaken++;
		if (numConsecDamagesTaken >= attackDownInterval) {
			numConsecDamagesTaken = 0;
			// randomly choose whether to attack down
			if (Random.Range(0,1f) > 0.5f)
				StartCoroutine (AttackDown ());
		}
	}

	IEnumerator DamageAnimation(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = originalColor;
	}
}
