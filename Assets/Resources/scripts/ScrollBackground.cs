using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {

	public float scrollSpeed = -5f;
	Vector2 startPos;

	// Use this for initialization
	void Start () {
		// resize background to fill screen
//		Debug.Assert(transform.childCount == 2);
//		for (int i = 0; i < 2; i++) {
//			GameObject childObj = transform.GetChild (i).gameObject;
//			Resize (childObj);
//			print ("finish resizing");
//		}
//			
//		float worldScreenHeight=Camera.main.orthographicSize*2f;
//		Transform firstObjTrans = transform.GetChild (0);
//		Transform secondObjTrans = transform.GetChild (1);
//		secondObjTrans.position = new Vector3 (firstObjTrans.position.x,
//			firstObjTrans.position.y + worldScreenHeight, firstObjTrans.position.z);
//		print ("finish positioning");
//
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		float newPos = Mathf.Repeat (Time.time * scrollSpeed, 10);
		transform.position = startPos + Vector2.down * newPos;
	}

	void Resize(GameObject o)
	{
		SpriteRenderer sr= o.GetComponent<SpriteRenderer>();
		if(sr==null) return;

		o.transform.localScale=new Vector3(1,1,1);

		float width=sr.sprite.bounds.size.x;
		float height=sr.sprite.bounds.size.y;


		float worldScreenHeight=Camera.main.orthographicSize*2f;
		float worldScreenWidth=worldScreenHeight/Screen.height*Screen.width;

		Vector3 xWidth = o.transform.localScale;
		xWidth.x=worldScreenWidth / width;
		o.transform.localScale=xWidth;

		Vector3 yHeight = o.transform.localScale;
		yHeight.y=worldScreenHeight / height;
		o.transform.localScale=yHeight;

	}
}
