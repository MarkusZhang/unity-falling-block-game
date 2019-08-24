using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// a class for holding and drawing curve path
public class BezierCurve : MonoBehaviour
{
	public Transform[] path;

	// for gizmos
	private float gizmosTStep = 0.01f;
		
	// Update is called once per frame
	private void OnDrawGizmos()
	{
		if (path!=null && path.Length > 0)
		{
			float gizmosT = 0;
			var p0 = path[0].position;

			while (gizmosT < 1)
			{
				var p1 = bezierInterpolate(path, gizmosT);
				Gizmos.DrawLine(p0,p1);
				p0 = p1;
				gizmosT += gizmosTStep;
			}
		}
	}
	
	// Bezier interpolation with any number of mid points
	Vector3 bezierInterpolate(Transform[] points, float t)
	{
		var pts = points.Select(p=>p.position).ToArray();
		for (var iter = 0; iter < pts.Length - 1; iter++){
			for (var i = 0; i < pts.Length - 1; i++){
				pts[i] = Vector3.Lerp(pts[i],pts[i+1],t);
			}
		}
    
		return pts[0];
	}

	public Vector3 GetStartPoint()
	{
		Debug.Assert(path.Length>0);
		return path[0].position;
	}

	// `t` is between 0 and 1
	public Vector3 GetPoint(float t)
	{
		return bezierInterpolate(path, t);
	}

}
