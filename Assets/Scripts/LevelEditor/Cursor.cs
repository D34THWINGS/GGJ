using UnityEngine;
using System.Collections.Generic;
using XRay.UI;

namespace XRay.LevelEditor {
    public enum XRayObjects {
        Interactible,
        Ground
    }

    public enum EditorFunctions {
        None,
        Scalex,
        Scaley,
        Scale,
        Rotate
    }

    public class Cursor : MonoBehaviour {
        public float Speed = 0.2f;

        private XRayObjects _selectedObjectType = XRayObjects.Ground;
        private GameObject _objectHolder;
        private GameObject _levelHolder;
        private EditorFunctions _currentTool = EditorFunctions.None;
        private int _colorChangeDir = -1;
        private TransformButton _btnTree;
        private bool _placeMode = true;
        private Transform _grabOrigin;

        /// <summary>
        /// Returns the object holded into the cursor.
        /// </summary>
        public GameObject CurrentObject {
            get { return HasCurrentObject ? _objectHolder.transform.GetChild(0).gameObject : null; }
            set {
                if (HasCurrentObject) {
                    if (_placeMode) {
                        RemoveCurrentObject();
                    }
                    else {
                        DropCurrentObject();
                    }
                }
                value.transform.parent = _objectHolder.transform;
            }
        }

        /// <summary>
        /// Return if an object is holded into the cursor
        /// </summary>
        public bool HasCurrentObject {
            get { return _objectHolder.transform.childCount > 0; }
        }

        public void Start() {
            _objectHolder = GameObject.Find("ObjectHolder");
            _levelHolder = GameObject.Find("LevelHolder");

            var interactiveObject = Resources.Load("Prefabs/InteractibleBlock", typeof (GameObject)) as GameObject;

            // Interface creation
            _btnTree = new TransformButton("Root") {
                ChildButtons = new List<TransformButton> {
                    new TransformButton("Shape") {
                        KeyboardButton = KeyCode.Q,
                        Active = true,
                        Spacing = 5f,
                        ButtonTrigger =
                            () => XRayInput.GetKeysDown(KeyCode.Joystick1Button4, KeyCode.Joystick1Button0),
                        ChildButtons = new List<TransformButton> {
                            new TransformButton("Interactible") {
                                ButtonTrigger =
                                    () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickLeftArrow))
                            },
                            new TransformButton("Prefab") {
                                ButtonTrigger =
                                    () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickRightArrow))
                            }
                        }
                    }
                    ,
                    new TransformButton("Scale") {
                        JoystickButton = KeyCode.JoystickButton2,
                        KeyboardButton = KeyCode.E,
                        Spacing = 5f,
                        ChildButtons = new List<TransformButton> {
                            new TransformButton("XY") {
                                ButtonTrigger =
                                    () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickLeftArrow)),
                                KeyboardButton = KeyCode.Alpha1
                            },
                            new TransformButton("X") {
                                ButtonTrigger =
                                    () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickUpArrow)),
                                KeyboardButton = KeyCode.Alpha2
                            },
                            new TransformButton("Y") {
                                ButtonTrigger =
                                    () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickRightArrow)),
                                KeyboardButton = KeyCode.Alpha3
                            }
                        }
                    }
                    ,
                    new TransformButton("Rotate") {
                        JoystickButton = KeyCode.JoystickButton3,
                        KeyboardButton = KeyCode.R
                    },
                    new TransformButton("Copy") {
                        KeyboardButton = KeyCode.C,
                        ButtonTrigger =
                            () => XRayInput.GetKeysDown(KeyCode.Joystick1Button5, KeyCode.Joystick1Button0)
                    },
                    new TransformButton("Delete") {
                        ButtonTrigger =
                            () => XRayInput.GetKeysDown(KeyCode.Joystick1Button5, KeyCode.Joystick1Button1),
                        KeyboardButton = KeyCode.Delete
                    },
                    new TransformButton("Place") {
                        JoystickButton = KeyCode.JoystickButton0,
                        KeyboardButton = KeyCode.Space
                    },
                    new TransformButton("Cancel") {
                        JoystickButton = KeyCode.Joystick1Button1,
                        KeyboardButton = KeyCode.Escape
                    },
                    new TransformButton("Grab") {
                        JoystickButton = KeyCode.JoystickButton0,
                        KeyboardButton = KeyCode.Space
                    }
                },
                Spacing = 10f,
                Enabled = true,
                Position = new Vector2((float) Screen.width/2, Screen.height - 10)
            }.Init("Editor");

            // Bindings
            _btnTree.OnPress += btnName => {
                print(btnName);
                switch (btnName) {
                    case "Shape.Interactible":
                        CurrentObject =
                            Instantiate(interactiveObject, transform.position, new Quaternion()) as GameObject;
                        _placeMode = true;
                        break;
                    case "Scale.XY":
                        _currentTool = EditorFunctions.Scale;
                        break;
                    case "Scale.X":
                        _currentTool = EditorFunctions.Scalex;
                        break;
                    case "Scale.Y":
                        _currentTool = EditorFunctions.Scaley;
                        break;
                    case "Rotate":
                        _currentTool = EditorFunctions.Rotate;
                        break;
                    case "Copy":
                        if (HasCurrentObject) {
                            PlaceCurrentObject();
                            _placeMode = true;
                        }
                        break;
                    case "Delete":
                        RemoveCurrentObject();
                        break;
                    case "Place":
                        if (!_placeMode) {
                            DropCurrentObject();
                        }
                        else {
                            PlaceCurrentObject();
                        }
                        break;
                    case "Cancel":
                        if (!_placeMode) {
                            CurrentObject.transform.position = _grabOrigin.position;
                            CurrentObject.transform.localScale = _grabOrigin.localScale;
                            CurrentObject.transform.rotation = _grabOrigin.rotation;
                            DropCurrentObject();
                        }
                        else {
                            RemoveCurrentObject();
                        }
                        _placeMode = false;
                        _currentTool = EditorFunctions.None;
                        break;
                    case "Grab":
                        var overlap = Physics2D.OverlapPoint(transform.position);
                        if (overlap) {
                            transform.position = overlap.transform.position;
                            overlap.transform.parent = _objectHolder.transform;
                            _grabOrigin =
                                Instantiate(overlap.transform, overlap.transform.position, overlap.transform.rotation)
                                as Transform;
                            if (_grabOrigin != null) _grabOrigin.gameObject.SetActive(false);
                        }
                        break;
                }
            };
        }

        public void Update() {
            XRayInput.Update();

            if (HasCurrentObject) {
                // Scaling object
                if (_currentTool == EditorFunctions.Scale || _currentTool == EditorFunctions.Scalex ||
                    _currentTool == EditorFunctions.Scaley) {
                    var diff = new Vector3(
                        _currentTool == EditorFunctions.Scale || _currentTool == EditorFunctions.Scalex ? 0.05f : 0f,
                        _currentTool == EditorFunctions.Scale || _currentTool == EditorFunctions.Scaley ? 0.05f : 0f);
                    var newScale = new Vector3();
                    if (Input.GetKey(KeyCode.KeypadPlus)) {
                        newScale = CurrentObject.transform.localScale + diff;
                    }
                    else if (Input.GetKey(KeyCode.KeypadMinus)) {
                        newScale = CurrentObject.transform.localScale - diff;
                    }
                    if (newScale.x > 0 && newScale.y > 0) {
                        CurrentObject.transform.localScale = newScale;
                    }
                }

                if (_currentTool == EditorFunctions.Rotate) {
                    var diff = new Vector3(0f, 0f, 1f);
                    var rot = _objectHolder.transform.GetChild(0).rotation;
                    if (Input.GetKey(KeyCode.KeypadPlus)) {
                        CurrentObject.transform.rotation = Quaternion.Euler(rot.eulerAngles + diff);
                    }
                    else if (Input.GetKey(KeyCode.KeypadMinus)) {
                        CurrentObject.transform.rotation = Quaternion.Euler(rot.eulerAngles - diff);
                    }
                }
            }

            // Cursor position
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            var move = new Vector3(h*Speed, v*Speed, 0);
            transform.position += move;

            // Buttons activation
            _btnTree["Scale"].Active = HasCurrentObject;
            _btnTree["Rotate"].Active = _btnTree["Place"].Active = _btnTree["Cancel"].Active = HasCurrentObject;
            _btnTree["Copy"].Active = _btnTree["Delete"].Active = HasCurrentObject && !_placeMode;
            _btnTree["Grab"].Active = !HasCurrentObject && !_placeMode && Physics2D.OverlapPoint(transform.position);

            _btnTree.Update();
            if ((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape)) &&
                !_btnTree.HasOneEnabledChild) {
                _btnTree.DisableChilds();
            }
        }

        public void OnGUI() {
            _btnTree.DrawButtonTree(_btnTree.Position);
        }

        /// <summary>
        /// Destroy the object contained into the cursor.
        /// </summary>
        public void RemoveCurrentObject() {
            if (HasCurrentObject) {
                Destroy(CurrentObject);
            }
        }

        /// <summary>
        /// Copy the object holded into the cursor to the level holder.
        /// </summary>
        public void PlaceCurrentObject() {
            if (HasCurrentObject) {
                PlaceObject(CurrentObject);
            }
        }

        /// <summary>
        /// Copy the given object to the level holder. 
        /// </summary>
        /// <param name="model">The object to copy.</param>
        public void PlaceObject(GameObject model) {
            var placed = (GameObject) Instantiate(model, model.transform.position, model.transform.rotation);
            placed.name = model.name;
            placed.transform.parent = _levelHolder.transform;
        }

        /// <summary>
        /// Drop the object into the level holder.
        /// </summary>
        /// <returns></returns>
        public void DropCurrentObject() {
            if (!HasCurrentObject) return;
            CurrentObject.transform.parent = _levelHolder.transform;
            if (_grabOrigin != null)
                Destroy(_grabOrigin.gameObject);
        }
    }
}