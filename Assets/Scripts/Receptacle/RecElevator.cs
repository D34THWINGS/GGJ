using UnityEngine;
using System.Collections;

public class RecElevator : MonoBehaviour {
	public GameObject Elevator;

	void OnCollisionEnter2D(Collision2D theCollision) {
		print(theCollision.gameObject.name);
		if(theCollision.gameObject.name == "Support") {
			Elevator.GetComponent<Elevator>().Start = true;
		}
	}
}