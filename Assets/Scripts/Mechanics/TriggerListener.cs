using UnityEngine;
using System.Collections.Generic;

public abstract class TriggerListener : MonoBehaviour {
	
	public List<TriggeringMechanism> Mechanics;
	
	protected virtual void Start() {
		print (Mechanics);
		foreach (var mech in Mechanics) {
			print(mech);
			mech.OnTrigger += TriggerAction;
		}
	}

	protected abstract void TriggerAction (TriggeringMechanism.EventNames eventName);
}

