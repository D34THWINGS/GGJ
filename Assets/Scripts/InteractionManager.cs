using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

	private CharacterControl character;

	void Start(){
		var player = GameObject.Find("Player");
		if(player != null){
			character = player.GetComponent<CharacterControl>();
		}
	}

	void OnTriggerEnter2D(Collider2D collision){
		InteractibleObject IO =  collision.GetComponent<InteractibleObject>();
		if(IO != null){
			IO.Interact(character);
		}
	}

	void OnTriggerExit2D(Collider2D collision){
		InteractibleObject IO =  collision.GetComponent<InteractibleObject>();
		if(IO != null){
			IO.Unteract(character);
		}
	}
}
