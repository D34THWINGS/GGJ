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
        public TranformInterface TUI;

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
        private GameObject _spawn;

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
            _spawn = GameObject.Find("PlayerSpawnPoint");

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
                            () => XRayInput.GetKeysDown(KeyCode.JoystickButton4, KeyCode.JoystickButton0),
                        ChildButtons = new List<TransformButton> {
                            new TransformButton("Interactible") {
                                ButtonTrigger = () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickLeftArrow))
                            },
                            new TransformButton("Prefab") {
                                ButtonTrigger = () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickRightArrow))
                            },
                            new TransformButton("Spawn") {
                                ButtonTrigger = () => (XRayInput.GetKeyDown(XRayKeyCode.JoystickUpArrow))
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
                        ButtonTrigger = () => XRayInput.GetKeysDown(KeyCode.JoystickButton5, KeyCode.JoystickButton0)
                    },
                    new TransformButton("Delete") {
                        ButtonTrigger = () => XRayInput.GetKeysDown(KeyCode.JoystickButton5, KeyCode.JoystickButton1),
                        KeyboardButton = KeyCode.Delete
                    },
                    new TransformButton("Place") {
                        JoystickButton = KeyCode.JoystickButton0,
                        KeyboardButton = KeyCode.Space
                    },
                    new TransformButton("Cancel") {
                        JoystickButton = KeyCode.JoystickButton1,
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
                    case "Shape.Spawn":
                        DropCurrentObject();
                        GrabObject(_spawn);
                        break;
                    case "Scale":
                        _currentTool = EditorFunctions.Scale;
                        break;
                    case "Rotate":
                        _currentTool = EditorFunctions.Rotate;
                        break;
                    case "Copy":
                        if (HasCurrentObject) {
                            if (_grabOrigin != null)
                                Destroy(_grabOrigin.gameObject);
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
                            GrabObject(overlap.gameObject);
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
                            var val = XRayInput.GetRightStick();
                            ScaleCurrentObject(val.x*0.5f, -val.y*0.5f);
                        }, 50));
                    }
                    if (Input.GetKeyUp(KeyCode.JoystickButton4)) {
                        _looper = false;
                    }
                    if (!Input.GetKey(KeyCode.JoystickButton4)) {
                        var val = XRayInput.GetRightStick();
                        ScaleCurrentObject(new Vector3(val.x*0.2f, -val.y*0.2f));
                    }
                }

                // Rotate object
                if (_currentTool == EditorFunctions.Rotate) {
                    if (Input.GetKeyDown(KeyCode.JoystickButton4)) {
                        CurrentObject.transform.rotation = Quaternion.Euler(RoundToClosestHalf(CurrentObject.transform.rotation.eulerAngles, 22.5f));
                        StartCoroutine(LoopWithWait(() => {
                            var val = XRayInput.GetRightStick();
                            RotateCurrentObject(val.x*22.5f);
                        }, 100));
                    }
                    if (Input.GetKeyUp(KeyCode.JoystickButton4)) {
                        _looper = false;
                    }
                    if (!Input.GetKey(KeyCode.JoystickButton4)) {
                        var val = XRayInput.GetRightStick();
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
            if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Escape)) &&
                !_btnTree.HasOneEnabledChild) {
                _btnTree.DisableChilds();
            }

            // Blink spawn point
            var sr = _spawn.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Sin(Time.time*3f)/4f + 0.75f);

            // Test mode
            if (!Input.GetKeyDown(KeyCode.JoystickButton7) && !Input.GetKeyDown(KeyCode.Return)) return;
            StartTestMode();
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
        /// Grabs the given object.
        /// </summary>
        /// <param name="grabbed">The object to grab.</param>
        public void GrabObject(GameObject grabbed) {
            if (HasCurrentObject) return;
            transform.position = grabbed.transform.position;
            grabbed.transform.parent = _objectHolder.transform;
            _grabOrigin =
                Instantiate(gameObject.transform, grabbed.transform.position, grabbed.transform.rotation)
                as Transform;
            if (_grabOrigin != null) _grabOrigin.gameObject.SetActive(false);
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

        /// <summary>
        /// Rotates the current object.
        /// </summary>
        /// <param name="diff">The angle in degrees to add to the current angle.</param>
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
        /// Returns the rounding of the given number to the closest given half.
        /// </summary>
        /// <param name="number">Number to round.</param>
        /// <param name="half">The rounding unit.</param>
        /// <returns>Rounded number.</returns>
        private static float RoundToClosestHalf(float number, float half) {
            return (float) Math.Round(number*(1f/half), MidpointRounding.AwayFromZero)/(1f/half);
        }

        /// <summary>
        /// Returns the rounding of both axis of the given Vector to the closest given half.
        /// </summary>
        /// <param name="vector">Vector to round.</param>
        /// <param name="half">The rounding unit.</param>
        /// <returns>Rounded number.</returns>
        private static Vector2 RoundToClosestHalf(Vector2 vector, float half) {
            return new Vector2(RoundToClosestHalf(vector.x, half), RoundToClosestHalf(vector.y, half));
        }

        /// <summary>
        /// Enables the test mode of the level.
        /// </summary>
        private void StartTestMode() {
            enabled = false;
            _crosshair.SetActive(false);
            MainCamera.SetActive(true);
            Player.SetActive(true);
            Player.transform.position = _spawn.transform.position;
            _spawn.SetActive(false);
            TUI.enabled = true;
            var cpy = Instantiate(_levelHolder) as GameObject;
            if (cpy != null) cpy.name = "LevelCopy";
            _levelHolder.SetActive(false);
            GameObject.Find("PressStart").GetComponent<GUIText>().text = "Press start to end the test";
            StartCoroutine(EndTestMode());
        }

        /// <summary>
        /// Waits for the player to press the start button and end the test mode.
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndTestMode() {
            do {
                yield return 0;
            } while (!Input.GetKeyDown(KeyCode.JoystickButton7) && !Input.GetKeyDown(KeyCode.Return));

            Destroy(GameObject.Find("LevelCopy"));
            _levelHolder.SetActive(true);
            _crosshair.SetActive(true);
            Player.SetActive(false);
            MainCamera.SetActive(false);
            _spawn.SetActive(true);
            TUI.enabled = false;
            GameObject.Find("PressStart").GetComponent<GUIText>().text = "Press start to test your level";
            enabled = true;
        }

        /// <summary>
        /// Executes the given callback every [waitTime] seconds until _looper is set to false. 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        private IEnumerator LoopWithWait(LoopDelegate callback, float waitTime) {
            _looper = true;
            while (_looper) {
                callback();
                yield return new WaitForSeconds(waitTime/1000);
            }
        }
    }
}