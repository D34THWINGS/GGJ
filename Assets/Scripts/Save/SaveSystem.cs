using UnityEngine;
using System.Collections;

namespace XRay.Save {

	public static class SaveSystem {

		public static void SaveLevel(int level) {
			BestLevel = level;
			CurrentLevel = level;
		}
		
		public static int BestLevel {
			get {
				return PlayerPrefs.GetInt("best_level");
			}
			set {
				if(value > BestLevel || value == 1) {
					PlayerPrefs.SetInt("best_level", value);
				}
			}
		}
		
		public static int CurrentLevel {
			get {
				return PlayerPrefs.GetInt("current_level");
			}
			set {
				if(value > 0)
					PlayerPrefs.SetInt("current_level", value);
			}
		}

	}

}
