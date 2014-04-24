using UnityEngine;
using System.Collections;

public class TutoTriggerClose : MonoBehaviour {

	public void OnClick(){
		if(!PlayerPrefs.HasKey("passTuto")){
			XRay.Save.SaveSystem.PassTuto = "";
		}
		XRay.Save.SaveSystem.PassTuto += gameObject.transform.parent.gameObject.name.Replace("TutoPanel","")+";";
		gameObject.transform.parent.gameObject.SetActive(false);
		XRay.UI.StaticVariables.IsOnTuto = false;
	}
}
