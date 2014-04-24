using UnityEngine;
using System;
using System.Collections;

public class OnCLickLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		Application.LoadLevel(Int32.Parse(gameObject.name.Substring(gameObject.name.Length-1, 1)));
	}
}
