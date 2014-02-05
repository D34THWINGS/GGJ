using UnityEngine;
using System.Collections;

public class BeamGenerator : TriggerListener {

	public bool Enabled = true;
	public Transform Origin;
	public Transform DirectionPoint;
	public LayerMask CollidesWith;
	public float MaxLength = 200f;
	public float ZIndex = 3f;

	private LineRenderer beam;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		beam = gameObject.GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		var z = new Vector3(0,0,ZIndex);
		beam.SetPosition(0, Origin.position + z);
		if (Enabled) {			
			var dir = DirectionPoint.position - Origin.position;
			var ray = Physics2D.Raycast(Origin.position, dir);
			if (ray.fraction == 0) {
				beam.SetPosition(1, Origin.position + dir * MaxLength + z);
			} else {
				beam.SetPosition(1, (Vector3)ray.point + z);
				if (ray.collider.name == "BeamSensorArea") {
					ray.collider.transform.parent.GetComponent<BeamSensor>().ReceiveBeam();
				}
			}
		} else {
			beam.SetPosition(1, DirectionPoint.position + z);
		}
	}

	protected override void TriggerAction (TriggeringMechanism.EventNames eventName)
	{
		switch(eventName) {
		case TriggeringMechanism.EventNames.ENABLE:
			Enabled = true;
			break;
		case TriggeringMechanism.EventNames.DISABLE:
			Enabled = false;
			break;
		default:
			break;
		}
	}
}
