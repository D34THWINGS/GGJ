using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XRay.Objects;

namespace XRay.Mechanics.Triggering {

	public class TriggerZone : TriggeringMechanism {

		public EventNames typeEvent;
		public bool OneUse = false;


		void OnTriggerEnter2D(Collider2D collider) {
			Trigger(typeEvent);
			if(OneUse){
				Destroy(gameObject);
			}
		}
	}
}
