using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour {
	public int BestLevel;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey("best_level")){
			BestLevel = PlayerPrefs.GetInt("best_level");
			print (BestLevel);
		}else {
			PlayerPrefs.SetInt("best_level", 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
