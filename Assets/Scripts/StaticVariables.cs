using UnityEngine;
using System.Collections.Generic;

public static class StaticVariables {

	public enum Powers {
		RESHAPE_CIRCLE,
		REVEAL,
		CHANGE_WEIGHT
	}

	private static List<Powers> EnabledPowers = new List<Powers>(){
		Powers.CHANGE_WEIGHT,
		Powers.RESHAPE_CIRCLE,
		Powers.REVEAL
	};

	public static int WeightSoft = 20;
	public static int WeightHeavy = 100;

	public static bool HasPower(Powers power) {
		return EnabledPowers.Contains(power);
	}

	public static void AddPower(Powers power) {
		EnabledPowers.Add(power);
	}
}

