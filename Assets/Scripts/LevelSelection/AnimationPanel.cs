using UnityEngine;
using System.Collections;


public class AnimationPanel : MonoBehaviour {

	public static bool isMenu = true;
	public GameObject PanelMenu;
	public GameObject PanelSelect;
	public UIScrollBar scrollBar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		if(isMenu){
			Hashtable hash = iTween.Hash("x",-5f,"time", 0.5f, "easetype", "easeInBack");
			iTween.MoveTo(PanelMenu, hash);
			hash = iTween.Hash("x",0,"time", 0.5f, "easetype", "easeOutBack", "delay", 0.5f, "oncomplete", "SetBoolFalse", "oncompletetarget", gameObject);
			iTween.MoveTo(PanelSelect, hash);
		}else{
			//scrollBar.scrollValue = 0;
			Hashtable hash = iTween.Hash("x",5f,"time", 0.5f, "easetype", "easeInBack");
			iTween.MoveTo(PanelSelect, hash);
			hash = iTween.Hash("x",0,"time", 0.5f, "easetype", "easeOutBack", "delay", 0.5f, "oncomplete", "SetBoolTrue", "oncompletetarget", gameObject);
			iTween.MoveTo(PanelMenu, hash);
		}
	}

	void SetBoolFalse(){
		isMenu = false;
	}

	void SetBoolTrue(){
		isMenu = true;
	}
}
