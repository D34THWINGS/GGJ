using UnityEngine;
using System.Collections;

public class SelectChestTreasure : MonoBehaviour {

	public bool canTransformPoint;
	public bool canHeavy;
	public bool canInvisible;
	
	void OnCollisionEnter2D(Collision2D colision){
		if(colision.gameObject.name == "Player"){
			if(canTransformPoint){
				PermanentVar.CanTransformRond = true;
			}else if(canHeavy){
				PermanentVar.CanHeavy = true;
			}else if(canInvisible){
				PermanentVar.CanInvisible = true;
			}
			AudioSource[] audios = colision.gameObject.GetComponents<AudioSource>();
			audios[2].Play();
		}
		Destroy(gameObject);
	}
}
