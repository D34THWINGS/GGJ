using UnityEngine;
using System.Collections;
using System.Timers;
using XRay.UI;
using XRay.Player;

namespace XRay.Objects {

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
		
		public InteractionType Type;
		public float InteractionTimer;
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
		private PhysicsMaterial2D originMaterial;
		private Coroutine timedCoroutine;
		
		// Use this for initialization
		void Start () {
			GetComponent<Animator>().SetBool("Hidden", false);
		    var boxCollider = gameObject.GetComponent<BoxCollider2D>();
			if (boxCollider != null)
                boxCollider.isTrigger = false;
            var circleCollider = gameObject.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
                circleCollider.isTrigger = false;
			targetScale = transform.localScale;
			originMaterial = collider2D.sharedMaterial;
			
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
			if(Type == InteractionType.INSTANT){
				gameObject.GetComponent<SpriteRenderer>().color =new Color(1,0,0);
			}
		}
		
		// Update is called once per frame
		void Update () {
			if (Type == InteractionType.TIMED && timerElapsed) {
				Unteract();
				timerElapsed = false;
			}
			
			if (transform.localScale != (Vector3) targetScale) {
				transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * RescaleSpeed);
			}
		}
		
		public void Interact (GameObject player) {
			if (reshape.CurrentShape != 1) {
				collider2D.sharedMaterial = player.collider2D.sharedMaterial;
			}
			if(IsReshapable){
				var playerControler = player.GetComponent<CharacterControl>();
				reshape.CurrentShape = playerControler.Reshaper.CurrentShape;
				
				// Fire state change event
				if (OnStateChange != null) {
					OnStateChange(InteractionEvent.RESHAPE, gameObject);
				}
			}
			if(IsInvisible && StaticVariables.HasPower(StaticVariables.Powers.Reveal)){
				GetComponent<Animator>().SetBool("Hidden", false);
				gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
				
				// Fire state change event
				if (OnStateChange != null) {
					OnStateChange(InteractionEvent.SHOW, gameObject);
				}
			}
			if(IsKillable){
				IsDead = player.GetComponent<CharacterControl>().IsDead;
				
				// Fire state change event
				if (OnStateChange != null) {
					if (IsDead)
						OnStateChange(InteractionEvent.KILL, gameObject);
					else
						OnStateChange(InteractionEvent.RESURECT, gameObject);
				}
			}
			if(IsWeightChangeable && StaticVariables.HasPower(StaticVariables.Powers.ChangeWeight)){
				// Do nothing if the wieght hasn't change
				if (rigidbody2D.mass == player.rigidbody2D.mass) return;
				
				// Apply new weight
				rigidbody2D.mass = player.rigidbody2D.mass;
				
				// Is object is not mmoving apply a little force to recalculate physics
				if (rigidbody2D.velocity == Vector2.zero) {
					rigidbody2D.AddForce(new Vector2(0f, 0.1f));
				}
				
				// Display weight change text
				if(player.rigidbody2D.mass == StaticVariables.LightWeight){
					WeightMessage.GetComponent<TextMesh>().text = "Soft";
				}else{
					WeightMessage.GetComponent<TextMesh>().text = "Heavy";
				}
				Instantiate(WeightMessage, gameObject.transform.position, Quaternion.identity);
				
				// Fire weight change event
				if (OnStateChange != null) {
					OnStateChange(InteractionEvent.WEIGHT_CHANGE, gameObject);
				}
			}
			
			// Timed change
			if (Type == InteractionType.TIMED) {
				StopCoroutine("TimedEndInteraction");
				timedCoroutine = StartCoroutine("TimedEndInteraction", InteractionTimer);
			}
		}
		
		public void Unteract(){
			if (Type != InteractionType.INSTANT) return;
			EndInteraction();
		}
		
		protected void EndInteraction() {
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
		
		public IEnumerator TimedEndInteraction(float waitTime) {		
			// Wait for respawn time
			yield return new WaitForSeconds(waitTime / 1000);
			
			EndInteraction();
		}
	}
}
