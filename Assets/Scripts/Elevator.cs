using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	public float Speed = 0f;
	public float LimitDown = -3.5f;
	public float LimitUp = 10f;

	private bool _goesUp = true;
	
	// Update is called once per frame
	void Update () {
		var y = transform.position.y;
		if(y > LimitUp || y < LimitDown) {
			_goesUp = !_goesUp;
		}
		var move = new Vector2(0, (_goesUp ? Speed : -Speed));
		transform.Translate(move * Time.deltaTime);
	}
}
