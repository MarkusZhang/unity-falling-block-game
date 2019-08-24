using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class ChangeColorOnDamage : MonoBehaviour
{
	public Color colorWhenDamage = Color.red;
	public float changeColorSeconds = 0.1f; // time before returning to original color
	public bool keepFullOpacity = true;
	
	private Color originalColor;
	private bool isChangingColor;
	
	// Use this for initialization
	void Start ()
	{
		var r = GetComponent<SpriteRenderer>();
		if (r != null)
		{
			originalColor = r.color;
			if (keepFullOpacity) // we will discard the alpha info
			{
				originalColor = new Color(originalColor.r,originalColor.g,originalColor.b,1f);
			}
		}

		GetComponent<LivingEntity>().OnTakeDamage += OnTakeDamage;

		isChangingColor = false;
	}

	void OnTakeDamage()
	{
		if (!isChangingColor)
		{
			StartCoroutine(ChangeColor());
		}
	}

	IEnumerator ChangeColor()
	{
		isChangingColor = true;
		var alpha = GetComponent<SpriteRenderer>().color.a;
		GetComponent<SpriteRenderer>().color = new Color(colorWhenDamage.r,colorWhenDamage.g,colorWhenDamage.b,alpha);
		yield return new WaitForSeconds(changeColorSeconds);
		alpha = GetComponent<SpriteRenderer>().color.a;
		GetComponent<SpriteRenderer>().color = new Color(originalColor.r,originalColor.g,originalColor.b,alpha);
		isChangingColor = false;
	}
}
