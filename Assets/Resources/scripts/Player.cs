using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity {

	private float speed = 7;
	private float screenHalfWidth;

	bool disabled = false;

	GunManager gunManager;
	WeaponDispatcher weaponDispatcher;
	Color originalColor;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		weaponDispatcher = gameObject.AddComponent<WeaponDispatcher>();
		weaponDispatcher.Init ();

		gunManager = gameObject.AddComponent<GunManager> ();
		gunManager.Init (transform);

		ScoreCtrl.OnLevelChange += OnLevelUp;

		screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
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
		if (inputY > 0) {
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
			TakeAndDispatchWeapon (WeaponType.ScreenBomber);
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
		} else if (level == 2) {
			gunManager.SetRelativeShootSpeed (2f);
		} else if (level == 3) {
			gunManager.SetRelativeShootSpeed (3f);
		} else if (level == 4) {
			gunManager.SetRelativeShootSpeed (4f);
		}
	}
}
