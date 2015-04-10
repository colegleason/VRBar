using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

[System.Serializable]
public class VRCPlayerMod : VRCSDK2.VRCPlayerMod
{
	public VRCPlayerMod(string modName, List<VRCSDK2.VRCPlayerModProperty> defaultProperties, string modComponentName)
		: base( modName, defaultProperties,  modComponentName)
	{
	}
}
