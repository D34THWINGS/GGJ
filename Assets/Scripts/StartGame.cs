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
			if(SaveSystem.CurrentLevel == 0){
				GameObject.Find("Continue").collider.isTrigger = false;
			}
			if(!PlayerPrefs.HasKey("passTuto")){
				SaveSystem.PassTuto = "";
			}
		}
		
		// Update is called once per frame
		void Update () {

		}
		
		void OnClick () {
			if(gameObject.name == "NewGame"){
				print ("NewGame");
				SaveSystem.BestLevel = 1;
				SaveSystem.CurrentLevel = 1;
				SaveSystem.PassTuto = "";
				Application.LoadLevel(1);
			}else if(gameObject.name == "Continue"){
				print ("Continue");
				Application.LoadLevel(SaveSystem.CurrentLevel);
			}else if(gameObject.name == "Exit"){
				print ("Exit");
				Application.Quit();
			}
		}
	}

}