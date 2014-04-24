using UnityEngine;
using XRay.UI;

namespace XRay.Player {

	public class FollowMouse : MonoBehaviour {
		
		private GameObject _player;
		private bool _usingController;
		private Vector3 _lastMousePos;
		private float _lastJoyX;
		private float _lastJoyY;
		
		// Use this for initialization
		public void Start () {
			_player = GameObject.Find("Player");
		}
		
		// Update is called once per frame
		public void Update () {
		    if (StaticVariables.IsOnTuto) return;
		    transform.position = _player.transform.position;
		    if(!Input.GetAxis("Joy X").Equals(0f) || !Input.GetAxis("Joy Y").Equals(0f)) {
		        _usingController = true;
		    }
		    if(_lastMousePos != Input.mousePosition){
		        _usingController = false;
		    }
		    _lastMousePos = Input.mousePosition;
				
		    if(!_usingController) {
		        var mousePos = Input.mousePosition;
		        var objectPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		        mousePos.x = mousePos.x - objectPos.x;
		        mousePos.y = mousePos.y - objectPos.y;
		        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		    }else {
		        if(!Input.GetAxis("Joy X").Equals(0f))
		            _lastJoyX = Input.GetAxis("Joy X");
		        if(!Input.GetAxis("Joy Y").Equals(0f))
		            _lastJoyY = Input.GetAxis("Joy Y");
		        var angle = Mathf.Atan2(-_lastJoyY, _lastJoyX) * Mathf.Rad2Deg;
		        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		    }
		}
	}
}
