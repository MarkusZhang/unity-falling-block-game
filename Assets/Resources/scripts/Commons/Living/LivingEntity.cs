using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class LivingEntity : MonoBehaviour {

	public int startingHealth;
	protected int health;
	protected bool dead;
	float startAliveTime;

	public event System.Action OnDeath;
	public event System.Action OnTakeDamage;
	public event System.Action OnAddHealth; 

	protected virtual void Start() {
		health = startingHealth;
		startAliveTime = Time.time;
	}

	public virtual void TakeDamage(int damage) {
		health -= Mathf.Min(health,damage);
	
		if (OnTakeDamage != null) {
			OnTakeDamage ();
		}

		if (health <= 0 && !dead) {
			Die();
		}
	}

	public virtual void Die() {
		dead = true;

		if (GlobalConfig.LogLivingEntity)
		{
			print(gameObject.name + " killed in " + (Time.time-startAliveTime) + " seconds");
		}
		
		if (OnDeath != null) {
			OnDeath();
		}
		Destroy (gameObject);
	}

	public int GetHealth(){
		return health;
	}

	public float GetStartAliveTime(){
		return startAliveTime;
	}

	public virtual void AddHealth(int amount){
		if (health < startingHealth) {
			health += amount;
			if (OnAddHealth != null) {
				OnAddHealth ();
			}
		}
	}
}

