using UnityEngine;
using System.Collections;

public class RecElevator : MonoBehaviour {
	public GameObject Elevator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D theCollision) {
		print(theCollision.gameObject.name);
		if(theCollision.gameObject.name == "Support") {
			Elevator.GetComponent<Elevator>().Start = true;
		}
	}
}