using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFallBlock : MonoBehaviour
{
	public float rotateSpeed;
	public float fallSpeed;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(rotate());
	}

	IEnumerator rotate()
	{
		var angle = 0f;
		while (true)
		{
			angle += rotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(Vector3.forward*angle);
			transform.position = new Vector3(transform.position.x,transform.position.y - fallSpeed*Time.deltaTime,transform.position.z);
			
			yield return null;
		}
	}
}
