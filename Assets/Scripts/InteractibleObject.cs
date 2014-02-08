﻿using UnityEngine;
using System.Collections;
using System.Timers;

public class InteractibleObject : MonoBehaviour {
	public enum InteractionType {
		PERMANENT,
		TIMED,
		INSTANT
	};
	public enum InteractionEvent {
		RESHAPE,
		HIDE,
		SHOW,
		KILL,
		RESURECT,
		RESCALE,
		WEIGHT_CHANGE
	};

	public InteractionType Interaction;
	public float Timer;
	public bool IsReshapable;
	public bool IsInvisible;
	public bool IsKillable;
	public bool IsWeightChangeable;
	public bool IsHeavy = false;
	public GameObject WeightMessage;
	public float RescaleSpeed = 5f;

	public delegate void DelegateInteraction(InteractionEvent iEvent, GameObject sender);
	public event DelegateInteraction OnStateChange;

	[HideInInspector]
	public bool IsDead { 
		get {
			return isDead;
		}
		set {
			isDead = value;
			GetComponent<Animator>().SetBool("IsDead", value);
			collider2D.isTrigger = value;
		}
	}

	private Reshape reshape;
	private bool timerElapsed;
	private Timer timer;
	private Vector2 targetScale;
	private bool isDead;

	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetBool("Hidden", false);
		gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		targetScale = transform.localScale;

		if(IsReshapable){
			reshape = gameObject.GetComponent<Reshape>();
			if(reshape == null){
				Debug.LogError("No Reshape Component");
			}
		}
		if(IsInvisible){
			GetComponent<Animator>().SetBool("Hidden", true);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		if(IsWeightChangeable){
			if (rigidbody2D == null)
				gameObject.AddComponent("Rigidbody2D");
			rigidbody2D.mass = IsHeavy ? StaticVariables.HeavyWeight : StaticVariables.LightWeight;
		}
		timerElapsed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Interaction == InteractionType.TIMED && timerElapsed) {
			Unteract();
			timerElapsed = false;
		}

		if (transform.localScale != (Vector3) targetScale) {
			transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * RescaleSpeed);
		}
	}
	
	public void Interact (GameObject player) {
		if(IsReshapable){
			var playerControler = player.GetComponent<CharacterControl>();
			reshape.CurrentShape = playerControler.Reshaper.CurrentShape;

			if(Interaction == InteractionType.TIMED){
				if (timer != null) {
					timer.Stop ();
				}
				timer = new System.Timers.Timer(Timer);
				timer.Elapsed += (sender, args) => {
					timerElapsed = true;
					timer.Stop();
				};
				timer.AutoReset = false;
				timer.Start();
			}
			if (OnStateChange != null) {
				OnStateChange(InteractionEvent.RESHAPE, gameObject);
			}
		}
		if(IsInvisible && StaticVariables.HasPower(StaticVariables.Powers.REVEAL)){
			GetComponent<Animator>().SetBool("Hidden", false);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			if (OnStateChange != null) {
				OnStateChange(InteractionEvent.SHOW, gameObject);
			}
		}
		if(IsKillable){
			IsDead = player.GetComponent<CharacterControl>().IsDead;
			if (OnStateChange != null) {
				if (IsDead)
					OnStateChange(InteractionEvent.KILL, gameObject);
				else
					OnStateChange(InteractionEvent.RESURECT, gameObject);
			}
		}
		if(IsWeightChangeable && StaticVariables.HasPower(StaticVariables.Powers.CHANGE_WEIGHT)){
			if (gameObject.rigidbody2D.mass == player.rigidbody2D.mass) return;
			gameObject.rigidbody2D.mass = player.rigidbody2D.mass;
			if(player.rigidbody2D.mass == StaticVariables.LightWeight){
				WeightMessage.GetComponent<TextMesh>().text = "Soft";
			}else{
				WeightMessage.GetComponent<TextMesh>().text = "Heavy";
			}
			Instantiate(WeightMessage, gameObject.transform.position, Quaternion.identity);			
			if (OnStateChange != null) {
				OnStateChange(InteractionEvent.WEIGHT_CHANGE, gameObject);
			}
		}
	}
	
	public void Unteract(){
		if (Interaction != InteractionType.INSTANT) return;
		if(IsReshapable){
			reshape.CurrentShape = reshape.OriginShape;
		}
		if(IsInvisible){
			GetComponent<Animator>().SetBool("Hidden", true);
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
		if(IsKillable){
			IsDead = false;
		}
		if(IsWeightChangeable){
			if(IsWeightChangeable)
				gameObject.rigidbody2D.mass = StaticVariables.HeavyWeight;
			else
				gameObject.rigidbody2D.mass = StaticVariables.LightWeight;
		}
	}
}
