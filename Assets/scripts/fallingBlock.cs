using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingBlock : MonoBehaviour {

	public float maxSpeed = 20;
	public float minSpeed = 5;
	public float speed;

	// Use this for initialization
	void Start () {
		speed = Mathf.Lerp (minSpeed, maxSpeed, Difficulty.GetDifficultyPercent ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.down * speed * Time.deltaTime);

		if (transform.position.y < -Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			FindObjectOfType<GameOver> ().OnGameOver ();
			Destroy (gameObject);
		}
		if (other.tag == "bullet") {
			Destroy (other.gameObject);
			Destroy (gameObject);
			FindObjectOfType<ScoreCtrl> ().addScore ();
		}
	}

}
