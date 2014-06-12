using UnityEngine;
using System.Collections;

namespace XRay.Mechanics.Triggered {

	public class RotationGround : TriggerListener {

		public bool isActive;
		public bool keepActive;
		public Transform Target;
		public float speed;

		public Transform Origin;

		// Use this for initialization
		void Start () {
			base.Start();
		}
		
		// Update is called once per frame
		void Update () {
			if(isActive){
				gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, Target.transform.position, Time.deltaTime * speed);
				gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Target.transform.rotation, Time.deltaTime * speed);
			}else{
				gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, Origin.position, Time.deltaTime * speed);
				gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Origin.rotation, Time.deltaTime * speed);
			}
		}

		protected override void TriggerAction (bool trigger)
		{
			if(trigger) {
				isActive = true;
			}
			else {
				if(!keepActive){
					isActive = false;
				}
			}
		}
	}
}
