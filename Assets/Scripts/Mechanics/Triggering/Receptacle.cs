using UnityEngine;
using System.Collections;

namespace XRay.Mechanics.Triggering {

	public class Receptacle : TriggeringMechanism {
		void OnCollisionEnter2D(Collision2D theCollision) {
			if(theCollision.gameObject.name == "Support") {
				Trigger(EventNames.ENABLE,name);
			}
		}
		
		void OnCollisionExit2D(Collision2D theCollision) {
			if(theCollision.gameObject.name == "Support") {
				Trigger(EventNames.DISABLE,name);
			}
		}
	}
}