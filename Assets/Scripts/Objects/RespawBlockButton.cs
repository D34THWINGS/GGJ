using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawBlockButton : MonoBehaviour {

	public List<GameObject> BlockToRespawn;

	private List<Vector3> OriginPosition;
	
	void Start(){
		OriginPosition = new List<Vector3>();
		foreach(var block in BlockToRespawn) {
			OriginPosition.Add(block.transform.position);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.name == "Player"){
			var count = 0;
			foreach(var block in BlockToRespawn) {
				block.transform.position = OriginPosition[count];
				print(OriginPosition[count]);
				count++;
			}
		}
	}
}
