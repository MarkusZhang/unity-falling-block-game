using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Collider2D))]
public class Laser : MonoBehaviour
{

	public float expandSpeed;
	public float laserStayTime;

	private float initialTopY; // laser will keep its top y the same as initial
	
	// Use this for initialization
	void Start ()
	{
		var bounds = GetComponent<Renderer> ().bounds.size;
		initialTopY = transform.position.y + bounds.y / 2;
		StartCoroutine(ExpandLaser());
	}

	IEnumerator ExpandLaser()
	{
		float screenBottomY = - Camera.main.orthographicSize;
		var startExpandTime = Time.time;
		while (getBottomY() > screenBottomY)
		{
			// expand size
			float newScaleY = transform.localScale.y + expandSpeed * Time.deltaTime;
			transform.localScale = new Vector3(transform.localScale.x,newScaleY, transform.localScale.z);
			
			// move to keep top y unchanged
			float newPosY = initialTopY - GetComponent<Renderer> ().bounds.size.y / 2; 
			transform.position = new Vector3(transform.position.x,newPosY,transform.position.z);
			
			// resize collider to match
			var collider = GetComponent<Collider2D>(); 
			collider.transform.localScale = transform.localScale;
			collider.transform.position = transform.position;

			yield return new WaitForSeconds(0.05f);
		}
		
		yield return new WaitForSeconds(laserStayTime - (Time.time - startExpandTime));
	
		StartCoroutine(destroyLaser());
	}

	IEnumerator destroyLaser()
	{
		float screenBottomY = - Camera.main.orthographicSize;
		var squeezeSpeed = 2 * expandSpeed;
		while (transform.localScale.y > 0.05f)
		{
			// reduce size
			float newScaleY = transform.localScale.y - squeezeSpeed * Time.deltaTime;
			transform.localScale = new Vector3(transform.localScale.x,newScaleY, transform.localScale.z);
			
			// move to keep bottom y unchanged
			float newPosY = screenBottomY + GetComponent<Renderer> ().bounds.size.y / 2; 
			transform.position = new Vector3(transform.position.x,newPosY,transform.position.z);
			
			yield return new WaitForSeconds(0.05f);
		}
		
		Destroy(gameObject);
	}

	float getTopY()
	{
		var bounds = GetComponent<Renderer> ().bounds.size;
		return transform.position.y + bounds.y / 2;
	}
	
	float getBottomY()
	{
		var bounds = GetComponent<Renderer> ().bounds.size;
		return transform.position.y - bounds.y / 2;
	}
}
