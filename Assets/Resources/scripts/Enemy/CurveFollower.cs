using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveFollower : MonoBehaviour
{
	public BezierCurve curve;
	public float moveInterval;
	public float stepSize = 0.01f;
	public bool rotateWithPath = true;
	
	private float t = 0;
	private Vector3 previousPt;
	
	public void StartPath ()
	{
		StartCoroutine(FollowPath());
	}

	IEnumerator FollowPath()
	{
		t = 0;
		previousPt = curve.GetStartPoint();
		while (t < 1)
		{
			var position = curve.GetPoint(t);
			// update position
			transform.position = position;
			
			// update rotation
			if (rotateWithPath)
			{
				Vector3 targetDir = (position - previousPt).normalized;
				var angle = 90 + Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
				transform.eulerAngles = Vector3.forward * angle;
			}
			
			t += stepSize;
			previousPt = position;
			yield return new WaitForSeconds(moveInterval);
		}
	}
}
