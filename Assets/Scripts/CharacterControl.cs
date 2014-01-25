using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {
	public Reshape Reshaper;

	void Start () {
		Reshaper = GetComponent<Reshape>();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.A) && Reshaper.CurrentShape != 1) {
			Reshaper.PreviousShape = Reshaper.CurrentShape;
			Reshaper.CurrentShape = 1;
			print ("To Circle !");
		}
		if (Input.GetKeyDown(KeyCode.E) && Reshaper.CurrentShape != 0) {
			Reshaper.PreviousShape = Reshaper.CurrentShape;
			Reshaper.CurrentShape = 0;
			print ("To Square !");
		}
	}
}
