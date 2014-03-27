using UnityEngine;
using System.Collections;

namespace XRay {

	public class StartGame : MonoBehaviour {
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void OnMouseUp () {
			if(gameObject.name == "Start")
				Application.LoadLevel(1);
			else if(gameObject.name == "Demo")
				Application.LoadLevel(7);
			else
				print ("Credits");
		}
	}

}