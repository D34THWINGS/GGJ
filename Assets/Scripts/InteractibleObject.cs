using UnityEngine;
using System.Collections;
using System.Timers;

public class InteractibleObject : MonoBehaviour {
	public enum InteractionType {
		PERMANENT,
		TIMED,
		INSTANT
	}
	public InteractionType Interaction;
	public float Timer;
	public bool isTransformable;
	public bool isInvisible;
	public bool isKillable;
	public bool MassController;

	public PhysicsMaterial2D Glass;
	
	private Reshape _reshape;
	private bool _timerElapsed;
	private Timer _timer;
	
	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetBool("Hidden", false);
		gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		Debug.Log(isTransformable);
		if(isTransformable){
			_reshape = gameObject.GetComponent<Reshape>();
			if(_reshape == null){
				Debug.LogError("No Reshape Component");
			}
		}
		if(isInvisible){
			GetComponent<Animator>().SetBool("Hidden", true);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		if(isKillable){
			
		}
		if(MassController){
			gameObject.AddComponent("Rigidbody2D");
			//gameObject.rigidbody2D.
		}
		_timerElapsed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Interaction == InteractionType.TIMED && _timerElapsed) {
			DoDetransform();
			_timerElapsed = false;
		}
	}
	
	public void Interact (GameObject player) {
		if(isTransformable){
			DoTransform(player);

			if(Interaction == InteractionType.PERMANENT){
				// Rescaling is working only for permanent change
				if(gameObject.transform.localScale != player.transform.localScale){
					gameObject.transform.localScale = player.transform.localScale;
					Vector3 temp = gameObject.transform.localPosition;
					//temp.x = gameObject.transform.localPosition.x - ((_reshape.OriginSize.x/2f)-(player.transform.localScale.x/2f));
					temp.y = gameObject.transform.localPosition.y - ((_reshape.OriginSize.y/2f)-(player.transform.localScale.y/2f));
					gameObject.transform.localPosition = temp;
				}
			}
			if(Interaction == InteractionType.TIMED){
				if (_timer != null) {
					_timer.Stop ();
				}
				_timer = new System.Timers.Timer(Timer);
				_timer.Elapsed += (sender, args) => {
					_timerElapsed = true;
					_timer.Stop();
				};
				_timer.AutoReset = false;
				_timer.Start();
			}
		}
		if(isInvisible){
			GetComponent<Animator>().SetBool("Hidden", false);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		}
		if(isKillable){
			
		}
	}

	private void DoTransform (GameObject player) {
		var resh = player.GetComponent<Reshape>();
		_reshape.CurrentShape = resh.CurrentShape;
		collider2D.sharedMaterial = Glass;

		if(MassController){
			var playerControler = player.GetComponent<CharacterControl>();
			if(playerControler.JumpSpeed>=18){
				gameObject.rigidbody2D.mass = 2;
			}else{
				gameObject.rigidbody2D.mass = 10;
			}
		}
	}
	
	public void Unteract(GameObject player){
		if(isTransformable){
			if(Interaction == InteractionType.INSTANT){
				DoDetransform();
			}
		}
		if(isInvisible){
			GetComponent<Animator>().SetBool("Hidden", true);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		if(isKillable){
			
		}
	}

	private void DoDetransform () {
		_reshape.CurrentShape = _reshape.OriginShape;
		collider2D.sharedMaterial = Glass;
	}
}
