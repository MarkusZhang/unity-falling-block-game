using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	public static Utils instance;

	void Awake(){
		instance = this;
	}

	public bool IsOffScreen(Vector2 position){
		float screenHalfHeight = Camera.main.orthographicSize;
		float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		if (position.x > screenHalfWidth || position.x < -screenHalfWidth
		    || position.y > screenHalfHeight || position.y < -screenHalfHeight) {
			return true;
		} else {
			return false;
		}
	}
}
