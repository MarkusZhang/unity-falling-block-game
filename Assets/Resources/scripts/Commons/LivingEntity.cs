using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour {

	public int startingHealth;
	protected int health;
	protected bool dead;

	public event System.Action OnDeath;
	public event System.Action OnHealthChange;

	protected virtual void Start() {
		health = startingHealth;
	}

	public virtual void TakeDamage(int damage) {
		health -= damage;

		if (OnHealthChange != null) {
			OnHealthChange ();
		}

		if (health <= 0 && !dead) {
			Die();
		}
	}

	public virtual void Die() {
		dead = true;
		if (OnDeath != null) {
			OnDeath();
		}
		GameObject.Destroy (gameObject);
	}

	public int GetHealth(){
		return health;
	}

	public void AddHealth(int amount){
		health += amount;
		if (OnHealthChange != null) {
			OnHealthChange ();
		}
	}
}

