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
}