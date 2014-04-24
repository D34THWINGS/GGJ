using UnityEngine;

namespace XRay.UI {
	public class TutoTrigger : MonoBehaviour {
		public GameObject TutoPanel;
		
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
		}
	}
}
