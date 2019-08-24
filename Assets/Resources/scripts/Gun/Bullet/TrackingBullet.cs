using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : MonoBehaviour {

	public string[] enemyTags = new string[] {"enemy:boss","enemy:alien-ship","enemy:guard","enemy:tracking-missile","enemy:falling-block"};
	public float moveSpeed = 10f;
	public float turnDelay = 1f;
	public bool moveUp = true;

	GameObject targetEnemy;

	void Start () {
		// find an enemy to target
		targetEnemy = GetTargetEnemy();
		
		if (targetEnemy != null) {
			StartCoroutine (TurnToFaceTarget());
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.up * moveSpeed * Time.deltaTime);
	}

	IEnumerator TurnToFaceTarget(){
		while (true) {
			if (targetEnemy == null) {
				// enemy already destroyed
				break;
			}
			Transform targetTrans = targetEnemy.transform;
			// look at target
			float xDiff = targetTrans.position.x - transform.position.x;
			float yDiff = targetTrans.position.y - transform.position.y;
			Vector2 targetDir = (new Vector2(xDiff,yDiff)).normalized;
			if ((yDiff < 0 && moveUp) || (yDiff > 0 && !moveUp)) {
				// we already fly over the enemy
				break;
			}
			float targetAngle = Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
			transform.eulerAngles = new Vector3(0,0,targetAngle);

			yield return new WaitForSeconds(turnDelay);
		}
	}

	GameObject GetTargetEnemy(){
		foreach (string tag in enemyTags) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag (tag);
			if (enemies.Length > 0) {
				return getClosestEnemy(enemies);
			}
		}
		return null;
	}

	GameObject getClosestEnemy(GameObject[] enemies)
	{
		var closest = enemies[0];
		var minDist = Vector3.Distance(transform.position, closest.transform.position);
		foreach (var enemy in enemies)
		{
			var dist = Vector3.Distance(enemy.transform.position, transform.position);
			if (dist < minDist)
			{
				closest = enemy;
				minDist = dist;
			}
		}

		return closest;
	}
}
