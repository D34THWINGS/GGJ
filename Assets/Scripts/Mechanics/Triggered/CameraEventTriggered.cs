using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XRay.Mechanics.Triggered {

	public class CameraEventTriggered :  TriggerListener{

		public List<GameObject> Path;
		public GameObject CameraObject;
		
		private bool startReset = false;
		private float startResetTimer = 0;
		private float timetemp = 0;

		private bool CameraEventDone = false;
		
		// Use this for initialization
		protected override void Start () {
			base.Start();
		}
		
		void Update(){
			if(startReset){
				timetemp += Time.deltaTime;
				if(timetemp >= startResetTimer+0.5f){
					CameraEventDone = true;
					XRay.UI.StaticVariables.CantMove = false;
					startReset = false;
					startResetTimer = 0;
					timetemp = 0;
					CameraObject.GetComponent<XRay.Player.CameraFollow>().enabled = true;
				}
			}
		}

		protected override void TriggerAction (bool trigger)
		{
			if(trigger) {
				if(!CameraEventDone){
					float delay = 0;
					if(Path.Count > 0){
						XRay.UI.StaticVariables.CantMove = true;
						CameraObject.GetComponent<XRay.Player.CameraFollow>().enabled = false;
						foreach(GameObject pp in Path){
							CameraEventPathPoint cepp = pp.GetComponent<CameraEventPathPoint>();
							if(cepp != null){
								Hashtable hash = iTween.Hash("position", cepp.transform.localPosition,"time", cepp.speed, "delay", delay);
								iTween.MoveTo(CameraObject, hash);
								delay += cepp.waitTime +0.3f;
							}
						}
						startResetTimer = delay;
						startReset = true;
					}else{
						new UnityException("Your Path is Empty. Create your path points and drag them in the editor");
					}
				}
			}
		}
	}
}