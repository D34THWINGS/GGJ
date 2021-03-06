﻿using UnityEngine;
using System.Collections;
using XRay.Save;

namespace XRay {

	public class ChangeLevel : MonoBehaviour {
		public int NextLevel = 0;
		
		public void OnTriggerEnter2D (Collider2D collider) {
			if (collider.gameObject.name != "Player") return;
			SaveSystem.SaveLevel(NextLevel);
			Application.LoadLevel(NextLevel);
		}
	}
}
