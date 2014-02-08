using UnityEngine;
using System.Collections;
using System.Timers;

public class CharacterControl : MonoBehaviour {
	[HideInInspector]
	public bool Jump = false;				// Condition for whether the player should jump.
	[HideInInspector]	
	public bool IsDead = false;				// If player is dead or not
	[HideInInspector]
	public Reshape Reshaper;				// Public accessor/setter for reshping component
	
	public float MoveForce = 365f;			// Amount of force added to move the player left and right.
	public float MaxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float JumpForce = 1000f;			// Amount of force added when the player jumps.
	public float CameraDistance = 8f;		// Camera distance from player.
	public float RespawnTime = 10000f;		// Camera distance from player.

	// Grounding
	[HideInInspector]
	public bool IsGrounded = false;
	public Transform GroundTransform;
	public float GroundRadius = 0.3f;
	public LayerMask GroundLayers;

	private bool doubleJump = true;
	private Timer respawnTimer;

	void Start () {
		Reshaper = GetComponent<Reshape>();
	}

	void FixedUpdate () {
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		if(Mathf.Sign(h) != Mathf.Sign(rigidbody2D.velocity.x) && h != 0) {
			rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
		}

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < MaxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * MoveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > MaxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * MaxSpeed, rigidbody2D.velocity.y);
		
		// If the player should jump...
		if(Jump)
		{
			// Add a vertical force to the player.
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
			rigidbody2D.AddForce(new Vector2(0f, JumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			Jump = false;
		}
	}

	void Update () {
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		IsGrounded = Physics2D.OverlapArea(
			new Vector2(GroundTransform.position.x - 0.48f, GroundTransform.position.y - 0.1f), 
			new Vector2(GroundTransform.position.x + 0.48f, GroundTransform.position.y + 0.1f),
			GroundLayers
			);

		// If the player is grounded, reset the double jump
		if (IsGrounded) {
			doubleJump = false;
		}
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if (Input.GetButtonDown("Jump") && (IsGrounded || !doubleJump)) {
			Jump = true;
			if (!IsGrounded)
				doubleJump = true;
		}

		// Death effects
		Camera.main.GetComponent<MotionBlur>().enabled = IsDead;
		Camera.main.GetComponent<GrayscaleEffect>().enabled = IsDead;

		Camera.main.orthographicSize = CameraDistance;
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
	}

	void OnCollisionEnter2D (Collision2D theCollision){

		if (theCollision.gameObject.tag == "Deadly" && !IsDead) {
			IsDead = true;
			respawnTimer = new Timer(RespawnTime);
			respawnTimer.Elapsed += (sender, e) => {
				IsDead = false;
				respawnTimer.Stop();
			};
			respawnTimer.AutoReset = false;
			respawnTimer.Start();
		}
	}
}
