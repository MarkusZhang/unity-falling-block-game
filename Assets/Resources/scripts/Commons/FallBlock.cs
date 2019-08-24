using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// component that moves the object down until out of screen
public class FallBlock : MonoBehaviour {
	
	public float fallingSpeed = 2;
	public bool moveDown = true;
	
	// Update is called once per frame
	void Update () {
		Vector2 moveDir = moveDown ? Vector2.down : Vector2.up;
		transform.Translate (moveDir * fallingSpeed * Time.deltaTime);
	}
}
