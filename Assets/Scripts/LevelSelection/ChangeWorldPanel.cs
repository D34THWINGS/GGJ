using UnityEngine;
using System.Collections;

public class ChangeWorldPanel : MonoBehaviour {

	public UIScrollBar scrollbar;
	public UICenterOnChild CenterOnCHild;
	private float scrollvaluetochangeperupdate = 0.0085f;
	private float numberOfSeconds = 0.5f;
	bool onMoving = false;
	float count = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(onMoving){
			if(this.tag == "left"){
				scrollbar.scrollValue -= scrollvaluetochangeperupdate;
			}else if(this.tag == "right"){
				scrollbar.scrollValue += scrollvaluetochangeperupdate;
			}
			count+= Time.deltaTime;
			if(count >= numberOfSeconds){
				count = 0;
				onMoving = false;
				CenterOnCHild.Recenter();
			}
		}
	}

	void OnClick(){
		if(!onMoving){
			onMoving = true;
		}
	}
}
