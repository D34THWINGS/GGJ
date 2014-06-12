using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraEventTrigger : MonoBehaviour {


	public List<GameObject> Path;
	public GameObject CameraObject;

	private bool startReset = false;
	private float startResetTimer = 0;
	private float timetemp = 0;

	// Use this for initialization
	void Start () {
		//Path = new List<GameObject>();
	}

	public void Update(){
		if(startReset){
			timetemp += Time.deltaTime;
			if(timetemp >= startResetTimer+0.5f){
				XRay.UI.StaticVariables.CantMove = false;
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				startReset = false;
				startResetTimer = 0;
				timetemp = 0;
				CameraObject.GetComponent<XRay.Player.CameraFollow>().enabled = true;
			}
		}
	}
	
	public void OnTriggerEnter2D(Collider2D collider){	
		if(collider.name == "Player"){
			float delay = 0;
			if(Path.Count > 0){
				XRay.UI.StaticVariables.CantMove = true;
				CameraObject.GetComponent<XRay.Player.CameraFollow>().enabled = false;
				foreach(GameObject pp in Path){
					CameraEventPathPoint cepp = pp.GetComponent<CameraEventPathPoint>();
					if(cepp != null){
						Hashtable hash = iTween.Hash("position", cepp.transform.localPosition,"time", cepp.speed, "delay", delay);
						iTween.MoveTo(CameraObject, hash);
						delay += cepp.waitTime +0.3f;
					}
				}
				startResetTimer = delay;
				startReset = true;
			}else{
				new UnityException("Your Path is Empty. Create your path points and drag them in the editor");
			}
		}
	}
}
