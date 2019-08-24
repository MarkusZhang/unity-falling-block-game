using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity),typeof(BulletAttacker))]
public class ShootOnDeath : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		GetComponent<LivingEntity>().OnDeath += OnDeath;
	}
	
	void OnDeath () {
		GetComponent<BulletAttacker>().Attack();
	}
}
