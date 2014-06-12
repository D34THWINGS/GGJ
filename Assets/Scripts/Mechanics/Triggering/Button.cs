using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XRay.Mechanics.Triggering {

	public class Button : TriggeringMechanism {
		
		public EventNames OnPressEvent = EventNames.ENABLE;
		public EventNames OnReleaseEvent = EventNames.DISABLE;

		public List<GameObject> BlockToRespawn;
		public TriggerListener ResetTriggerObject;
		
		private List<Vector3> OriginPosition;

		void Start(){
			OriginPosition = new List<Vector3>();
			foreach(var block in BlockToRespawn) {
				OriginPosition.Add(block.transform.position);
			}
		}
		
		void OnCollisionEnter2D(Collision2D theCollision) {
			if(theCollision.gameObject.name == "ButtonTrigger") {
				Trigger(OnPressEvent, name);
			}
			if(theCollision.gameObject.name == "Player" && BlockToRespawn.Count > 0){
				for(var i = 0; i < BlockToRespawn.Count; i++) {
					BlockToRespawn[i].transform.position = OriginPosition[i];
				}
				ResetTriggerObject.resetTrigger();
			}
		}
		
		void OnCollisionExit2D(Collision2D theCollision) {
			if(theCollision.gameObject.name == "ButtonTrigger") {
				Trigger(OnReleaseEvent, name);
			}
		}
	}

}