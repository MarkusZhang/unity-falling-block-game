using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType{
	Default,
	Spray,
	Wide,
	Ring
}


public static class GunConstants {
	public static Dictionary<GunType,string> typeToBlockImgName= new Dictionary<GunType,string>{
		{GunType.Ring,"bullet-block-ring"},
		{GunType.Wide,"bullet-block-wide"},
		{GunType.Spray,"bullet-block-spray"}
	};

	public static GunType GetTypeFromName(string name){
		foreach (GunType type in System.Enum.GetValues(typeof(GunType))){
			if (name == type.ToString()){
				return type;
			}
		}
		throw new KeyNotFoundException (name + " is not a name for any gun type");
	}
}