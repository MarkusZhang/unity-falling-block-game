using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a weapon block that keeps changing its type
[RequireComponent(typeof(Collectable))]
public class ChangingBulletBlock : MonoBehaviour {

	public float typeSwitchInterval = 1f; // seconds between weapon switching
	public int numBulletsAwarded = 50;

	private GunType[] allTypes = new GunType[]{GunType.Spray,GunType.Ring,GunType.Wide};
	private GunType currentType;
	
	// Use this for initialization
	void Start () {
//		currentType = GetRandomType ();
		Debug.Assert(allTypes.Length>0);
		currentType = allTypes[0];
		SetCollectableType (currentType);
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (currentType));
		StartCoroutine (SwitchType ());
	}


	IEnumerator SwitchType(){
		while (true) {
			currentType = GetNextType ();
			SetCollectableType (currentType);
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (TypeToImageName (currentType));
			yield return new WaitForSeconds (typeSwitchInterval);
		}
	}

//	GunType GetRandomType(){
//		float rand = Random.Range (0, 1f);
//		if (rand <= 0.3f)
//			return GunType.Spray;
//		else if (rand <= 0.7f)
//			return GunType.Wide;
//		else
//			return GunType.Ring;
//	}

	GunType GetNextType()
	{
		for (int i = 0; i < allTypes.Length; i++)
		{
			if (allTypes[i] == currentType)
			{
				return allTypes[(i + 1) % allTypes.Length];
			}
		}
		throw new UnityException("ChangeBulletBlock: Cannot get next gun type");
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
		c.cname = type.ToString ();
		c.param = numBulletsAwarded.ToString();
	}
}
