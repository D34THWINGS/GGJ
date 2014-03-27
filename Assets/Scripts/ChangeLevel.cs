using UnityEngine;
using System.Collections;

namespace XRay {

	public class ChangeLevel : MonoBehaviour {
		public int NextLevel = 0;
		
		public void OnTriggerEnter2D (Collider2D collider) {
			if (collider.gameObject.name != "Player") return;

			int best = PlayerPrefs.GetInt("best_level");
			int current = PlayerPrefs.GetInt("current_level");
			if(NextLevel > best) {
				PlayerPrefs.SetInt("best_level", NextLevel);
			}
			PlayerPrefs.SetInt("current_level", NextLevel);
			Application.LoadLevel(NextLevel);
		}
	}
}
