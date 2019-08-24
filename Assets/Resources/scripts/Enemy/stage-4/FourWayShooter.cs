using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class FourWayShooter : LivingEntity,IControlledAttacker 
{

	public bool autoStart;
	public Transform[] leftMuzzles;
	public Transform[] rightMuzzles;
	public Transform[] upMuzzles;
	public Transform[] downMuzzles;

	public float attackInterval;
	public GameObject bulletPrefab;
	
	private Dictionary<string, Transform[]> muzzles;

	// Use this for initialization
	void Start () {
		base.Start();
		muzzles = new Dictionary<string, Transform[]>();
		muzzles.Add("up",upMuzzles);
		muzzles.Add("down",downMuzzles);
		muzzles.Add("left",leftMuzzles);
		muzzles.Add("right",rightMuzzles);
		
		if (autoStart)
		{
			StartAttack();
		}
	}

	public void StartAttack()
	{
		var jumper = GetComponent<Jumper>();
		if (jumper != null)
		{
			jumper.StartJumpAround();
		}

		StartCoroutine(shootAttack());
	}

	IEnumerator shootAttack()
	{
		while (true)
		{
			var playerRef = GameObject.FindGameObjectWithTag("player");
			if (playerRef != null)
			{
				var deltaX = playerRef.transform.position.x - transform.position.x;
				var deltaY = playerRef.transform.position.y - transform.position.y;
				var choices = new string[2];
				choices[0] = deltaX > 0 ? "right" : "left";
				choices[1] = deltaY > 0 ? "up" : "down";
				var choice = choices[Random.Range(0, 2)];
				shoot(choice);
			}
			yield return new WaitForSeconds(attackInterval);
		}
	}

	void shoot(string choice)
	{
		Transform[] muzzlesToUse;
		muzzles.TryGetValue(choice,out muzzlesToUse);
		foreach (var muzzle in muzzlesToUse)
		{
			Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
		}
	}
}
