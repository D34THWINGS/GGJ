using UnityEngine;
using System.Collections;

public class Elevator : TriggerListener {
	public float Speed = 1f;
	public Transform Origin;
	public Transform Target;
	public bool Started = false;

	protected override void Start() {
		base.Start();
		transform.position = Origin.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Started) {
			Vector2 move;
			move = Target.position - transform.position;
			move.Normalize();
			rigidbody2D.velocity = move * Speed;

			if (Vector2.Distance(transform.position, Target.position) < 0.01f) {
				Revert();
			}
		}
	}

	public void Revert() {
		var target = Target;
		Target = Origin;
		Origin = target;
	}

	protected override void TriggerAction (TriggeringMechanism.EventNames eventName)
	{
		switch(eventName) {
			case TriggeringMechanism.EventNames.ENABLE:
				Started = true;
				break;
			case TriggeringMechanism.EventNames.DISABLE:
				Started = false;
				break;
			default:
				break;
		}
	}
}
