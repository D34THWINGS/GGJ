using UnityEngine;
using System.Collections;

public class Button : TriggeringMechanism {

	public EventNames OnPressEvent = EventNames.ENABLE;
	public EventNames OnReleaseEvent = EventNames.DISABLE;

	void OnCollisionEnter2D(Collision2D theCollision) {
		if(theCollision.gameObject.name == "Body") {
			Trigger(OnPressEvent);
		}
	}
	
	void OnCollisionExit2D(Collision2D theCollision) {
		if(theCollision.gameObject.name == "Body") {
			Trigger(OnReleaseEvent);
		}
	}
}
