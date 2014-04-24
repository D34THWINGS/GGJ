using UnityEngine;

namespace XRay.UI {
	public class TutoTrigger : MonoBehaviour {
		public GameObject TutoPanel;

		public void OnTriggerEnter2D(Collider2D col){
		    if (col.name != "Player") return;
		    StaticVariables.IsOnTuto = true;
		    TutoPanel.SetActive(true);
		}
	}
}
