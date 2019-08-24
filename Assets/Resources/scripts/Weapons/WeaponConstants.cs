using System.Collections;
using System.Collections.Generic;

public enum WeaponType {
	RingProtector,
	SliderProtector,
	BackMissle,
	SolidShield,
	SideGun
};


public static class WeaponConstants{
	private static string typeToBlockNamePrefix = "images/collectables/weapon-blocks/";
	
	public static Dictionary<WeaponType,string> typeToBlockImgName = new Dictionary<WeaponType,string>{
		{WeaponType.RingProtector,typeToBlockNamePrefix + "block-ring-protector"},
		{WeaponType.BackMissle,typeToBlockNamePrefix + "block-screen-bomb"},
		{WeaponType.SliderProtector,typeToBlockNamePrefix + "block-slider-protector"},
		{WeaponType.SolidShield,typeToBlockNamePrefix + "block-solid-shield"}
	};

	public static WeaponType GetTypeFromName(string name){
		foreach (WeaponType type in System.Enum.GetValues(typeof(WeaponType))){
			if (name == type.ToString()){
				return type;
			}
		}
		throw new KeyNotFoundException (name + " is not a name for any weapon type");
	}
}