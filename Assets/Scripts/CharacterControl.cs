using UnityEngine;
using System.Collections;
using System.Timers;

public class CharacterControl : MonoBehaviour {
	public float JumpSpeed = 12.0f;
	public float MaxSpeed = 10.0f;
	public float CameraDistance = 10.0f;
	public float RespawnTime = 2000.0f;

	// Grounding
	public bool IsGrounded { get; private set; }
	public Transform GroundTransform;
	public float GroundRadius = 0.3f;
	public LayerMask GroundLayers;

	public Reshape Reshaper { get; private set; }

	private bool _doubleJump = true;
	private GameObject _collider;
	private AudioSource[] audios;

	public bool IsDead { get; private set; }
	private Timer _respawnTimer;

	void Start () {
		Reshaper = GetComponent<Reshape>();
		IsGrounded = false;
		audios = gameObject.GetComponents<AudioSource>();
		IsDead = false;
	}

	void FixedUpdate () {
		IsGrounded = Physics2D.OverlapCircle(GroundTransform.position, GroundRadius, GroundLayers);

		var move_x = Input.GetAxis("Horizontal");
		var move = new Vector2(move_x * MaxSpeed, rigidbody2D.velocity.y);

		if (IsGrounded) {
			_doubleJump = true;
		}

		var axis = new Vector2(move.x, 0);
		axis.Normalize();
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), axis, 0.6f, GroundLayers);
		if (hit.fraction != 0.0f && !hit.collider.isTrigger) {
			move.x = 0.0f;
		}
		hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), axis, 0.6f, GroundLayers);
		if (hit.fraction != 0.0f && !hit.collider.isTrigger) {
			move.x = 0.0f;
		}

		rigidbody2D.velocity = move;
	}

	void Update () {		
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) {
			if (IsGrounded || (!IsGrounded && _doubleJump)) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, JumpSpeed);
				audios[0].Play();
				if (!IsGrounded) {
					audios[1].Play();
					_doubleJump = false;
				}
			}
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
			_respawnTimer = new Timer(RespawnTime);
			_respawnTimer.Elapsed += (sender, e) => {
				IsDead = false;
				_respawnTimer.Stop();
			};
			_respawnTimer.AutoReset = false;
			_respawnTimer.Start();
		}
	}
}
