using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShootBoss : MonoBehaviour, IControlledAttacker
{
	public GameObject laserPrefab;
	public Transform[] muzzles;
	public Color laserColor;
	public float laserStayTime;
	public float laserExpandSpeed;
	public float moveSpeed;
	public float maxRandomOffset;
	public int minMuzzlesToUse;

	public bool autoStart;
	
	// Use this for initialization
	void Start () {
		if (autoStart)
		{
			StartAttack();
		}
	}

	IEnumerator moveAndAttack()
	{
		while (true)
		{
			// locate the player
			var playObj = GameObject.FindGameObjectWithTag("player");
			if (playObj != null)
			{
				var targetX = playObj.transform.position.x + Random.Range(-1, 1f) * maxRandomOffset;
				var targetPos = new Vector3(targetX,transform.position.y,0);
				// move to player
				while (transform.position.x != targetX)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
					yield return null;
				}
				
				// launch attack
				var numMuzzlesToUse = Random.Range(minMuzzlesToUse, muzzles.Length + 1);
				var muzzleIdxs = Utils.Sample(numMuzzlesToUse, muzzles.Length);
				foreach (var idx in muzzleIdxs)
				{
					var laserObj = Instantiate(laserPrefab, muzzles[idx].position, Quaternion.identity);
					Laser laser = laserObj.GetComponent<Laser>();
					Debug.Assert(laser!=null);
					laser.laserStayTime = laserStayTime;
					laser.expandSpeed = laserExpandSpeed;
					laserObj.GetComponent<SpriteRenderer>().color = laserColor;
				}
				
				yield return new WaitForSeconds(laserStayTime + 0.3f);
			}

			yield return null;
		}
	}

	public void StartAttack()
	{
		StartCoroutine(moveAndAttack());
	}
}
