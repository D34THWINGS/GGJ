using UnityEngine;
using System.Collections;
using XRay.Player;

namespace XRay.Mechanics.Triggered {

	public class Elevator : TriggerListener {
		public float Speed = 1f;
		public Transform Origin;
		public Transform Target;
		public bool Started = false;
		public float journeyLength = 3f;
		public int timeStop;

		private float startTime;
		private bool freez;
		private bool checkContact;
		
		protected override void Start() {
			base.Start();
			transform.position = Origin.position;
			startTime = Time.time;
			freez = false;
			timeStop = timeStop * 1000;
			checkContact = false;
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			if(Started && !freez) {				

				float distCovered = (Time.time - startTime) * Speed;
				float fracJourney = distCovered / journeyLength;
				transform.position = Vector3.Lerp(Origin.position, Target.position, fracJourney);

				if (transform.position == Target.position) {
					StartCoroutine(RevertWait(timeStop));
				}
			}
		}
		
		public void Revert() {
			startTime = Time.time;
			var target = Target;
			Target = Origin;
			Origin = target;
		}
		
		protected override void TriggerAction (TriggeringMechanism.EventNames eventName)
		{
			switch(eventName) {
			case TriggeringMechanism.EventNames.ENABLE:
				Started = true;
				break;
			case TriggeringMechanism.EventNames.DISABLE:
				Started = false;
				rigidbody2D.velocity = Vector2.zero;
				break;
			default:
				break;
			}
		}

		private IEnumerator RevertWait(float waitTime) {		
			// Wait for respawn time

			freez = true;
			GameObject.Find("Limit").collider2D.enabled = false;
			yield return new WaitForSeconds(waitTime / 1000);
			Revert();
			freez = false;
			GameObject.Find("Limit").collider2D.enabled = true;
		}

		public bool GetFreez () {
			return freez;
		}
	}
}