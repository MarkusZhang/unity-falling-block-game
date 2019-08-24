using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils {
	public static bool IsOffScreen2D(Vector2 pos)
	{
		return IsOffScreen2D(pos, Vector2.zero);
	}
	
	public static bool IsOffScreen2D(Vector2 pos, Vector2 halfSize)
	{
		float screenHalfHeight = Camera.main.orthographicSize;
		float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		if (pos.x + halfSize.x> screenHalfWidth || pos.x - halfSize.x< -screenHalfWidth
		                            || pos.y + halfSize.y > screenHalfHeight || pos.y - halfSize.y < -screenHalfHeight) {
			return true;
		} else {
			return false;
		}
	}
	

	public static bool IsOffBottom(Vector2 pos)
	{
		float screenHalfHeight = Camera.main.orthographicSize;
		return pos.y < -screenHalfHeight;
	}

	public static bool IsOffTop(Vector2 pos)
	{
		float screenHalfHeight = Camera.main.orthographicSize;
		return pos.y > screenHalfHeight;
	}

	public static float GetRandomX()
	{
		return GetRandomX(-1f, 1f);
	}

	public static float GetRandomX(float min, float max)
	{
		float screenHalfHeight = Camera.main.orthographicSize;
		float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
		return Random.Range(min, max) * screenHalfWidth;
	}

	public static float GetRandomY(float min, float max)
	{
		float screenHalfHeight = Camera.main.orthographicSize;
		return Random.Range(min, max) * screenHalfHeight;
	}

	public static float GetTopY()
	{
		return Camera.main.orthographicSize;
	}

	public static float GetRightX()
	{
		return Camera.main.aspect * Camera.main.orthographicSize;
	}

	public static Vector3 GetRandomPos(float xMin, float xMax, float yMin, float yMax)
	{
		var x = GetRandomX(xMin, xMax);
		var y = GetRandomY(yMin, yMax);
		return new Vector3(x,y,0);
	}

	public static void SetAlphaValue(GameObject obj, float alpha)
	{
		if (obj != null)
		{
			var c = obj.GetComponent<SpriteRenderer>().color;
			obj.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, alpha);
		}
	}

	public static void SetAlphaValues(GameObject[] objs, float alpha)
	{
		foreach (var obj in objs)
		{
			SetAlphaValue(obj,alpha);
		}
	}

	// sample `numToSelect` numbers from [0,1,2....numToSelect - 1]
	public static int[] Sample(int numToSelect, int totalNum)
	{
		int[] arr = Enumerable.Range(0, totalNum).ToArray();
		System.Random rnd = new System.Random();
	
		return arr.OrderBy(r => rnd.Next()).Take(numToSelect).ToArray();
	}

	// get angle in degree from `from` to `to`
	public static float getAngleTo(Vector3 from, Vector3 to)
	{
		float xDiff = to.x - from.x;
		float yDiff = to.y - from.y;
		return Mathf.Atan2 (yDiff, xDiff) * Mathf.Rad2Deg;
	}
	
	public static void ResetStaticEventListeners(){
		ScoreCtrl.RemoveEventListeners();
		WeaponStoreCtrl.RemoveEventListeners();
		GunStore.RemoveEventListeners();
		LifeCtrl.RemoveEventListeners();
	}

	public static void ResetStaticComponents()
	{
		ScoreCtrl.Reset();
		WeaponStoreCtrl.Reset();
		GunStore.Reset();
		LifeCtrl.Reset();
	}
}
