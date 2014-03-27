using UnityEngine;
using System.Collections;

public class EnableCenterObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<UICenterOnChild>().Recenter();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
