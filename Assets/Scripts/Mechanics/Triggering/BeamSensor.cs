using UnityEngine;
using System.Collections;

namespace XRay.Mechanics.Triggering {

	public class BeamSensor : TriggeringMechanism {
		
		public EventNames BeamEnterEvent = EventNames.ENABLE;
		public EventNames BeamExitEvent = EventNames.DISABLE;
		
		private bool beamState = false;
		private bool lastBeamState = false;
		
		// Update is called once per frame
		void Update () {
			if (beamState != lastBeamState) {
				if (beamState) {
					Trigger(BeamEnterEvent, name);
				} else {
					Trigger(BeamExitEvent, name);
				}
			}
			lastBeamState = beamState;
			beamState = false;
		}
		
		public void ReceiveBeam () {
			beamState = true;
		}
	}
}