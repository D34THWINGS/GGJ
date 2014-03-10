using UnityEngine;
using System.Collections.Generic;
using XRay;

public class TranformInterface : MonoBehaviour {
	public Texture BtnShape;
	public Texture BtnWeight;
	public Texture BtnSquare;
	public Texture BtnCircle;
	public Texture BtnTriangle;
	public Texture BtnLight;
	public Texture BtnHeavy;

	public GameObject WeightMessage;

	private AudioSource[] audios;

	private TransformButton btnTree;

	// Use this for initialization
	void Start () {
		if(!BtnShape || !BtnWeight){
			Debug.LogError("Assign a Texture in the inspector.");
		}

		var player = GameObject.Find("Player");

		audios = gameObject.GetComponents<AudioSource>();

		btnTree = new TransformButton {
			ChildButtons = new Dictionary<string, TransformButton> {
				{
					"Shape", new TransformButton {
						BtnTexture = BtnShape, JoystickButton = KeyCode.JoystickButton4,
						ChildButtons = new Dictionary<string, TransformButton> {
							{"Square", new TransformButton { BtnTexture = BtnSquare, JoystickButton = KeyCode.JoystickButton2 }},
							{"Circle", new TransformButton { BtnTexture = BtnCircle, JoystickButton = KeyCode.JoystickButton3 }}
						}
					}
				},{
					"Weight", new TransformButton {
						BtnTexture = BtnWeight, JoystickButton = KeyCode.JoystickButton5,
						ChildButtons = new Dictionary<string, TransformButton> {
							{"Light", new TransformButton { BtnTexture = BtnLight, JoystickButton = KeyCode.JoystickButton2 }},
							{"Heavy", new TransformButton { BtnTexture = BtnHeavy, JoystickButton = KeyCode.JoystickButton3 }}
						}
					}
				}
			},
			Spacing = 400f, Enable = true, Position = new Vector2(Screen.width / 2, Screen.height - 10)
		};
		btnTree.Init();
		btnTree.OnPress += (Name) => {
			switch (Name) {
				case "Square":
					player.GetComponent<Reshape>().CurrentShape = 0;
					audios[0].Play();
					break;
				case "Circle":
					player.GetComponent<Reshape>().CurrentShape = 1;
					audios[0].Play();
					break;
				case "Light":
					player.rigidbody2D.mass = StaticVariables.LightWeight;
					WeightMessage.GetComponent<TextMesh>().text = "Soft";
					Instantiate(WeightMessage, player.transform.position, Quaternion.identity);
					audios[1].Play();
					break;
				case "Heavy":
					player.rigidbody2D.mass = StaticVariables.HeavyWeight;
					WeightMessage.GetComponent<TextMesh>().text = "Heavy";
					Instantiate(WeightMessage, player.transform.position, Quaternion.identity);
					audios[2].Play();
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
	}
	
	void OnGUI () {
		btnTree.DrawButtonTree(btnTree.Position);
	}
}
