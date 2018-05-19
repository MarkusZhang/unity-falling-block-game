using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a weapon block that keeps changing its type
public class ChangingWeaponBlock : MonoBehaviour {

	WeaponType type;
	public float fallingSpeed = 2;
	public float swingSpeed = 1;
	public float swingDist = 0.5f;
	public float typeSwitchInterval = 1f; // seconds between weapon switching

	// Use this for initialization
	void Start () {
		type = GetRandomType ();
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (type));
		StartCoroutine (SwingLeftRight ());
		StartCoroutine (SwitchType ());
	}
	
	// fall down
	void Update () {
		transform.Translate (Vector2.down * fallingSpeed * Time.deltaTime);

		if (transform.position.y < -Camera.main.orthographicSize) {
			Destroy (gameObject);
		}
	}

	IEnumerator SwingLeftRight(){
		float centerX = transform.position.x;
		Vector2 moveDir = Vector2.left;

		while (true) {
			transform.Translate (moveDir * swingSpeed * Time.deltaTime);
			if (transform.position.x < centerX - swingDist) {
				moveDir = Vector2.right;
			} else if (transform.position.x > centerX + swingDist) {
				moveDir = Vector2.left;
			}
			yield return null;
		}
	}

	IEnumerator SwitchType(){
		while (true) {
			type = GetRandomType ();
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (type));
			yield return new WaitForSeconds (typeSwitchInterval);
		}
	}

	// 30% probability for bomb, slider, ring, 10% for shield
	WeaponType GetRandomType(){
		float rand = Random.Range (0, 1f);
		if (rand <= 0.3f)
			return WeaponType.ScreenBomber;
		else if (rand <= 0.6f)
			return WeaponType.SliderProtector;
		else if (rand <= 0.9f)
			return WeaponType.RingProtector;
		else
			return WeaponType.SolidShield;
	}

	string TypeToImageName(WeaponType type){
		string imageName;
		WeaponConstants.typeToBlockImgName.TryGetValue (type, out imageName);
		if (imageName != null)
			return "images/weapon-blocks/" + imageName;
		else
			throw new UnityException (type.ToString () + " is not valid type");
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "player") {
			other.gameObject.GetComponent<Player> ().GetWeapon (type);
			Destroy (gameObject);
		}
	}
}
