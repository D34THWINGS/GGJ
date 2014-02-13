using UnityEngine;
using System;

public class RespawnTimer : MonoBehaviour {

	private CharacterControl playerControl;
	private TimeSpan dieTime;

	// Use this for initialization
	void Start () {
		playerControl = GameObject.Find("Player").GetComponent<CharacterControl>();
		playerControl.OnDie += () => {
			dieTime = DateTime.Now.TimeOfDay;
		};
	}
	
	// Update is called once per frame
	void Update () {
		if (playerControl.IsDead) {
			var respawnIn = playerControl.RespawnTime - (DateTime.Now.TimeOfDay - dieTime).TotalMilliseconds;
			guiText.text = "Respawn in : " + (Math.Round(respawnIn / 100) / 10) + "s";
		} else {
			guiText.text = "";
		}
	}
}
