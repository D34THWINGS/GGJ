using UnityEngine;
using System.Collections;

public class RespawnableInteractiveObject : MonoBehaviour {

	private Vector3 OriginPosition;

	void Start(){
		OriginPosition = gameObject.transform.localPosition;
	}

	void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.GetComponent<XRay.RespawnSystem>() != null){
			gameObject.transform.localPosition = OriginPosition;
		}
	}


}
