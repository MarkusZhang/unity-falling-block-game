using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QiangGe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "bullet") {
			StartCoroutine (MoveLeg ());
		}
	}

	IEnumerator MoveLeg(){
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("images/astronaut-2");
		yield return new WaitForSeconds (0.2f);
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("images/astronaut");
		yield return new WaitForSeconds (0.2f);
	}

}
