using UnityEngine;
using System.Collections;
using System.Timers;

public class ScriptLvl5 : MonoBehaviour {
	public GameObject VisTourne;
	public float Speed = 0f;

	public GameObject PopObject;
	public Transform PopTransform;
	public float PopInterval = 5000.0f;
	public GameObject Combinaison;
	public GameObject Door;

	private float angle = 0f;
	private bool _openDoor = false;
	private bool _pop = false;
	private Timer _timer;

	// Use this for initialization
	void Start () {
		Combinaison.GetComponent<CombinaisonController>().OnCombinaisonFull += () => {
			_openDoor = true;
			_timer.Stop();
		};

		_timer = new Timer(PopInterval);
		_timer.Elapsed += (sender, e) => {
			_pop = true;
		};
		_timer.AutoReset = true;
		_timer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		// Hélice
		angle -= Speed;
		VisTourne.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

		// Pop
		if (_pop) {
			GameObject poped = (GameObject) Instantiate(PopObject);
			poped.transform.position = PopTransform.position;
			poped.AddComponent<Rigidbody2D>();
			poped.GetComponent<Reshape>().CurrentShape = Random.Range(0, 1);
			_pop = false;
		}

		// Door
		if (_openDoor) {
			Door.transform.rotation = Quaternion.Lerp(Door.transform.rotation, Quaternion.Euler(new Vector3(0,0,0)), Time.deltaTime * 2.0f);
			Door.transform.position = Vector2.Lerp(Door.transform.position, new Vector2(76.0f, -0.5f), Time.deltaTime * 2.0f);
		}
	}
}
