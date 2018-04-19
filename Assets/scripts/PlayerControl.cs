using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private float speed = 7;
	private float screenHalfWidth;

	public GameObject bulletPrefab;
	public Transform mainProjectile;
	public Transform leftProjectile;
	public Transform rightProjectile;

	private float lastShootTime;
	public float shootMinInterval = .4f;

	bool disabled = false;
	bool sideProjectileEnabled = false;
	float bulletScale = 1;

	// Use this for initialization
	void Start () {

		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
		lastShootTime = Time.time;
	}

	void ShootBullet(){
		lastShootTime = Time.time;
		Vector3 spawnPosition = mainProjectile.position;
		GameObject bullet = Instantiate (bulletPrefab, spawnPosition, Quaternion.identity);
		bullet.transform.localScale = bulletScale * bullet.transform.localScale;

		if (sideProjectileEnabled) {
			GameObject leftBullet = Instantiate (bulletPrefab, leftProjectile.position, Quaternion.identity);
			leftBullet.transform.localScale = bulletScale * leftBullet.transform.localScale;
			GameObject rightBullet = Instantiate (bulletPrefab, rightProjectile.position, Quaternion.identity);
			rightBullet.transform.localScale = bulletScale * rightBullet.transform.localScale;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (disabled) {
			return;
		}

		// get left-right moving command
		float inputX = Input.GetAxisRaw ("Horizontal");
		float velocity = inputX * speed;
		transform.Translate (Vector2.right * velocity * Time.deltaTime);

		// wrap around the scene
		if (transform.position.x < -screenHalfWidth) {
			transform.position = new Vector2(screenHalfWidth,transform.position.y);
		} else if (transform.position.x > screenHalfWidth) {
			transform.position = new Vector2(- screenHalfWidth, transform.position.y);
		}

		// get shooting bullet command
		float inputY = Input.GetAxisRaw("Vertical");
		if (inputY > 0 && Time.time - lastShootTime > shootMinInterval) {
			ShootBullet ();
		}
	}

	public void changeShootSpeed(float interval){
		shootMinInterval = interval;
	}

	public void enableSideProjectile(){
		sideProjectileEnabled = true;
	}
}
