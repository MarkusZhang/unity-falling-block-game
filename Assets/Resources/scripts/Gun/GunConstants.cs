using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public enum GunType{
	Default,
	Spray,
	Wide,
	Ring,
	Track,
	Burst,
	Swing,
	Laser,
	Lightning
}


public static class GunConstants {
	public static Dictionary<GunType,string> typeToBlockImgName= new Dictionary<GunType,string>{
		{GunType.Default,"bullet-block-default"},
		{GunType.Ring,"bullet-block-ring"},
		{GunType.Wide,"bullet-block-wide"},
		{GunType.Spray,"bullet-block-spray"},
		{GunType.Track,"bullet-block-track"},
		{GunType.Burst,"bullet-block-burst"},
		{GunType.Swing,"bullet-block-swing"},
		{GunType.Laser,"bullet-block-laser"},
		{GunType.Lightning,"bullet-block-lightning"}
	};
	
	public static Dictionary<GunType,string> typeToPrefabName = new Dictionary<GunType, string>()
	{
		{GunType.Default,"Prefabs/guns/default-gun"},
		{GunType.Ring,"Prefabs/guns/ring-gun"},
		{GunType.Wide,"Prefabs/guns/wide-gun"},
		{GunType.Spray,"Prefabs/guns/spray-gun"},
		{GunType.Track,"Prefabs/guns/track-gun"},
		{GunType.Burst,"Prefabs/guns/burst-gun"},
		{GunType.Swing,"Prefabs/guns/swing-gun"},
		{GunType.Laser,"Prefabs/guns/laser-gun"},
		{GunType.Lightning,"Prefabs/guns/lightning-gun"},
	};

	private static string typeToIconNamePrefix = "images/UI/gun-icons/";
	public static Dictionary<GunType,string> typeToIconName = new Dictionary<GunType, string>()
	{
		{GunType.Default, typeToIconNamePrefix + "icon-default-gun"},
		{GunType.Ring,typeToIconNamePrefix + "icon-ring-gun"},
		{GunType.Wide,typeToIconNamePrefix + "icon-wide-gun"},
		{GunType.Spray,typeToIconNamePrefix + "icon-spray-gun"},
		{GunType.Track,typeToIconNamePrefix + "icon-track-gun"},
		{GunType.Burst,typeToIconNamePrefix + "icon-burst-gun"},
		{GunType.Swing,typeToIconNamePrefix + "icon-swing-gun"},
		{GunType.Laser,typeToIconNamePrefix + "icon-laser-gun"},
		{GunType.Lightning,typeToIconNamePrefix + "icon-lightning-gun"},
	};

	public static GunType GetTypeFromName(string name){
		foreach (GunType type in System.Enum.GetValues(typeof(GunType))){
			if (String.Equals(name, type.ToString(), StringComparison.CurrentCultureIgnoreCase)){
				return type;
			}
		}
		throw new KeyNotFoundException (name + " is not a name for any gun type");
	}
	
}