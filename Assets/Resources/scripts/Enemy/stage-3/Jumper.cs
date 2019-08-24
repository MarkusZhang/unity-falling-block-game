using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jumper : MonoBehaviour
{
	public float jumpSpeed;
	public float rotateSpeed;
	
	// Use this for initialization
	public void StartJumpAround()
	{
		StartCoroutine("jumpAround");
		StartCoroutine("spin");
	}

	public void StopJumpAround()
	{
		StopCoroutine("jumpAround");
		StopCoroutine("spin");
	}

	IEnumerator jumpAround()
	{
		var xDir = (Random.Range(0, 1f) > 0.5f) ? 1 : -1;
		var yDir = -1; //(Random.Range(0, 1f) > 0.5f) ? 1 : -1;
		var moveDir = new Vector3(xDir,yDir,0).normalized;
		
		while (true)
		{
			// move until "almost" hitting screen border
			while (!Utils.IsOffScreen2D(transform.position + moveDir * Time.deltaTime * jumpSpeed))
			{
				transform.position += moveDir * Time.deltaTime * jumpSpeed;
				yield return null;
			}
			
			// we've hit the border, need to change dir
			moveDir = getNewDir(moveDir);
			transform.position = transform.position + moveDir * Time.deltaTime * jumpSpeed;
			yield return null;
		}
	}

	IEnumerator spin()
	{
		var angle = 0f;
		while (true)
		{
			transform.eulerAngles = Vector3.forward * angle;
			angle += rotateSpeed * Time.deltaTime % 360;
			yield return null;
		}
		 
	}

	Vector2 getNewDir(Vector2 originalDir)
	{
		float screenHalfHeight = Camera.main.orthographicSize;
		float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		var nextX = transform.position.x + Time.deltaTime * jumpSpeed * originalDir.normalized.x;
		var nextY = transform.position.y + Time.deltaTime * jumpSpeed * originalDir.normalized.y;
		
		var toLeft = nextX + screenHalfWidth;
		var toRight = screenHalfWidth - nextX;
		var toTop = screenHalfHeight - nextY;
		var toBottom = screenHalfHeight + nextY;
		
		if (toLeft < 0 || toRight < 0) // we are hitting vertical border
		{
			return new Vector2(-originalDir.x,originalDir.y);
		}
		else if (toTop < 0 || toBottom < 0)// we are hitting horizontal border
		{
			return new Vector2(originalDir.x,-originalDir.y);
		}
		else // we are hitting corner
		{
			return new Vector2(-originalDir.x,-originalDir.y);
		}
	}
}
