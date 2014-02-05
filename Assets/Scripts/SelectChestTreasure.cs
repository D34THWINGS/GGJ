using UnityEngine;
using System.Collections;

public class SelectChestTreasure : MonoBehaviour {

	public StaticVariables.Powers GrantedPower;
	public bool Enabled = true;
	
	public void OnTriggerEnter2D (Collider2D collider) {
		if(!Enabled || collider.gameObject.name != "Player" || StaticVariables.HasPower(GrantedPower)) return;
		StaticVariables.AddPower(GrantedPower);
		AudioSource[] audios = collider.gameObject.GetComponents<AudioSource>();
		audios[2].Play();
		gameObject.GetComponent<Animator>().SetBool("Opened", true);
	}
}
