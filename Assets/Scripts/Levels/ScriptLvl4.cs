using UnityEngine;
using System.Collections;

public class ScriptLvl4 : MonoBehaviour {
	public GameObject VisTourne;
	public float Speed = 0f;

	private float angle = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		angle -= Speed;
		VisTourne.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
	}
}
