using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntityWithAudio {

	public float moveSpeed = 8;

	bool disabled = false;

	public GunManager gunManager;
	WeaponDispatcher weaponDispatcher;
	Color originalColor;
	Utils utils;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		weaponDispatcher = WeaponDispatcher.instance;

		gunManager = gameObject.AddComponent<GunManager>();

		utils = gameObject.AddComponent<Utils> ();

		ScoreCtrl.OnLevelChange += OnLevelUp;

		originalColor = GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		if (disabled) {
			return;
		}
		UseWeapon ();

		// get left-right moving command
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		Vector2 dirToMove = new Vector2 (inputX, inputY).normalized;
		Vector2 myPosition = new Vector2 (transform.position.x, transform.position.y);
		if (!utils.IsOffScreen (myPosition + dirToMove * moveSpeed * Time.deltaTime)) {
			transform.Translate (dirToMove * moveSpeed * Time.deltaTime);
		}

		// get shooting bullet command
		if (Input.GetKey(KeyCode.Space)) {
			gunManager.Shoot ();
		}

		// get switch gun command
		if (Input.GetKeyDown (KeyCode.Tab)) {
			gunManager.SwitchGun ();
		}
	}

	void UseWeapon(){
		if (Input.GetKeyDown (KeyCode.A)) {
			TakeAndDispatchWeapon (WeaponType.RingProtector);
		} else if (Input.GetKeyDown (KeyCode.S)) {
			TakeAndDispatchWeapon (WeaponType.BackMissle);
		} else if (Input.GetKeyDown (KeyCode.D)) {
			TakeAndDispatchWeapon (WeaponType.SliderProtector);
		} else if (Input.GetKeyDown (KeyCode.F)) {
			TakeAndDispatchWeapon (WeaponType.SolidShield);
		}
	}

	void TakeAndDispatchWeapon(WeaponType type){
		if (WeaponStoreCtrl.TakeWeapon (type)) {
			weaponDispatcher.DispatchWeapon (type);
		}
	}

	public void GetWeapon(WeaponType type){
		WeaponStoreCtrl.StoreWeapon (type, 1);
		TakeAndDispatchWeapon (type);
	}

	public override void TakeDamage(int damage){
		base.TakeDamage (damage);
		StartCoroutine (DamageAnimation ());
	}

	IEnumerator DamageAnimation(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SpriteRenderer> ().color = originalColor;
	}

	void OnLevelUp(){
		int level = ScoreCtrl.GetLevel ();
		if (level == 1) {
			gunManager.SetRelativeShootSpeed (1.5f);
			moveSpeed += 2;
		} else if (level == 2) {
			gunManager.SetRelativeShootSpeed (2f);
			moveSpeed += 2;
		} else if (level == 3) {
			gunManager.SetRelativeShootSpeed (3f);
			moveSpeed += 1;
		} else if (level == 4) {
			gunManager.SetRelativeShootSpeed (4f);
			moveSpeed += 1;
		}
	}
}
