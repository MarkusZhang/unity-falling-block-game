using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntityWithAudio : LivingEntity {

	public string hitAudioName;
	public string deathAudioName;

	public override void TakeDamage(int damage) {
		if (hitAudioName != null && hitAudioName != "") {
			AudioManager.instance.PlaySound (AudioStore.instance.GetAudioSourceByName (hitAudioName));
		}
		base.TakeDamage (damage);
	}

	public override void Die() {
		if (deathAudioName != null && deathAudioName != "") {
			AudioManager.instance.PlaySound (AudioStore.instance.GetAudioSourceByName (deathAudioName));
		}
		base.Die ();
	}
}
