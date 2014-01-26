using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {
	public float JumpSpeed = 12.0f;
	public float MaxSpeed = 10.0f;
	public float CameraDistance = 10.0f;

	// Grounding
	public bool IsGrounded { get; private set; }
	public Transform GroundTransform;
	public float GroundRadius = 0.3f;
	public LayerMask GroundLayers;

	public Reshape Reshaper { get; private set; }

	private bool _doubleJump = true;
	private GameObject _collider;

	void Start () {
		Reshaper = GetComponent<Reshape>();
		IsGrounded = false;
	}

	void FixedUpdate () {
		IsGrounded = Physics2D.OverlapCircle(GroundTransform.position, GroundRadius, GroundLayers);

		var move_x = Input.GetAxis("Horizontal");
		var move = new Vector2(move_x * MaxSpeed, rigidbody2D.velocity.y);

		if (IsGrounded) {
			_doubleJump = true;
		}

		rigidbody2D.velocity = move;
	}

	void Update () {		
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (IsGrounded || (!IsGrounded && _doubleJump)) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, JumpSpeed);
				//rigidbody2D.AddForce(new Vector2(0, 900f));

				if (!IsGrounded) {
					_doubleJump = false;
				}
			}
		}

		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -CameraDistance);
	}

	void OnCollisionEnter2D (Collision2D theCollision){
		if(theCollision.gameObject.name == "EndDoor"){
			theCollision.gameObject.GetComponent<ChangeLevel>().ChangeScene();
		}

	}
}
