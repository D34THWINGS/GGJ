using UnityEngine;
using System.Collections;

public class HeavyInterrupt : MonoBehaviour {

	public GameObject HeavyObject;
	public GameObject OpenGameObject;
	public float Timer = 0;

	private float time = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(HeavyObject.rigidbody2D.mass >= 1000){
			OpenGameObject.GetComponent<MeshRenderer>().enabled = false;
			OpenGameObject.GetComponent<BoxCollider2D>().enabled = false;

			if(Timer != 0){
				time += Time.deltaTime;
				if(time > Timer){
					time = 0;
					OpenGameObject.GetComponent<MeshRenderer>().enabled = true;
					OpenGameObject.GetComponent<BoxCollider2D>().enabled = true;
					HeavyObject.rigidbody2D.mass = 200;
				}
			}
		}
	}
}
