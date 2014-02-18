﻿using UnityEngine;
using System.Collections;

public class TranformInterface : MonoBehaviour {
	public Texture BtnShape;
	public Texture BtnWeight;
	public Texture BtnSquare;
	public Texture BtnCircle;
	public Texture BtnTriangle;
	public Texture BtnLight;
	public Texture BtnHeavy;

	public GameObject WeightMessage;

	private enum ButtonType { NOKEY, SHAPE, WEIGHT };
	private ButtonType _activatedBtn = ButtonType.NOKEY;
	private GameObject _player;
	private AudioSource[] audios;

	// Use this for initialization
	void Start () {
		if(!BtnShape || !BtnWeight){
			Debug.LogError("Assign a Texture in the inspector.");
		}

		var player = GameObject.Find("Player");
		if (player != null) {
			_player = player;
		}
		audios = gameObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_activatedBtn = ButtonType.NOKEY;
			} else {
				if(StaticVariables.HasPower(StaticVariables.Powers.RESHAPE_CIRCLE))
					_activatedBtn = ButtonType.SHAPE;
			}
		}
		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5)) {
			if (_activatedBtn == ButtonType.WEIGHT) {
				_activatedBtn = ButtonType.NOKEY;
			} else {
				if(StaticVariables.HasPower(StaticVariables.Powers.CHANGE_WEIGHT))
					_activatedBtn = ButtonType.WEIGHT;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.JoystickButton2)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_player.GetComponent<Reshape>().CurrentShape = 0;
				_activatedBtn = ButtonType.NOKEY;
				audios[0].Play();

			}
			if (_activatedBtn == ButtonType.WEIGHT) {
				_player.rigidbody2D.mass = StaticVariables.LightWeight;
				_activatedBtn = ButtonType.NOKEY;
				WeightMessage.GetComponent<TextMesh>().text = "Soft";
				Instantiate(WeightMessage, _player.transform.position, Quaternion.identity);
				audios[1].Play();
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.JoystickButton3)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_player.GetComponent<Reshape>().CurrentShape = 1;
				_activatedBtn = ButtonType.NOKEY;
				audios[0].Play();
			}
			if (_activatedBtn == ButtonType.WEIGHT) {
				_player.rigidbody2D.mass = StaticVariables.HeavyWeight;
				_activatedBtn = ButtonType.NOKEY;
				WeightMessage.GetComponent<TextMesh>().text = "Heavy";
				Instantiate(WeightMessage, _player.transform.position, Quaternion.identity);
				audios[2].Play();
			}
		}
		/*if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_player.GetComponent<Reshape>().CurrentShape = 2;
				_activatedBtn = ButtonType.NOKEY;
				audios[0].Play();
			}
			if (_activatedBtn == ButtonType.WEIGHT) {
				
			}
		}*/
	}
	
	void OnGUI () {
		if(StaticVariables.HasPower(StaticVariables.Powers.RESHAPE_CIRCLE)){
			GUI.DrawTexture(new Rect(Screen.width / 2 - 150, Screen.height - 60, 50, 50), BtnShape);
		}

		if(StaticVariables.HasPower(StaticVariables.Powers.CHANGE_WEIGHT)){
			GUI.DrawTexture(new Rect(Screen.width / 2 + 150, Screen.height - 60, 50, 50), BtnWeight);
		}

		if (_activatedBtn == ButtonType.SHAPE) {
			GUI.DrawTexture(new Rect(Screen.width / 2 - 200, Screen.height - 120, 50, 50), BtnSquare);//Screen.width / 2 - 220
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, Screen.height - 120, 50, 50), BtnCircle);//Screen.width / 2 - 150
			//GUI.DrawTexture(new Rect(Screen.width / 2 - 80, Screen.height - 110, 50, 50), BtnTriangle);
		} else if (_activatedBtn == ButtonType.WEIGHT) {
			GUI.DrawTexture(new Rect(Screen.width / 2 + 100, Screen.height - 120, 50, 50), BtnLight);
			GUI.DrawTexture(new Rect(Screen.width / 2 + 200, Screen.height - 120, 50, 50), BtnHeavy);
		}
	}
}
