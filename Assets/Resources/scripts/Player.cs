using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity {

	private float moveSpeed;
	public GameObject leftSideGun;
	private GameObject rightSideGun;

	bool disabled = false;
	private bool isHidden = false;

	public GunManager gunManager;
	public float startHideTime = 2f;
	public GameObject sideGunPrefab;
	
	WeaponDispatcher weaponDispatcher;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		
		StartCoroutine(hideSelf(startHideTime)); // be non-attackable for a short period of time
		
		weaponDispatcher = WeaponDispatcher.instance;

		gunManager = gameObject.AddComponent<GunManager>();
		gunManager.Initialize(gameObject.transform);
		
		ScoreCtrl.OnLevelChange += OnLevelUp;
		OnLevelUp(); // need to call this, otherwise at the beginning of stage 2, we won't get correct speed

		moveSpeed = ScoreCtrl.GetLevelMoveSpeed();
	}
	
	// Update is called once per frame
	void Update () {
		if (disabled) {
			return;
		}

		// get left-right moving command
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		Vector2 dirToMove = new Vector2 (inputX, inputY).normalized;
		Vector2 myPosition = new Vector2 (transform.position.x, transform.position.y);
		if (!Utils.IsOffScreen2D(myPosition + dirToMove * moveSpeed * Time.deltaTime)) {
			transform.Translate (dirToMove * moveSpeed * Time.deltaTime);
		}

		// get shooting bullet command
		if (Input.GetKey(KeyCode.Space)) {
			gunManager.Shoot ();
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
		if (isHidden)
		{
			return;
		}

		var healthLeft = Mathf.Max(0, health - damage);
		LifeCtrl.UpdateHealth(healthLeft);
		base.TakeDamage (damage);
	}

	public override void AddHealth(int amount)
	{
		base.AddHealth(amount);
		LifeCtrl.UpdateHealth(health);
	}

	public override void Die()
	{
		base.Die();
		ScoreCtrl.OnLevelChange -= OnLevelUp;
	}

	public void OnLevelUp(){
		if (gunManager != null)
		{
			gunManager.SetRelativeShootSpeed (ScoreCtrl.GetLevelGunSpeed());
		}
		moveSpeed = ScoreCtrl.GetLevelMoveSpeed();
	}

	public void EquipSideGun()
	{
		if (leftSideGun == null)
		{
			var pos = transform.position - Vector3.left * 1f;
			leftSideGun = Instantiate(sideGunPrefab, pos, Quaternion.identity);
			initSideGun(leftSideGun,true);
		}else if (rightSideGun == null)
		{
			var pos = transform.position + Vector3.left * 1f;
			rightSideGun = Instantiate(sideGunPrefab, pos, Quaternion.identity);
			initSideGun(rightSideGun,false);
		}
	}

	void initSideGun(GameObject sideGun, bool isLeft)
	{
		sideGun.transform.parent = transform;
		var g = sideGun.GetComponent<SideGun>();
		g.side = isLeft ? -1 : 1;
		g.SetParent(transform);
		g.StartShoot();
	}


	IEnumerator hideSelf(float t)
	{
		// hide self and be non-attackable
		isHidden = true;
		var oriColor = GetComponent<SpriteRenderer>().color;
		GetComponent<SpriteRenderer>().color = new Color(oriColor.r,oriColor.g,oriColor.b,0.6f);
		
		yield return new WaitForSeconds(t);
		
		// show self
		isHidden = false;
		GetComponent<SpriteRenderer>().color = new Color(oriColor.r,oriColor.g,oriColor.b,1f);
	}
}
