using UnityEngine;
using System.Collections;

public class TranformInterface : MonoBehaviour {
	public Texture BtnShape;
	public Texture BtnWeight;
	public Texture BtnSquare;
	public Texture BtnCircle;
	public Texture BtnTriangle;
	public Texture BtnLight;
	public Texture BtnHeavy;

	private enum ButtonType { NOKEY, SHAPE, WEIGHT };
	private ButtonType _activatedBtn = ButtonType.NOKEY;
	private GameObject _player;

	// Use this for initialization
	void Start () {
		if(!BtnShape || !BtnWeight){
			Debug.LogError("Assign a Texture in the inspector.");
		}

		var player = GameObject.Find("Player");
		if (player != null) {
			_player = player;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4)) {
			_activatedBtn = ButtonType.SHAPE;
		}
		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5)) {
			_activatedBtn = ButtonType.WEIGHT;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.JoystickButton2)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_player.GetComponent<Reshape>().CurrentShape = 0;
				_activatedBtn = ButtonType.NOKEY;
			}
			if (_activatedBtn == ButtonType.WEIGHT) {
				_player.GetComponent<CharacterControl>().JumpSpeed = 18;
				_activatedBtn = ButtonType.NOKEY;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.JoystickButton3)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_player.GetComponent<Reshape>().CurrentShape = 1;
				_activatedBtn = ButtonType.NOKEY;
			}
			if (_activatedBtn == ButtonType.WEIGHT) {
				_player.GetComponent<CharacterControl>().JumpSpeed = 12;
				_activatedBtn = ButtonType.NOKEY;
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
			if (_activatedBtn == ButtonType.SHAPE) {
				_player.GetComponent<Reshape>().CurrentShape = 2;
				_activatedBtn = ButtonType.NOKEY;
			}
			if (_activatedBtn == ButtonType.WEIGHT) {
				
			}
		}
	}
	
	void OnGUI () {
		GUI.DrawTexture(new Rect(Screen.width / 2 - 150, Screen.height - 60, 50, 50), BtnShape);
		GUI.DrawTexture(new Rect(Screen.width / 2 + 150, Screen.height - 60, 50, 50), BtnWeight);

		if (_activatedBtn == ButtonType.SHAPE) {
			GUI.DrawTexture(new Rect(Screen.width / 2 - 220, Screen.height - 110, 50, 50), BtnSquare);
			GUI.DrawTexture(new Rect(Screen.width / 2 - 150, Screen.height - 130, 50, 50), BtnCircle);
			GUI.DrawTexture(new Rect(Screen.width / 2 - 80, Screen.height - 110, 50, 50), BtnTriangle);
		} else if (_activatedBtn == ButtonType.WEIGHT) {
			GUI.DrawTexture(new Rect(Screen.width / 2 + 200, Screen.height - 120, 50, 50), BtnSquare);
			GUI.DrawTexture(new Rect(Screen.width / 2 + 100, Screen.height - 120, 50, 50), BtnTriangle);
		}
	}
}
