using System.Collections;
using System.Collections.Generic;

public enum WeaponType {
	RingProtector,
	SliderProtector,
	ScreenBomber,
	SolidShield
};


public static class WeaponConstants{
	public static Dictionary<WeaponType,string> typeToBlockImgName = new Dictionary<WeaponType,string>{
		{WeaponType.RingProtector,"block-ring-protector"},
		{WeaponType.ScreenBomber,"block-screen-bomb"},
		{WeaponType.SliderProtector,"block-slider-protector"},
		{WeaponType.SolidShield,"block-solid-shield"}
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