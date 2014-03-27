using UnityEngine;
using System.Collections;

namespace XRay.Player {

	public class FollowMouse : MonoBehaviour {
		
		private GameObject player;
		private bool usingController = false;
		private Vector3 lastMousePos;
		private float lastJoyX = 0f;
		private float lastJoyY = 0f;
		
		// Use this for initialization
		void Start () {
			player = GameObject.Find("Player");
		}
		
		// Update is called once per frame
		void Update () {
			transform.position = player.transform.position;
			if(Input.GetAxis("Joy X") != 0 || Input.GetAxis("Joy Y") != 0) {
				usingController = true;
			}
			if(lastMousePos != Input.mousePosition){
				usingController = false;
			}
			lastMousePos = Input.mousePosition;
			
			if(!usingController) {
				var mouse_pos = Input.mousePosition;
				var object_pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				mouse_pos.x = mouse_pos.x - object_pos.x;
				mouse_pos.y = mouse_pos.y - object_pos.y;
				float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
			}else {
				if(Input.GetAxis("Joy X") != 0f)
					lastJoyX = Input.GetAxis("Joy X");
				if(Input.GetAxis("Joy Y") != 0f)
					lastJoyY = Input.GetAxis("Joy Y");
				float angle = Mathf.Atan2(-lastJoyY, lastJoyX) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
			}
		}
	}
}
