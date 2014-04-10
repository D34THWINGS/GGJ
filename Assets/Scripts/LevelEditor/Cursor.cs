using UnityEngine;
using System.Collections;

namespace XRay.LevelEditor {

	public enum XRayObjects {
		INTERACTIBLE,
		GROUND
	}

	public class Cursor : MonoBehaviour {

		public float Speed = 0.2f;

		private XRayObjects SelectedObjectType = XRayObjects.GROUND;
		private GameObject ObjectHolder;
		private GameObject LevelHolder;

		void Start () {
			ObjectHolder = GameObject.Find("ObjectHolder");
			LevelHolder = GameObject.Find("LevelHolder");
		}

		void Update () {
			// Placing object
			if (Input.GetKeyDown(KeyCode.Space)) {
				var holded = ObjectHolder.transform.GetChild(0).gameObject;
				var placed = (GameObject) Instantiate(holded, transform.position, new Quaternion());
				placed.transform.parent = LevelHolder.transform;
			}

			// Scaling object
			//if (Input.GetKeyDown(KeyCode.Plus))

			// Cursor position
			var h = Input.GetAxis("Horizontal");
			var v = Input.GetAxis("Vertical");
			var move = new Vector3(h * Speed, v * Speed, 0);
			transform.position += move;
		}
	}
}
