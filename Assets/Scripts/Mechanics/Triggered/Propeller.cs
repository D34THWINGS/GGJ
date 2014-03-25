using UnityEngine;
using System.Collections;

namespace XRay.Mechanics.Triggered {

	public class Propeller : TriggerListener {
		
		public bool Enabled = true;
		public float Speed = 1.0f;
		public bool UseLimits = false;
		public float LowerAngle = 0f;
		public float UpperAngle = 0f;
		
		public GameObject Blades;
		
		// Use this for initialization
		protected override void Start () {
			base.Start();
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			if (Enabled) {
				var angle = Blades.transform.rotation.eulerAngles.z + Speed;
				if (UseLimits && (Speed > 0 && angle > UpperAngle || Speed < 0 && angle < UpperAngle)) return;
				transform.Rotate(0f,0f, Speed);
			}
		}
		
		protected override void TriggerAction (TriggeringMechanism.EventNames eventName)
		{		
			switch(eventName) {
			case TriggeringMechanism.EventNames.ENABLE:
				Enabled = true;
				break;
			case TriggeringMechanism.EventNames.DISABLE:
				Enabled = false;
				break;
			default:
				break;
			}
		}
	}
}
