using UnityEngine;
using System.Collections;

public class PressurePlate : TriggeringMechanism {

	public delegate void DelegatePressure(bool value);
	public event DelegatePressure OnToggle;
	public bool IsPressed { get; private set; }

	public GameObject HeavyObject;

	// Use this for initialization
	void Start () {
		HeavyObject.GetComponent<InteractibleObject>().OnStateChange += (iEvent, sender) => {
			if (iEvent != InteractibleObject.InteractionEvent.WEIGHT_CHANGE) return;
			if (sender.rigidbody2D.mass >= StaticVariables.HeavyWeight) {
				IsPressed = true;
			} else { 
				IsPressed = false;
			}
			if (OnToggle != null) {
				OnToggle(IsPressed);
			}
		};
	}

	void OnCollisionStay2D (Collision2D collision) {
		print (collision.rigidbody.velocity);
	}
}
