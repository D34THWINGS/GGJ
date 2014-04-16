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
		
		private AudioSource[] audios;
		private TransformButton btnTree;
		
		// Use this for initialization
		void Start () {
			var player = GameObject.Find("Player");
			audios = gameObject.GetComponents<AudioSource>();
			
			btnTree = new TransformButton {
				ChildButtons = new Dictionary<string, TransformButton> {
					{
						"Shape", new TransformButton {
							BtnTexture = BtnShape, JoystickButton = KeyCode.JoystickButton4, KeyboardButton = KeyCode.Q,
							ChildButtons = new Dictionary<string, TransformButton> {
								{"Square", new TransformButton { BtnTexture = BtnSquare, JoystickButton = KeyCode.JoystickButton2, KeyboardButton = KeyCode.Alpha1 }},
								{"Circle", new TransformButton { BtnTexture = BtnCircle, JoystickButton = KeyCode.JoystickButton3, KeyboardButton = KeyCode.Alpha2 }}
							}
						}
					},{
						"Weight", new TransformButton {
							BtnTexture = BtnWeight, JoystickButton = KeyCode.JoystickButton5, KeyboardButton = KeyCode.E,
							ChildButtons = new Dictionary<string, TransformButton> {
								{"Light", new TransformButton { BtnTexture = BtnLight, JoystickButton = KeyCode.JoystickButton2, KeyboardButton = KeyCode.Alpha1 }},
								{"Heavy", new TransformButton { BtnTexture = BtnHeavy, JoystickButton = KeyCode.JoystickButton3, KeyboardButton = KeyCode.Alpha2 }}
							}
						}
					},{
						"Material", new TransformButton {
							BtnTexture = BtnWeight, JoystickButton = KeyCode.JoystickButton5, KeyboardButton = KeyCode.R, Spacing = 50f,
							ChildButtons = new Dictionary<string, TransformButton> {
								{"Normal", new TransformButton { BtnTexture = BtnLight, JoystickButton = KeyCode.JoystickButton1, KeyboardButton = KeyCode.Alpha1 }},
								{"Glass", new TransformButton { BtnTexture = BtnHeavy, JoystickButton = KeyCode.JoystickButton2, KeyboardButton = KeyCode.Alpha2 }},							
								{"Rubber", new TransformButton { BtnTexture = BtnLight, JoystickButton = KeyCode.JoystickButton3, KeyboardButton = KeyCode.Alpha3 }},
								{"Bouncy", new TransformButton { BtnTexture = BtnHeavy, JoystickButton = KeyCode.JoystickButton4, KeyboardButton = KeyCode.Alpha4 }}
							}
						}
					}
				},
				Spacing = 100f, Enable = true, Position = new Vector2(Screen.width / 2, Screen.height - 10)
			}.Init();
			btnTree.OnPress += (Name) => {
				print (Name);
				switch (Name) {
				case "Shape.Square":
					player.GetComponent<Reshape>().CurrentShape = 0;
					audios[0].Play();
					break;
				case "Shape.Circle":
					player.GetComponent<Reshape>().CurrentShape = 1;
					audios[0].Play();
					break;
				case "Weight.Light":
					player.rigidbody2D.mass = StaticVariables.LightWeight;
					WeightMessage.GetComponent<TextMesh>().text = "Soft";
					Instantiate(WeightMessage, player.transform.position, Quaternion.identity);
					audios[1].Play();
					break;
				case "Weight.Heavy":
					player.rigidbody2D.mass = StaticVariables.HeavyWeight;
					WeightMessage.GetComponent<TextMesh>().text = "Heavy";
					Instantiate(WeightMessage, player.transform.position, Quaternion.identity);
					audios[2].Play();
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
				default:
					break;
				}
			};
		}
		
		// Update is called once per frame
		void Update () {
			btnTree.Update();
			
			btnTree["Shape"].Active = StaticVariables.HasPower(StaticVariables.Powers.RESHAPE_CIRCLE);
			btnTree["Weight"].Active = StaticVariables.HasPower(StaticVariables.Powers.CHANGE_WEIGHT);
			btnTree["Material"].Active = StaticVariables.HasPower(StaticVariables.Powers.CHANGE_MATERIAL);
		}
		
		void OnGUI () {
			btnTree.DrawButtonTree(btnTree.Position);
		}
	}
}
