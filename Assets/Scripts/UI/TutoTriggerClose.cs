using UnityEngine;
using System.Collections;

public class TutoTriggerClose : MonoBehaviour {

	public void OnMouseDown(){
		gameObject.transform.parent.gameObject.SetActive(false);
		XRay.UI.StaticVariables.isOnTuto = false;
	}
}
