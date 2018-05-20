using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType{
	weapon,
	bullet,
	health
}

// interface class
[RequireComponent(typeof(Collider2D))]
public class Collectable : MonoBehaviour {

	public CollectableType type;
	public string name; 
	public string param;

	void OnTriggerEnter2D(Collider2D collider){
		if (IsCollector (collider.gameObject)) {
			ICollector c = collider.gameObject.GetComponent<ICollector> ();
			c.Collect (this);
			Destroy (gameObject);
		}
	}

	bool IsCollector(GameObject obj){
		return obj.GetComponent<ICollector> () != null;
	}
}
