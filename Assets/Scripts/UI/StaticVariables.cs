using UnityEngine;
using System.Collections.Generic;

public static class StaticVariables {

	public enum Powers {
		RESHAPE_CIRCLE,
		REVEAL,
		CHANGE_WEIGHT,
		CHANGE_MATERIAL
	}

	private static List<Powers> EnabledPowers = new List<Powers>(){
		Powers.CHANGE_WEIGHT,
		Powers.RESHAPE_CIRCLE,
		Powers.REVEAL,
		Powers.CHANGE_MATERIAL
	};

	public static float LightWeight = 1.2f;
	public static float HeavyWeight = 3f;

	public static bool HasPower(Powers power) {
		return EnabledPowers.Contains(power);
	}

	public static void AddPower(Powers power) {
		EnabledPowers.Add(power);
	}
}

