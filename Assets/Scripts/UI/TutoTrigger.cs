using UnityEngine;

namespace XRay.UI {
	public class TutoTrigger : MonoBehaviour {
		public GameObject TutoPanel;

<<<<<<< HEAD
		public void OnTriggerEnter2D(Collider2D collider){

			string[] passTutoList = XRay.Save.SaveSystem.PassTuto.Split(';');
			bool test = false;
			foreach(string passTutoString in passTutoList){
				if(gameObject.name.Replace("TutoTrigger","") == passTutoString){
					test = true;
				}
			}

			if(collider.name == "Player" && !test){
				StaticVariables.isOnTuto = true;
				TutoPanel.SetActive(true);
				this.gameObject.SetActive(false);
			}
=======
		public void OnTriggerEnter2D(Collider2D col){
		    if (col.name != "Player") return;
		    StaticVariables.IsOnTuto = true;
		    TutoPanel.SetActive(true);
>>>>>>> 3c6c8862a455f349daac0154ce7fb14703086d20
		}
	}
}
