using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerStone : MonoBehaviour
{
	public GameObject leader;
	public FollowerStone directFollower;
	public float moveSpeed;
	public float distToLeader; // keep this distance to the leader
	public GameObject destroyEffect;
	
	private float distMargin = 0.1f;
	
	// Use this for initialization
	void Start () {
		
	}

	public void StartFollowing()
	{
		StartCoroutine(followLeader());
	}

	public void SetLeader(GameObject l)
	{
		leader = l;
		if (l.GetComponent<FollowerStone>() != null)
		{
			l.GetComponent<FollowerStone>().directFollower = this;
		}
	}

	// CascadeDestory destorys gameobject itself as well as all the followers
	public void CascadeDestory()
	{
		StartCoroutine(destroyRoutine());
	}

	IEnumerator destroyRoutine()
	{
		Utils.SetAlphaValue(gameObject,0); // hide first
		if (destroyEffect != null)
		{
			Instantiate(destroyEffect, transform.position, Quaternion.identity);
			yield return new WaitForSeconds(0.2f);
		}

		if (directFollower != null)
		{
			directFollower.CascadeDestory();
		}
		Destroy(gameObject);
	}
	
	IEnumerator followLeader()
	{
		while (leader!=null)
		{
			var dist = Vector3.Distance(transform.position, leader.transform.position);
			if (dist > distToLeader * (1+distMargin))
			{
				// move towards last leader position
				transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, moveSpeed * Time.deltaTime);
			}else if (dist < distToLeader * (1-distMargin))
			{
				// too close to leader, move away
				var targetX = 2 * transform.position.x - leader.transform.position.x;
				var targetY = 2 * transform.position.y - leader.transform.position.y;
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX,targetY,0), moveSpeed * Time.deltaTime);
			}
			yield return null;
		}
	}
}
