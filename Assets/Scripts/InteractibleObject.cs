using UnityEngine;
using System.Collections;

public class InteractibleObject : MonoBehaviour {
	public enum InteractionType {
		PERMANENT,
		TIMED,
		INSTANT
	}
	public InteractionType Interaction;
	public float Timer;
	public bool isTransformable;
	public bool isInvisible;
	public bool isKillable;
	

	// Use this for initialization
	void Start () {
		if(isTransformable){

		}
		if(isInvisible){
			GetComponent<Animator>().SetBool("Hidden", true);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		if(isKillable){

		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Interact (CharacterControl character) {
		if(isTransformable){

		}
		if(isInvisible){
			if(Interaction == InteractionType.INSTANT){
				GetComponent<Animator>().SetBool("Hidden", false);
				gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			}
		}
		if(isKillable){

		}
	}

	public void Unteract(CharacterControl character){
		if(isTransformable){
			
		}
		if(isInvisible){
			if(Interaction == InteractionType.INSTANT){
				GetComponent<Animator>().SetBool("Hidden", true);
				gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			}
		}
		if(isKillable){
			
		}
	}
}
