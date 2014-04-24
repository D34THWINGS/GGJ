using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using XRay.UI;

namespace XRay.Editor {
    public class LevelEditor : MonoBehaviour {
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

        public float Speed = 0.2f;
        public GameObject MainCamera;
        public GameObject Player;

        private XRayObjects _selectedObjectType = XRayObjects.Ground;
        private GameObject _objectHolder;
        private GameObject _levelHolder;
        private EditorFunctions _currentTool = EditorFunctions.None;
        private int _colorChangeDir = -1;
        private TransformButton _btnTree;
        private bool _placeMode = true;
        private Transform _grabOrigin;
        private Camera _cursorCamera;
        private bool _testMode = false;
        private GameObject _crosshair;
        private bool _looper;
        private GUIText _pressLB;

        private delegate void LoopDelegate();

        /// <summary>
        /// Returns the object holded into the cursor.
        /// </summary>
        public GameObject CurrentObject {
            get { return HasCurrentObject ? _objectHolder.transform.GetChild(0).gameObject : null; }
            set {
                if (HasCurrentObject) {
                    if (_placeMode) {
                        RemoveCurrentObject();
                    } else {
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
            // Find objects
            _objectHolder = GameObject.Find("ObjectHolder");
            _levelHolder = GameObject.Find("LevelHolder");
            _cursorCamera = GameObject.Find("CursorCamera").GetComponent<Camera>();
            _crosshair = GameObject.Find("Crosshair");
            _pressLB = GameObject.Find("PressLB").GetComponent<GUIText>();

            // Disable player and it's camera
            MainCamera.SetActive(false);
            Player.SetActive(false);

            // Load prefabs
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
                    },
                    new TransformButton("Scale") {
                        JoystickButton = KeyCode.JoystickButton2,
                        KeyboardButton = KeyCode.E,
                        Spacing = 5f
                    },
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
                    case "Scale":
                        _currentTool = EditorFunctions.Scale;
                        break;
                    case "Rotate":
                        _currentTool = EditorFunctions.Rotate;
                        break;
                    case "Copy":
                        if (HasCurrentObject) {
                            Destroy(_grabOrigin);
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
                        } else {
                            PlaceCurrentObject();
                        }
                        break;
                    case "Cancel":
                        if (!_placeMode) {
                            CurrentObject.transform.position = _grabOrigin.position;
                            CurrentObject.transform.localScale = _grabOrigin.localScale;
                            CurrentObject.transform.rotation = _grabOrigin.rotation;
                            DropCurrentObject();
                        } else {
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

            if (_currentTool == EditorFunctions.Scale || _currentTool == EditorFunctions.Rotate) {
                _pressLB.enabled = true;
            } else {
                _pressLB.enabled = false;
            }

            if (HasCurrentObject) {
                // Scaling object
                if (_currentTool == EditorFunctions.Scale) {
                    if (Input.GetKeyDown(KeyCode.JoystickButton4)) {
                        CurrentObject.transform.localScale = RoundToClosestHalf(CurrentObject.transform.localScale, 0.5f);
                        StartCoroutine(LoopWithWait(() => {
                            var val = GetRightStick();
                            ScaleCurrentObject(val.x*0.5f, -val.y*0.5f);
                        }, 50));
                    }
                    if (Input.GetKeyUp(KeyCode.JoystickButton4)) {
                        _looper = false;
                    }
                    if (!Input.GetKey(KeyCode.JoystickButton4)) {
                        var val = GetRightStick();
                        ScaleCurrentObject(new Vector3(val.x*0.2f, -val.y*0.2f));
                    }
                }

                // Rotate object
                if (_currentTool == EditorFunctions.Rotate) {
                    if (Input.GetKeyDown(KeyCode.JoystickButton4)) {
                        StartCoroutine(LoopWithWait(() => {
                            var val = GetRightStick();
                            RotateCurrentObject(val.x*22.5f);
                        }, 100));
                    }
                    if (Input.GetKeyUp(KeyCode.JoystickButton4)) {
                        _looper = false;
                    }
                    if (!Input.GetKey(KeyCode.JoystickButton4)) {
                        var val = GetRightStick();
                        RotateCurrentObject(val.x*1f);
                    }
                }
            }

            // Zoom
            if (!Input.GetAxis("Triggers").Equals(0f)) {
                var zoom = _cursorCamera.orthographicSize + Input.GetAxis("Triggers")/10f;
                if (zoom > 3f && zoom < 30f) {
                    _cursorCamera.orthographicSize = zoom;
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

            // Check for button pressing
            _btnTree.Update();

            // Click escape or B button will close all branches of the button tree
            if ((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape)) &&
                !_btnTree.HasOneEnabledChild) {
                _btnTree.DisableChilds();
            }

            // Test mode
            if (!Input.GetKeyDown(KeyCode.JoystickButton7) && !Input.GetKeyDown(KeyCode.Return)) return;
            enabled = false;
            _crosshair.SetActive(false);
            MainCamera.SetActive(true);
            Player.SetActive(true);
            var cpy = Instantiate(_levelHolder) as GameObject;
            if (cpy != null) cpy.name = "LevelCopy";
            _levelHolder.SetActive(false);
            GameObject.Find("PressStart").GetComponent<GUIText>().text = "Press start to end the test";
            StartCoroutine(WaitStartButton());
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

        /// <summary>
        /// Adds the given values to the localScale of the current object.
        /// </summary>
        /// <param name="x">X value to add.</param>
        /// <param name="y">Y value to add.</param>
        public void ScaleCurrentObject(float x, float y) {
            if (!HasCurrentObject) return;
            float newX = CurrentObject.transform.localScale.x + x, newY = CurrentObject.transform.localScale.y + y;
            if (newX > 0.5f && newY > 0.5f) {
                CurrentObject.transform.localScale = new Vector3(newX, newY, 1f);
            }
        }

        /// <summary>
        /// Adds the given values to the localScale of the current object.
        /// </summary>
        /// <param name="diff">XY values to add.</param>
        private void ScaleCurrentObject(Vector3 diff) {
            ScaleCurrentObject(diff.x, diff.y);
        }

        private void RotateCurrentObject(float diff) {
            if (!HasCurrentObject) return;
            var newAngle = CurrentObject.transform.rotation.eulerAngles.z + diff;
            if (newAngle < 0f)
                newAngle = 360f + newAngle;
            if (newAngle > 360f)
                newAngle = -360f + newAngle;
            CurrentObject.transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        /// <summary>
        /// Return the rounding of the given number to the closest .5f.
        /// </summary>
        /// <param name="number">Number to round.</param>
        /// <param name="half">The rounding unit.</param>
        /// <returns>Rounded number.</returns>
        private static float RoundToClosestHalf(float number, float half) {
            return (float) Math.Round(number*(1f/half), MidpointRounding.AwayFromZero)/(1f/half);
        }

        private static Vector2 RoundToClosestHalf(Vector2 vector, float half) {
            return new Vector2(RoundToClosestHalf(vector.x, half), RoundToClosestHalf(vector.y, half));
        }

        private IEnumerator WaitStartButton() {
            do {
                yield return 0;
            } while (!Input.GetKeyDown(KeyCode.JoystickButton7) && !Input.GetKeyDown(KeyCode.Return));

            Destroy(GameObject.Find("LevelCopy"));
            _levelHolder.SetActive(true);
            _crosshair.SetActive(true);
            Player.SetActive(false);
            MainCamera.SetActive(false);
            GameObject.Find("PressStart").GetComponent<GUIText>().text = "Press start to test your level";
            enabled = true;
        }

        private IEnumerator LoopWithWait(LoopDelegate callback, float waitTime) {
            _looper = true;
            while (_looper) {
                callback();
                yield return new WaitForSeconds(waitTime/1000);
            }
        }

        private Vector2 GetRightStick() {
            return new Vector2(
                Mathf.Round(!Input.GetAxis("Joy X").Equals(0f)
                                ? Input.GetAxis("Joy X")
                                : Input.GetAxis("PlusMinus")),
                Mathf.Round(!Input.GetAxis("Joy Y").Equals(0f)
                                ? Input.GetAxis("Joy Y")
                                : Input.GetAxis("PlusMinus")));
        }
    }
}