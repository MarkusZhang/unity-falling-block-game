using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStoreDisplay : MonoBehaviour {

	public Dictionary<WeaponType,GameObject> typeToUI;
	public GameObject weaponDisplayCanvas;

	// Use this for initialization
	void Start () {
		typeToUI = new Dictionary<WeaponType,GameObject> ();
		// add UI object references
		Transform canvasTransform = weaponDisplayCanvas.transform;
		typeToUI.Add(WeaponType.RingProtector,canvasTransform.Find ("ui-ring-protector").gameObject);
		typeToUI.Add(WeaponType.SliderProtector,canvasTransform.Find("ui-slider-protector").gameObject);
		typeToUI.Add(WeaponType.ScreenBomber,canvasTransform.Find("ui-screen-bomber").gameObject);
		typeToUI.Add(WeaponType.SolidShield,canvasTransform.Find("ui-solid-shield").gameObject);

		WeaponStoreCtrl.OnWeaponStoreChange += UpdateUI;
	}
	
	void UpdateUI(){
		foreach (WeaponType type in System.Enum.GetValues(typeof(WeaponType))) {
			GameObject uiObject = null;
			typeToUI.TryGetValue(type, out uiObject);
			int weaponCount = WeaponStoreCtrl.GetWeaponCount (type);
			if (weaponCount > 0) {
				// show the weapon icon and update count
				uiObject.SetActive (true);
			} else {
				uiObject.SetActive (false);
			}
			SetUICount (uiObject, weaponCount);
		}
	}

	void SetUICount(GameObject ui, int count){
		Text countText = ui.transform.Find ("count").gameObject.GetComponent<Text>();
		countText.text = "x" + count;
	}
}
