using UnityEngine;
using System.Collections;
using System.Timers;

public class InteractibleObject : MonoBehaviour {
	[HideInInspector]
	public bool MassController;

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
	public bool isHeavy;
	public GameObject WeightMessage;
	public float RescaleSpeed = 5f;

	public delegate void DelegateInteraction(GameObject sender);
	public event DelegateInteraction OnTransformChange;
	public event DelegateInteraction OnWeightChange;
	public event DelegateInteraction OnDisplayChange;
	
	public bool IsDead { get; private set; }

	private Reshape _reshape;
	private bool _timerElapsed;
	private Timer _timer;
	private Vector2 targetScale;
	
	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetBool("Hidden", false);
		gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		targetScale = transform.localScale;

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
			if(isHeavy)
				gameObject.rigidbody2D.mass = PermanentVar.WeightHeavy;
			else
				gameObject.rigidbody2D.mass = PermanentVar.WeightSoft;
		}
		_timerElapsed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Interaction == InteractionType.TIMED && _timerElapsed) {
			DoDetransform();
			_timerElapsed = false;
		}

		if (transform.localScale != (Vector3) targetScale)
			transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * RescaleSpeed);
	}
	
	public void Interact (GameObject player) {
		if(isTransformable){
			DoTransform(player);
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
			if (OnTransformChange != null) {
				OnTransformChange(gameObject);
			}
		}
		if(isInvisible){
			if(PermanentVar.CanInvisible){
				GetComponent<Animator>().SetBool("Hidden", false);
				gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
				if (OnDisplayChange != null) {
					OnDisplayChange(gameObject);
				}
			}
		}
		if(isKillable){
			var dead = player.GetComponent<CharacterControl>().IsDead;
			IsDead = dead;
			GetComponent<Animator>().SetBool("IsDead", dead);
			collider2D.isTrigger = dead;
		}
	}

	private void DoTransform (GameObject player) {
		var playerControler = player.GetComponent<CharacterControl>();
		_reshape.CurrentShape = playerControler.Reshaper.CurrentShape;

		// Rescaling
		targetScale = player.transform.localScale;
				
		if(MassController){
			if(playerControler.JumpForce>=18){
				gameObject.rigidbody2D.mass = PermanentVar.WeightSoft;
				WeightMessage.GetComponent<TextMesh>().text = "Soft";
				Instantiate(WeightMessage, gameObject.transform.position, Quaternion.identity);
			}else{
				gameObject.rigidbody2D.mass = PermanentVar.WeightHeavy;
				WeightMessage.GetComponent<TextMesh>().text = "Heavy";
				Instantiate(WeightMessage, gameObject.transform.position, Quaternion.identity);
			}
			if (OnWeightChange != null) {
				OnWeightChange(gameObject);
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
		if(MassController){
			if(isHeavy)
				gameObject.rigidbody2D.mass = PermanentVar.WeightHeavy;
			else
				gameObject.rigidbody2D.mass = PermanentVar.WeightSoft;
		}
	}
}
