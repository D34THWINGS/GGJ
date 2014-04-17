using UnityEngine;
using System.Collections;

public class TutoTriggerClose : MonoBehaviour {

	public void OnClick(){
		gameObject.transform.parent.gameObject.SetActive(false);
		XRay.UI.StaticVariables.isOnTuto = false;
	}
}
