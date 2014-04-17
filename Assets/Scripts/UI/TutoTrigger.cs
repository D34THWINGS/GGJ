using UnityEngine;
using System.Collections;

namespace XRay.UI {
	public class TutoTrigger : MonoBehaviour {
		public GameObject TutoPanel;

		public void OnTriggerEnter2D(Collider2D collider){
			if(collider.name == "Player"){
				StaticVariables.isOnTuto = true;
				TutoPanel.SetActive(true);
			}
		}
	}
}
