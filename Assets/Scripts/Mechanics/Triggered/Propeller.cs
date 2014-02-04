using UnityEngine;
using System.Collections;

public class Propeller : TriggerListener {

	public bool Enabled = true;
	public float Speed = 1.0f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Enabled) {
			transform.Rotate(0f,0f, Speed);
		}
	}

	protected override void TriggerAction (TriggeringMechanism.EventNames eventName)
	{		
		switch(eventName) {
		case TriggeringMechanism.EventNames.ENABLE:
			Enabled = true;
			break;
		case TriggeringMechanism.EventNames.DISABLE:
			Enabled = false;
			break;
		default:
			break;
		}
	}
}
