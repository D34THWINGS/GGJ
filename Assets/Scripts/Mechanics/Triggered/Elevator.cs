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

		private float elapsed;
		private bool freeze;
		private bool checkContact;
		private bool reverted;
		private Transform _target;
		private LineRenderer path;
		
		protected override void Start() {
			base.Start();
			transform.position = Origin.position;
			freeze = false;
			timeStop = timeStop * 1000;
			checkContact = false;
			_target = Target;
			reverted = false;
			path = gameObject.GetComponentInChildren<LineRenderer>();
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			showPath();
			if(Started && !freeze) {
				ChangeLimit(true);
				UpdatePosition();

				if (Vector2.Distance(transform.position, _target.position) < 0.01f) {
					StartCoroutine(ElevatorWait(timeStop));
				}
			} else {
				ChangeLimit(false);
			}
		}

		public void UpdatePosition () {
			var a = -(Mathf.Cos(elapsed * Speed) / 2) + 0.5f;
			transform.position = new Vector2(
				Origin.position.x + a * (Target.position.x - Origin.position.x),
				Origin.position.y + a * (Target.position.y - Origin.position.y));
			elapsed += Time.deltaTime;
		}
		
		public void Revert() {
			print ("Revert!");
			if (reverted)
				_target = Target;
			else
				_target = Origin;
			reverted = !reverted;
		}
		
		protected override void TriggerAction (bool trigger)
		{
			if(trigger) {
				Started = true;
			}
			else {
				Started = false;
			}
		}

		private IEnumerator ElevatorWait(float waitTime) {		
			// Wait for respawn time

			freeze = true;
			Revert ();
			yield return new WaitForSeconds(waitTime / 1000);
			freeze = false;
		}

		private void ChangeLimit (bool check) {
			GameObject.Find("LimitLeft").collider2D.enabled = check;
			GameObject.Find("LimitRight").collider2D.enabled = check;
		}

		private void showPath() {
			var z = new Vector3(0,0,3f);
			path.SetPosition(0, Origin.position + z);
			var dir = Target.position - Origin.position;
			var ray = Physics2D.Raycast(Origin.position, dir);
			path.SetPosition(1, Origin.position + dir + z);
		}
	}
}