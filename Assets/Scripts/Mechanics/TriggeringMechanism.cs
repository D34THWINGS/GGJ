using UnityEngine;
using System.Collections;

namespace XRay.Mechanics {

	public class TriggeringMechanism : MonoBehaviour {
		
		public enum EventNames {
			VOID_ACTION,
			ENABLE,
			DISABLE
		};
		
		public delegate void TriggerDelegate(EventNames eventName);
		public TriggerDelegate OnTrigger;
		
		protected void Trigger (EventNames eventName) {
			if (OnTrigger != null) {
				OnTrigger(eventName);
			}
		}
	}
}