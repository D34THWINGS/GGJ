using UnityEngine;
using System;
using System.Collections;

public class GenerateWorlds : MonoBehaviour {

	public TextAsset WorldsData;
	public GameObject GridParent;
	public GameObject WorldPrefab;

	// Use this for initialization
	void Start () {
		char[] delimiters = new char[] {'\n'};
		string[] WorldsArray = WorldsData.text.Split(delimiters);
		Array.Reverse(WorldsArray);
		foreach ( string Line in WorldsArray) {
			delimiters = new char[] {';'};
			string[] WorldData = Line.Split(delimiters);
			GameObject newWorldPanel = (GameObject) GameObject.Instantiate(WorldPrefab);
			newWorldPanel.transform.parent = GridParent.transform;
			newWorldPanel.name = "WorldPanel"+WorldData[0];
			newWorldPanel.GetComponentInChildren<UILabel>().text = WorldData[1];
			newWorldPanel.transform.localScale = new Vector3(1,1,1);
			newWorldPanel.transform.localPosition = new Vector3(0,0,0);
		}
		GridParent.GetComponent<UIGrid>().Reposition();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
