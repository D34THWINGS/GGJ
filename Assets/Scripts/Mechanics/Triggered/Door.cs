using UnityEngine;
using System.Collections;

public class Door : TriggerListener {

	public Transform Target;
	[HideInInspector]
	public Transform Origin;
	public float Speed = 2f;
	public bool IsOpened = false;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		Origin = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsOpened) {
			transform.position = Vector2.Lerp(transform.position, Origin.position, Time.deltaTime * Speed);
			transform.rotation = Quaternion.Lerp(transform.rotation, Origin.rotation, Time.deltaTime * Speed);
		} else {			
			transform.position = Vector2.Lerp(transform.position, Target.position, Time.deltaTime * Speed);
			transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, Time.deltaTime * Speed);
		}
	}

	protected override void TriggerAction (TriggeringMechanism.EventNames eventName)
	{
		print("triggered");
		switch(eventName) {
		case TriggeringMechanism.EventNames.ENABLE:
			IsOpened = true;
			break;
		case TriggeringMechanism.EventNames.DISABLE:
			IsOpened = false;
			break;
		default:
			break;
		}
	}
}
