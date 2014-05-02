using UnityEngine;
using System.Collections.Generic;

namespace XRay.Mechanics {

	public abstract class TriggerListener : MonoBehaviour {
		
		public List<TriggeringMechanism> Mechanics;
		public bool RevertedTrigger = false;
		
		private List<int> _nbOfValid;
		
		protected virtual void Start() {
			foreach (var mech in Mechanics) {
				mech.OnTrigger += Trigger;
			}
			_nbOfValid = new List<int>();
			for (int i = 0; i < Mechanics.Count; i++) {
				_nbOfValid.Add(0);
			}
		}

		private void Trigger (TriggeringMechanism.EventNames eventName, string name = "") {
			var inverter = eventName;
			int nb = 0;
			if(Mechanics.Count > 1){
				nb = int.Parse(name.Substring((name.Length - 1)));
			}
			if (RevertedTrigger && eventName == TriggeringMechanism.EventNames.ENABLE){
				inverter = TriggeringMechanism.EventNames.DISABLE;
				setValidTrigger(nb, 0);
			}
			else {
				setValidTrigger(nb, 1);
			}
			if (RevertedTrigger && eventName == TriggeringMechanism.EventNames.DISABLE) {
				inverter = TriggeringMechanism.EventNames.ENABLE;
				setValidTrigger(nb, 1);
			}
			else {
				setValidTrigger(nb, 0);
			}

			TriggerAction(checkValidTrigger());
		}

		protected abstract void TriggerAction (bool trigger);

		protected void setValidTrigger (int index, int value) {
			_nbOfValid[index] = value;
		}

		protected bool checkValidTrigger () {
			var count = 0;
			foreach(var valid in _nbOfValid) {
				if(valid == 1) {
					count++;
				}
				else {
					return false;
				}
			}
			if(count == Mechanics.Count) {
				return true;
			}
			else {
				return false;
			}
		}
	}
}
