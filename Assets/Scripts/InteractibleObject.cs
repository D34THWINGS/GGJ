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

	public PhysicsMaterial2D Glass;
	
	private Reshape _reshape;
	
	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetBool("Hidden", false);
		gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		Debug.Log(isTransformable);
		if(isTransformable){
			_reshape = gameObject.GetComponent<Reshape>();
			if(_reshape == null){
				Debug.LogError("No Reshape Component");
			}
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
	
	public void Interact (GameObject player) {
		if(isTransformable){
			var resh = player.GetComponent<Reshape>();
			if(Interaction == InteractionType.INSTANT){
				_reshape.CurrentShape = resh.CurrentShape;
				collider2D.sharedMaterial = Glass;
			}
			if(Interaction == InteractionType.PERMANENT){
				_reshape.CurrentShape = resh.CurrentShape;
				collider2D.sharedMaterial = Glass;
			}
			if(Interaction == InteractionType.TIMED){
				
			}
		}
		if(isInvisible){
			GetComponent<Animator>().SetBool("Hidden", false);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		}
		if(isKillable){
			
		}
	}
	
	public void Unteract(GameObject player){
		if(isTransformable){
			if(Interaction == InteractionType.INSTANT){
				_reshape.CurrentShape = _reshape.OriginShape;
				collider2D.sharedMaterial = Glass;
			}
			if(Interaction == InteractionType.TIMED){
				
			}
		}
		if(isInvisible){
			GetComponent<Animator>().SetBool("Hidden", true);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		if(isKillable){
			
		}
	}
}
