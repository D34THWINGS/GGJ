using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {
	public int NextLevel = 0;

	public void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.name != "Player") return;
		Application.LoadLevel(NextLevel);
	}
}
