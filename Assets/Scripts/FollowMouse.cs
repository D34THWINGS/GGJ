using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	public GameObject Player;

	private bool _usingController = false;
	private Vector3 _lastMousePos;
	private float _lastJoyX = 0f;
	private float _lastJoyY = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Player.transform.position;
		if(Input.GetAxis("Joy X") != 0 || Input.GetAxis("Joy Y") != 0) {
			_usingController = true;
		}
		if(_lastMousePos != Input.mousePosition){
			_usingController = false;
		}
		_lastMousePos = Input.mousePosition;

		if(!_usingController) {
			var mouse_pos = Input.mousePosition;
			var object_pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			mouse_pos.x = mouse_pos.x - object_pos.x;
			mouse_pos.y = mouse_pos.y - object_pos.y;
			float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		}else {
			if(Input.GetAxis("Joy X") != 0f)
				_lastJoyX = Input.GetAxis("Joy X");
			if(Input.GetAxis("Joy Y") != 0f)
				_lastJoyY = Input.GetAxis("Joy Y");
			float angle = Mathf.Atan2(-_lastJoyY, _lastJoyX) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		}
	}
}
