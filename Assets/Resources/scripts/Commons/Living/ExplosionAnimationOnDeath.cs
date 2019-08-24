using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class ExplosionAnimationOnDeath : MonoBehaviour {

	public ExplosionAnimation explosionEffect;
	public float animationLength = 1.5f;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<LivingEntity> ().OnDeath += AnimateExplosion;
	}
	
	// AnimateExplosion will fail silently if the explosionEffect is null
	void AnimateExplosion () {
		if (explosionEffect != null)
		{
			GameObject effect = Instantiate (explosionEffect.gameObject, gameObject.transform.position, gameObject.transform.rotation);
		
			ExplosionAnimation animation = effect.GetComponent<ExplosionAnimation> ();
			if (animation != null)
			{
				Vector3 bounds = gameObject.GetComponent<Renderer> ().bounds.size;
				float width = bounds.x; float height = bounds.y;
				animation.maxX = transform.position.x + width / 2;
				animation.minX = transform.position.x - width / 2;
				animation.maxY = transform.position.y + height / 2;
				animation.minY = transform.position.y - height / 2;
				animation.animationLength = animationLength;
			}
		}
		
	}
}
