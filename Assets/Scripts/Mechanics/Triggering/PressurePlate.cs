using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	public delegate void DelegatePressure(bool value);
	public event DelegatePressure OnToggle;
	public bool IsPressed { get; private set; }

	public GameObject HeavyObject;

	// Use this for initialization
	void Start () {
		HeavyObject.GetComponent<InteractibleObject>().OnWeightChange += (sender) => {
			if (sender.rigidbody2D.mass >= PermanentVar.WeightHeavy) {
				IsPressed = true;
			} else { 
				IsPressed = false;
			}
			if (OnToggle != null) {
				OnToggle(IsPressed);
			}
		};
	}
}
