using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IControlledAttacker))]
public class EnemyTrigger : MonoBehaviour
{
	public float delayBeforeAttack; // in seconds

	// before enemy become visible, should disable collider to prevent it from being killed
	void Start()
	{
		var collider = GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = false;
		}
	}
	
	// Update is called once per frame
	private void OnBecameVisible()
	{
		StartCoroutine(delayAndAttack());
	}

	IEnumerator delayAndAttack()
	{
		var collider = GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = true;
		}
		yield return new WaitForSeconds(delayBeforeAttack);
		GetComponent<IControlledAttacker>().StartAttack();
	}

	private void OnBecameInvisible()
	{
//		print(gameObject.name + " destroyed on invisible");
		Destroy(gameObject);
	}
}
