using UnityEngine;
using System.Collections.Generic;
using XRay.Objects;

namespace XRay.UI {
    public class TranformInterface : MonoBehaviour {
        public Texture BtnShape;
        public Texture BtnWeight;
        public Texture BtnSquare;
        public Texture BtnCircle;
        public Texture BtnTriangle;
        public Texture BtnLight;
        public Texture BtnHeavy;

        public PhysicsMaterial2D MaterialNormal;
        public PhysicsMaterial2D MaterialGlass;
        public PhysicsMaterial2D MaterialRubber;
        public PhysicsMaterial2D MaterialBouncy;

        public GameObject WeightMessage;

        private AudioSource[] _audios;
        private TransformButton _btnTree;

        public void Start() {
            var player = GameObject.Find("Player");
            _audios = gameObject.GetComponents<AudioSource>();

            // Buttons creation
            _btnTree = new TransformButton("Root") {
                ChildButtons = new List<TransformButton> {
                    new TransformButton("Shape") {
                        BtnTexture = BtnShape,
                        JoystickButton = KeyCode.JoystickButton4,
                        KeyboardButton = KeyCode.Q,
                        ChildButtons = new List<TransformButton> {
                            new TransformButton("Square") {
                                BtnTexture = BtnSquare,
                                JoystickButton = KeyCode.JoystickButton2,
                                KeyboardButton = KeyCode.Alpha1
                            },
                            new TransformButton("Circle") {
                                BtnTexture = BtnCircle,
                                JoystickButton = KeyCode.JoystickButton3,
                                KeyboardButton = KeyCode.Alpha2
                            }
                        }
                    },
                    new TransformButton("Weight") {
                        BtnTexture = BtnWeight,
                        JoystickButton = KeyCode.JoystickButton5,
                        KeyboardButton = KeyCode.E,
                        ChildButtons = new List<TransformButton> {
                            new TransformButton("Light") {
                                BtnTexture = BtnLight,
                                JoystickButton = KeyCode.JoystickButton2,
                                KeyboardButton = KeyCode.Alpha1
                            },
                            new TransformButton("Heavy") {
                                BtnTexture = BtnHeavy,
                                JoystickButton = KeyCode.JoystickButton3,
                                KeyboardButton = KeyCode.Alpha2
                            }
                        }
                    },
                    new TransformButton("Material") {
                        BtnTexture = BtnWeight,
                        JoystickButton = KeyCode.JoystickButton5,
                        KeyboardButton = KeyCode.R,
                        Spacing = 50f,
                        ChildButtons = new List<TransformButton> {
                            new TransformButton("Normal") {
                                BtnTexture = BtnLight,
                                JoystickButton = KeyCode.JoystickButton1,
                                KeyboardButton = KeyCode.Alpha1
                            },
                            new TransformButton("Glass") {
                                BtnTexture = BtnHeavy,
                                JoystickButton = KeyCode.JoystickButton2,
                                KeyboardButton = KeyCode.Alpha2
                            },
                            new TransformButton("Rubber") {
                                BtnTexture = BtnLight,
                                JoystickButton = KeyCode.JoystickButton3,
                                KeyboardButton = KeyCode.Alpha3
                            },
                            new TransformButton("Bouncy") {
                                BtnTexture = BtnHeavy,
                                JoystickButton = KeyCode.JoystickButton4,
                                KeyboardButton = KeyCode.Alpha4
                            }
                        }
                    }
                },
                Spacing = 100f,
                Enabled = true,
                Position = new Vector2((float) Screen.width/2, Screen.height - 10)
            }.Init("Game");

            // Bindings
            _btnTree.OnPress += (buttonName) => {
                print(buttonName);
                switch (buttonName) {
                    case "Shape.Square":
                        player.GetComponent<Reshape>().CurrentShape = 0;
                        _audios[0].Play();
                        break;
                    case "Shape.Circle":
                        player.GetComponent<Reshape>().CurrentShape = 1;
                        _audios[0].Play();
                        break;
                    case "Weight.Light":
                        player.rigidbody2D.mass = StaticVariables.LightWeight;
                        WeightMessage.GetComponent<TextMesh>().text = "Soft";
                        Instantiate(WeightMessage, player.transform.position, Quaternion.identity);
                        _audios[1].Play();
                        break;
                    case "Weight.Heavy":
                        player.rigidbody2D.mass = StaticVariables.HeavyWeight;
                        WeightMessage.GetComponent<TextMesh>().text = "Heavy";
                        Instantiate(WeightMessage, player.transform.position, Quaternion.identity);
                        _audios[2].Play();
                        break;
                    case "Material.Normal":
                        player.collider2D.sharedMaterial = MaterialNormal;
                        break;
                    case "Material.Glass":
                        player.collider2D.sharedMaterial = MaterialGlass;
                        break;
                    case "Material.Rubber":
                        player.collider2D.sharedMaterial = MaterialRubber;
                        break;
                    case "Material.Bouncy":
                        player.collider2D.sharedMaterial = MaterialBouncy;
                        break;
                }
            };
        }
        
        public void Update() {
            _btnTree.Update();

            _btnTree["Shape"].Active = StaticVariables.HasPower(StaticVariables.Powers.RESHAPE_CIRCLE);
            _btnTree["Weight"].Active = StaticVariables.HasPower(StaticVariables.Powers.CHANGE_WEIGHT);
            _btnTree["Material"].Active = StaticVariables.HasPower(StaticVariables.Powers.CHANGE_MATERIAL);
        }

        public void OnGUI() {
            _btnTree.DrawButtonTree(_btnTree.Position);
        }
    }
}