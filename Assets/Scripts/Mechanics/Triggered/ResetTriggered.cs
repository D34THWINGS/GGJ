using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ResetTriggered : MonoBehaviour {

	public List<GameObject> ObjectToResetPostion;
	private List<Vector3> ObjectOriginPosition;
	
	// Use this for initialization
	void Start () {
		ObjectOriginPosition = new List<Vector3>();
		foreach(GameObject g in ObjectToResetPostion){
			ObjectOriginPosition.Add(g.transform.localPosition);
		}
	}
	

	public void Reset(){
		for(int i=0; i<ObjectToResetPostion.Count;i++){
			ObjectToResetPostion[i].transform.localPosition = ObjectOriginPosition[i];
		}
	}
}
