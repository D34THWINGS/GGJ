using UnityEngine;
using System.Collections.Generic;

namespace XRay.Mechanics {

	public abstract class TriggerListener : MonoBehaviour {
		
		public List<TriggeringMechanism> Mechanics;
		public bool RevertedTrigger = false;
		
		protected virtual void Start() {
			foreach (var mech in Mechanics) {
				mech.OnTrigger += Trigger;
			}
		}

		private void Trigger (TriggeringMechanism.EventNames eventName) {
			var inverter = eventName;
			if (RevertedTrigger && eventName == TriggeringMechanism.EventNames.ENABLE){
				inverter = TriggeringMechanism.EventNames.DISABLE;
			}
			if (RevertedTrigger && eventName == TriggeringMechanism.EventNames.DISABLE) {
				inverter = TriggeringMechanism.EventNames.ENABLE;
			}
			TriggerAction(inverter);
		}

		protected abstract void TriggerAction (TriggeringMechanism.EventNames eventName);
	}
}
