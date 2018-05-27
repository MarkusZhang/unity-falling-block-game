using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Image bar;
	float maxHealth;

	void UpdateHealthBar(int health){
		bar.fillAmount = health / maxHealth;
	}

	public void AttachToLivingEntity(LivingEntity entity){
		maxHealth = entity.startingHealth;
		entity.OnHealthChange += () => UpdateHealthBar (entity.GetHealth());
		entity.OnDeath += () => Destroy (gameObject);
	}
}
