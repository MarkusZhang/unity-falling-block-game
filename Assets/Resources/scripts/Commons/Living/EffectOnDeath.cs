using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class EffectOnDeath : MonoBehaviour
{
	public GameObject effect;
	
	// Use this for initialization
	void Start ()
	{
		GetComponent<LivingEntity>().OnDeath += OnDeath;
	}
	
	// Update is called once per frame
	void OnDeath () {
		if (effect != null)
		{
			Instantiate(effect, transform.position, transform.rotation);
		}		
	}
}
