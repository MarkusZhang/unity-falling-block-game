using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class MoveWithAccl : MonoBehaviour
{
	public float startSpeed;
	public Vector2 dir;
	public float acceleration;
	public float minSpeed;
	public float maxSpeed;

	private float speed;
	
	// Use this for initialization
	void Start ()
	{
		speed = startSpeed;
		StartCoroutine(changeSpeed());
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(dir.normalized.x,dir.normalized.y,0) * speed * Time.deltaTime);
	}

	IEnumerator changeSpeed()
	{
		while (speed >= minSpeed && speed <= maxSpeed)
		{
			speed += acceleration * Time.deltaTime;
			yield return null;
		}
	}
}
