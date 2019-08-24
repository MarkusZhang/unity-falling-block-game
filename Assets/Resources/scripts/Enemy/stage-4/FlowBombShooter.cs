using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBombShooter : MonoBehaviour
{

	public float moveSpeed;
	public float genInterval = 1; // 1 second
	public GameObject flowBombPrefab;
	
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(moveDown());
		StartCoroutine(genFlowBomb());
	}

	IEnumerator genFlowBomb()
	{
		while (true)
		{
			Instantiate(flowBombPrefab, transform.position,Quaternion.identity);
			yield return new WaitForSeconds(genInterval);
		}
	}
	
	IEnumerator moveDown()
	{
		while (true)
		{
			transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
			yield return null;
		}
	}
}
