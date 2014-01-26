using UnityEngine;
using System.Collections;

public class HeavyInterrupt : MonoBehaviour {

	public delegate void DelegateHeavy(bool value);
	public event DelegateHeavy OnPressureSwitch;
	public bool IsPressed { get; private set; }

	public GameObject HeavyObject;
	public GameObject OpenGameObject;
	public float Timer = 0;

	private float time = 0;

	// Use this for initialization
	void Start () {
		HeavyObject.GetComponent<InteractibleObject>().OnWeightChange += (sender) => {
			if (sender.rigidbody2D.mass >= PermanentVar.WeightHeavy) {
				IsPressed = true;
			} else { 
				IsPressed = false;
			}
			if (OnPressureSwitch != null) {
				OnPressureSwitch(IsPressed);
			}
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
