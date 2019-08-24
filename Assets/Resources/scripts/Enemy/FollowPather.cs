using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED: should use DestroyWhenGoingOffScreen, Path interface
// this makes the game object follow a path
[RequireComponent(typeof(LivingEntity))]
public class FollowPather : MonoBehaviour
{
	private Vector3[] waypoints;
	public float moveSpeed;
	public bool destroyAfterPath;
	public event System.Action OnFollowPatherDestroyed; // event triggered when enemy killed or destroyed out of path
	
	// set path and start following
	public void SetPath(Transform[] pathPoints)
	{
		Debug.Assert(pathPoints.Length > 0);
		waypoints = new Vector3[pathPoints.Length];
		for (int i = 0; i < pathPoints.Length; i++)
		{
			waypoints[i] = pathPoints[i].position;
		}
		StartCoroutine(FollowPath());
	}
	
	IEnumerator FollowPath(){
		// move along the way points 
		transform.position = waypoints[0];

		// set the next target point
		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints [targetWaypointIndex];

		while (true) {
			// move toward next waypoint
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, moveSpeed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				if (targetWaypointIndex == waypoints.Length - 1) // we have reached the final point
				{
					break;
				}
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
			}
			yield return null;
		}
		
		// destroy itself after finishing path
		if (destroyAfterPath)
		{
			if (OnFollowPatherDestroyed != null)
			{
				OnFollowPatherDestroyed();
			}
			Destroy(gameObject);
		}
	}
}
