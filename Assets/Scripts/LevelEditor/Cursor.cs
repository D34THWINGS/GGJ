using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XRay.UI;

namespace XRay.LevelEditor {
	
	public enum XRayObjects {
		INTERACTIBLE,
		GROUND
	}
	
	public enum EditorFunctions {
		NONE,
		SCALEX,
		SCALEY,
		SCALE,
		ROTATE
	}
	
	public class Cursor : MonoBehaviour {
		
		public float Speed = 0.2f;
		public Texture PlaceButton;
		public Texture ScaleButton;
		public Texture RotateButton;
		public Texture ScaleXYButton;
		public Texture ScaleXButton;
		public Texture ScaleYButton;
				
		private XRayObjects SelectedObjectType = XRayObjects.GROUND;
		private GameObject ObjectHolder;
		private GameObject LevelHolder;
		private EditorFunctions currentTool = EditorFunctions.NONE;
		private int colorChangeDir = -1;
		private TransformButton btnTree;
		private bool placeMode = true;

		public bool HasCurrentObject {
			get {
				return ObjectHolder.transform.childCount > 0;
			}
		}
		
		public void Start () {
			ObjectHolder = GameObject.Find("ObjectHolder");
			LevelHolder = GameObject.Find("LevelHolder");

			btnTree = new TransformButton {
				ChildButtons = new Dictionary<string, TransformButton> {
					{
						"Shape", new TransformButton { BtnTexture = PlaceButton, JoystickButton = KeyCode.JoystickButton4, KeyboardButton = KeyCode.Q, Active = true }
					},{
						"Scale", new TransformButton {
							BtnTexture = ScaleButton, JoystickButton = KeyCode.JoystickButton2, KeyboardButton = KeyCode.E, Active = true,
							ChildButtons = new Dictionary<string, TransformButton> {
								{"XY", new TransformButton { BtnTexture = ScaleXButton, JoystickButton = KeyCode.JoystickButton1, KeyboardButton = KeyCode.Alpha1 }},
								{"X", new TransformButton { BtnTexture = ScaleXYButton, JoystickButton = KeyCode.JoystickButton2, KeyboardButton = KeyCode.Alpha1 }},
								{"Y", new TransformButton { BtnTexture = ScaleYButton, JoystickButton = KeyCode.JoystickButton3, KeyboardButton = KeyCode.Alpha2 }}
							}
						}
					},{
						"Rotate", new TransformButton { BtnTexture = RotateButton, JoystickButton = KeyCode.JoystickButton3, KeyboardButton = KeyCode.R, Active = true }
					}
				},
				Spacing = 100f, Enable = true, Position = new Vector2(Screen.width / 2, Screen.height - 10)
			}.Init();
			btnTree.OnPress += (Name) => {
				print (Name);
				switch (Name) {
				case "Shape":
					if (!HasCurrentObject) {

					}
					placeMode = true;
					break;
				case "Scale.XY":
					currentTool = EditorFunctions.SCALE;
					break;
				case "Scale.X":
					currentTool = EditorFunctions.SCALEX;
					break;
				case "Scale.Y":
					currentTool = EditorFunctions.SCALEY;
					break;
				case "Rotate":
					currentTool = EditorFunctions.ROTATE;
					break;
				default:
					break;
				}
			};
		}
		
		public void Update () {
			// Cancel placing
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
				if (!placeMode) {
					DropCurrentObject();
				} else {
					RemoveCurrentObject();
				}
				placeMode = false;
				currentTool = EditorFunctions.NONE;
			}

			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) {
				if (!placeMode) {
					// Drop object
					if (!DropCurrentObject()) {
						// Grab object
						var overlap = Physics2D.OverlapPoint(transform.position);
						if (overlap) {
							overlap.transform.parent = ObjectHolder.transform;
							overlap.transform.position = transform.position;
						}
					}
				} else {
					// Place object
					PlaceCurrentObject();
				}
			}
			
			if (HasCurrentObject) {
				// Scaling object
				if (currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEX || currentTool == EditorFunctions.SCALEY) {
					var diff = new Vector3(
						currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEX ? 0.1f : 0f, 
						currentTool == EditorFunctions.SCALE || currentTool == EditorFunctions.SCALEY ? 0.1f : 0f);
					if (Input.GetKey(KeyCode.KeypadPlus)) {
						ObjectHolder.transform.GetChild(0).localScale += diff;
					} else if (Input.GetKey(KeyCode.KeypadMinus)) {
						ObjectHolder.transform.GetChild(0).localScale -= diff;
					}
				}

				if (currentTool == EditorFunctions.ROTATE) {
					var diff = new Vector3(0f, 0f, 1f);
					var rot = ObjectHolder.transform.GetChild(0).rotation;
					if (Input.GetKey(KeyCode.KeypadPlus)) {
						ObjectHolder.transform.GetChild(0).rotation = Quaternion.Euler(rot.eulerAngles + diff);
					} else if (Input.GetKey(KeyCode.KeypadMinus)) {
						ObjectHolder.transform.GetChild(0).rotation = Quaternion.Euler(rot.eulerAngles - diff);
					}
				}
			}
			
			// Cursor position
			var h = Input.GetAxis("Horizontal");
			var v = Input.GetAxis("Vertical");
			var move = new Vector3(h * Speed, v * Speed, 0);
			transform.position += move;

			btnTree.Update();
		}

		public void OnGUI () {
			btnTree.DrawButtonTree(btnTree.Position);
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

		public bool DropCurrentObject() {
			if (HasCurrentObject) {
				var holded = ObjectHolder.transform.GetChild(0).gameObject;
				holded.transform.parent = LevelHolder.transform;
				return true;
			}
			return false;
		}
	}
}
