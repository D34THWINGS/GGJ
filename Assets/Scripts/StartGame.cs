using UnityEngine;
using System.Collections;

namespace XRay {

	public class StartGame : MonoBehaviour {
		
		// Use this for initialization
		void Start () {
			if(!PlayerPrefs.HasKey("best_level")){
				PlayerPrefs.SetInt("best_level", 1);
			}
			if(!PlayerPrefs.HasKey("current_level")){
				PlayerPrefs.SetInt("current_level", 0);
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void OnMouseUp () {
			if(gameObject.name == "Start"){
				Application.LoadLevel(1);
				PlayerPrefs.SetInt("current_level", 1);
			}else if(gameObject.name == "Demo")
				Application.LoadLevel(7);
			else
				print ("Credits");
		}
	}

}