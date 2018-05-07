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

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		transform.position = new Vector2(0,7);
		StartCoroutine (MoveToStage ());
	}

	IEnumerator MoveToStage(){
		float targetY = 3.2f;
		float moveSpeed = 2;
		while (transform.position.y >  targetY) {
			transform.Translate (Vector2.down * moveSpeed * Time.deltaTime);
			yield return null;
		}
		StartCoroutine (MoveLeftRight ());
		StartCoroutine (AttackCoroutine ());
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

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "bullet") {
			Vector2 collisionPosition = other.transform.position;
			Instantiate (hitEffect, collisionPosition, Quaternion.identity);
			TakeDamage (1);
			Destroy (other.gameObject);
		} 
	}
}
