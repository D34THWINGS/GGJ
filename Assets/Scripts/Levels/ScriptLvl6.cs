using UnityEngine;
using System.Collections;

public class ScriptLvl6 : MonoBehaviour {

	public GameObject PressurePlate;
	public GameObject Door;

	private bool _openDoor = false;

	// Use this for initialization
	void Start () {
		PressurePlate.GetComponent<PressurePlate>().OnToggle += (value) => {
			_openDoor = value;
		};
	}
	
	// Update is called once per frame
	void Update () {
		if (_openDoor) {
			Door.transform.position = Vector2.Lerp(Door.transform.position, new Vector2(56.5f, 16.0f), Time.deltaTime * 2.0f);
		}
	}
}
