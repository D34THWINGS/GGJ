using UnityEngine;
using System.Collections;

public class SelectChestTreasure : MonoBehaviour {

	public bool canTransformPoint;
	public bool canHeavy;
	public bool canInvisible;
	
	public void OnTriggerEnter2D (Collider2D collider) {
		if(collider.gameObject.name != "Player") return;
		if(canTransformPoint){
			PermanentVar.CanTransformRond = true;
		}else if(canHeavy){
			PermanentVar.CanHeavy = true;
		}else if(canInvisible){
			PermanentVar.CanInvisible = true;
		}
		AudioSource[] audios = collider.gameObject.GetComponents<AudioSource>();
		audios[2].Play();
		Destroy(gameObject);
	}
}
