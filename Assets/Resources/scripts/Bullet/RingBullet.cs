using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBullet : MonoBehaviour {

	private float currentAngle;
	public int rotationSpeed = 4; // degrees rotated per frame
	public float startRotationRadius = 4;
	public float rotationRadiusIncreaseSpeed = 0.1f;
	public Transform center;
	public float translateSpeed = 4;

	float rotationRadius;

	// Use this for initialization
	void Start () {
		rotationRadius = startRotationRadius;
		currentAngle = Random.Range(0f,360f);
		StartCoroutine (MoveForward ());
	}

	// Rotating bullet around center
	void Update () {
		float x = rotationRadius * Mathf.Cos (currentAngle * Mathf.Deg2Rad);
		float y = rotationRadius * Mathf.Sin (currentAngle * Mathf.Deg2Rad);
		transform.position = new Vector2 (center.position.x + x, center.position.y + y);
		currentAngle = (currentAngle + rotationSpeed) % 360;

		rotationRadius += rotationRadiusIncreaseSpeed * Time.deltaTime;
	}

	// translate the center upward
	IEnumerator MoveForward(){
		while (true) {
			center.Translate (Vector2.up * translateSpeed * Time.deltaTime);
			if (center.position.y > Camera.main.orthographicSize) {
				Destroy (transform.parent.gameObject);
			}
			yield return null;
		}

	}
}
