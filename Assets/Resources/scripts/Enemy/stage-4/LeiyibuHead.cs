using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeiyibuHead : MonoBehaviour
{
	public float upMoveDist = 0.1f;
	public float downMoveDist = 0.2f;
	public float upMoveTime = 0.5f;
	public float downMoveTime = 0.5f;
	
	public GameObject blinkEffect;
	public Transform[] eyes;
	
	private Vector3 originalPosition;
	
	public void PreAttack()
	{
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySound(AudioStore.instance.bossCut);
		}
		originalPosition = transform.position;
		foreach (var eye in eyes)
		{
			var effect = Instantiate(blinkEffect, eye.position, Quaternion.identity);
			effect.transform.parent = transform;
		}

		StartCoroutine(preAttackAnim());
	}

	public void PostAttack()
	{
		transform.position = originalPosition;
	}

	IEnumerator preAttackAnim()
	{
		var startTime = Time.time;
		var upMoveSpeed = upMoveDist * Time.deltaTime / upMoveTime;
		while (Time.time - startTime < upMoveTime)
		{
			transform.Translate(Vector3.up * upMoveSpeed);
			yield return null;
		}

		startTime = Time.time;
		var downMoveSpeed = downMoveDist * Time.deltaTime / downMoveTime;
		while (Time.time - startTime < downMoveTime)
		{
			transform.Translate(Vector3.down * downMoveSpeed);
			yield return null;
		}
	}
}
