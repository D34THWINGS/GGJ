using UnityEngine;
using System.Collections;
using XRay.Save;

namespace XRay {

	public class StartGame : MonoBehaviour {
		
		// Use this for initialization
		void Start () {
			if(!PlayerPrefs.HasKey("best_level")){
				SaveSystem.BestLevel = 1;
			}
			if(!PlayerPrefs.HasKey("current_level")){
				SaveSystem.CurrentLevel = 0;
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void OnMouseUp () {
			if(gameObject.name == "Start"){
				Application.LoadLevel(1);
				SaveSystem.CurrentLevel = 1;
			}else if(gameObject.name == "Demo")
				Application.LoadLevel(7);
			else
				print ("Credits");
		}
	}

}