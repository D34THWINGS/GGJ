using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XRay.LevelEditor {
	
	public enum XRayObjects {
		INTERACTIBLE,
		GROUND
	}
	
	public enum EditorFunctions {
		NONE,
		PLACE,
		SCALEX,
		SCALEY,
		SCALE,
		ROTATE
	}
	
	public class Cursor : MonoBehaviour {
		
		public float Speed = 0.2f;
		
		private XRayObjects SelectedObjectType = XRayObjects.GROUND;
		private GameObject ObjectHolder;
		private GameObject LevelHolder;
		private EditorFunctions currentTool = EditorFunctions.PLACE;
		private int colorChangeDir = -1;
		private Dictionary<EditorFunctions, KeyCode> bindings = new Dictionary<EditorFunctions, KeyCode> {
			{EditorFunctions.SCALE, KeyCode.Alpha1},
			{EditorFunctions.SCALEX, KeyCode.Alpha2},
			{EditorFunctions.SCALEY, KeyCode.Alpha3}
		};

		public bool HasCurrentObject {
			get {
				return ObjectHolder.transform.childCount > 0;
			}
		}
		
		public void Start () {
			ObjectHolder = GameObject.Find("ObjectHolder");
			LevelHolder = GameObject.Find("LevelHolder");
		}
		
		public void Update () {
			// Cancel placing
			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (currentTool != EditorFunctions.PLACE) {
					PlaceCurrentObject();
				} else {
					RemoveCurrentObject();
				}
				currentTool = EditorFunctions.NONE;
			}			
			
			// Move object
			var skipSelect = false;
			if (Input.GetKeyDown(KeyCode.Space) && HasCurrentObject && currentTool != EditorFunctions.PLACE) {
				ObjectHolder.transform.GetChild(0).parent = LevelHolder.transform;
				skipSelect = true;
			}

			// Select object
			if (!HasCurrentObject && Input.GetKeyDown(KeyCode.Space) && !skipSelect) {
				var overlap = Physics2D.OverlapPoint(transform.position);
				if (overlap) {
					overlap.transform.parent = ObjectHolder.transform;
					overlap.transform.position = transform.position;
				}
			}

			// Placing object
			if (Input.GetKeyDown(KeyCode.Space) && currentTool == EditorFunctions.PLACE) {
				PlaceCurrentObject();
			}
			
			if (HasCurrentObject) {
				// Scaling object
				if (currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEX || currentTool == EditorFunctions.SCALEY) {
					if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
						ObjectHolder.transform.GetChild(0).localScale += new Vector3(
							currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEX ? 1f : 0f, 
							currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEY ? 1f : 0f);
					}
					if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
						ObjectHolder.transform.GetChild(0).localScale += new Vector3(
							currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEX ? -1f : 0f, 
							currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEY ? -1f : 0f);
					}
				}

				if (currentTool == EditorFunctions.ROTATE) {
					if (Input.GetKeyDown(KeyCode.Plus)) {
						
					}
					if (Input.GetKeyDown(KeyCode.Minus)) {
						
					}
				}
			}
			
			// Cursor position
			var h = Input.GetAxis("Horizontal");
			var v = Input.GetAxis("Vertical");
			var move = new Vector3(h * Speed, v * Speed, 0);
			transform.position += move;

			// Tool selection
			foreach (var bind in bindings) {
				if (Input.GetKeyDown(bind.Value)) {
					currentTool = bind.Key;
				}
			}
		}

		public void RemoveCurrentObject () {
			if (HasCurrentObject) {
				var holded = ObjectHolder.transform.GetChild(0).gameObject;
				Destroy(holded);
			}
		}

		public void PlaceCurrentObject() {
			if (HasCurrentObject) {
				var holded = ObjectHolder.transform.GetChild(0).gameObject;
				PlaceObject(holded);
			}
		}

		public void PlaceObject(GameObject model) {
			var placed = (GameObject) Instantiate(model, model.transform.position, model.transform.rotation);
			placed.transform.parent = LevelHolder.transform;
		}
	}
}
