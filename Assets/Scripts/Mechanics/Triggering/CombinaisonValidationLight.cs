using UnityEngine;
using System.Collections;

public class CombinaisonValidationLight : MonoBehaviour {

	public bool isOn = false;

	public void ChangeLight(){
		if(this.isOn){
			Hashtable hash = iTween.Hash("color", Color.grey, "time", 1);
			iTween.ColorTo(gameObject, hash);
			this.isOn = false;
		}else{
			Hashtable hash = iTween.Hash("color", Color.green, "time", 1);
			iTween.ColorTo(gameObject, hash);
			this.isOn = true;
		}
	}
}
