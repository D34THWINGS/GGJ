using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XRay.Save;

public class GenerateWorlds : MonoBehaviour {

	//Worlds
	public TextAsset WorldsData;
	public GameObject GridParent;
	public GameObject WorldPrefab;

	//Level
	public TextAsset LvlData;
	public GameObject LevelPrefab;

	private List<GameObject> WorldsPanel = new List<GameObject>();

	// Use this for initialization
	void Start () {

		//Worlds Generation
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
			WorldsPanel.Add(newWorldPanel);
		}
		GridParent.GetComponent<UIGrid>().Reposition();


		//Level Generation
		delimiters = new char[] {'\n'};
		string[] LevelArray = LvlData.text.Split(delimiters);
		Array.Reverse(LevelArray);
		foreach ( string Line in LevelArray) {
			delimiters = new char[] {';'};
			string[] LevelData = Line.Split(delimiters);
			GameObject WorldParent = WorldsPanel[(WorldsPanel.Count-1)-(Int32.Parse(LevelData[2])-1)];
			GameObject newWorldLevel = (GameObject) GameObject.Instantiate(LevelPrefab);
			newWorldLevel.transform.parent = WorldParent.GetComponentInChildren<UIGrid>().gameObject.transform;
			newWorldLevel.name = "LevelButton"+LevelData[0];
			newWorldLevel.GetComponentInChildren<UILabel>().text = LevelData[1];
			newWorldLevel.transform.localScale = new Vector3(1,1,1);
			newWorldLevel.transform.localPosition = new Vector3(0,0,0);
			if(SaveSystem.BestLevel < Int32.Parse(LevelData[0])){
				newWorldLevel.GetComponent<BoxCollider>().enabled = false;
				newWorldLevel.GetComponentInChildren<UILabel>().text = "LOCKED";
				newWorldLevel.GetComponentInChildren<UISprite>().color = new Color(47f/255f, 43f/255f, 43f/255f);
			}
			WorldParent.GetComponentInChildren<UIGrid>().Reposition();
		}
		GridParent.GetComponent<UICenterOnChild>().Recenter();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
