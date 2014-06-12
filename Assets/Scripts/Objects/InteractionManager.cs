using UnityEngine;

namespace XRay.Objects {

	public class InteractionManager : MonoBehaviour {
		
		public GameObject Player;
		
		public void Start(){

		}
		
		public void OnTriggerEnter2D(Collider2D collision){
			if (collision.gameObject.name == "Player") return;
			var io =  collision.GetComponent<InteractibleObject>();
			if(io != null){
				io.Interact(Player);
			}
		}
		
		public void OnTriggerExit2D(Collider2D collision){
			if (collision.gameObject.name == "Player") return;
			var io =  collision.GetComponent<InteractibleObject>();
			if(io != null){
				io.Unteract();
			}
		}	
	}
}
