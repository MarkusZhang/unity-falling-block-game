using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveEnemy : MonoBehaviour
{
	public float speed;
	public int maxMoveTimes;

	private int moveTimes;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(randomMove());
	}

	IEnumerator randomMove()
	{
		while (true)
		{
			var x = Utils.GetRandomX(-0.9f, 0.9f);
			var y = Utils.GetRandomX(-0.9f, 0.9f);
			var targetPos = new Vector3(x,y,0);
			while (transform.position != targetPos)
			{
				transform.position = Vector3.MoveTowards(transform.position,targetPos,Time.deltaTime * speed);
				yield return null;
			}

			moveTimes++;
			if (moveTimes >= maxMoveTimes)
			{
				Destroy(gameObject);
				break;
			}
		}
	}
}
