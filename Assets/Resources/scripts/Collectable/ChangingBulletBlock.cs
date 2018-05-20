using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a weapon block that keeps changing its type
[RequireComponent(typeof(Collectable))]
public class ChangingBulletBlock : MonoBehaviour {

	public float typeSwitchInterval = 1f; // seconds between weapon switching
	public int numBulletsAwarded = 50;


	// Use this for initialization
	void Start () {
		GunType type = GetRandomType ();
		SetCollectableType (type);
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (type));
		StartCoroutine (SwitchType ());
	}


	IEnumerator SwitchType(){
		while (true) {
			GunType type = GetRandomType ();
			SetCollectableType (type);
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (type));
			yield return new WaitForSeconds (typeSwitchInterval);
		}
	}

	GunType GetRandomType(){
		float rand = Random.Range (0, 1f);
		if (rand <= 0.3f)
			return GunType.Spray;
		else if (rand <= 0.7f)
			return GunType.Wide;
		else
			return GunType.Ring;
	}

	string TypeToImageName(GunType type){
		string imageName;
		GunConstants.typeToBlockImgName.TryGetValue (type, out imageName);
		if (imageName != null)
			return "images/bullet-blocks/" + imageName;
		else
			throw new UnityException (type.ToString () + " is not valid type");
	}

	void SetCollectableType(GunType type){
		Collectable c = gameObject.GetComponent<Collectable> ();
		c.name = type.ToString ();
		c.param = numBulletsAwarded.ToString();
	}
}
