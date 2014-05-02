using UnityEngine;
using System.Collections;

namespace XRay.Mechanics.Triggering {

	public class Button : TriggeringMechanism {
		
		public EventNames OnPressEvent = EventNames.ENABLE;
		public EventNames OnReleaseEvent = EventNames.DISABLE;
		
		void OnCollisionEnter2D(Collision2D theCollision) {
			if(theCollision.gameObject.name == "ButtonTrigger") {
				Trigger(OnPressEvent, name);
			}
		}
		
		void OnCollisionExit2D(Collision2D theCollision) {
			if(theCollision.gameObject.name == "ButtonTrigger") {
				Trigger(OnReleaseEvent, name);
			}
		}
	}

}