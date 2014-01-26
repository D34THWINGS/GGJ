using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {
	public float JumpSpeed = 12.0f;
	public float MaxSpeed = 10.0f;
	public float CameraDistance = 10.0f;

	public bool IsGrounded { get; private set; }
	public bool CollidesLeft { get; private set; }
	public bool CollidesRight { get; private set; }
	public GameObject GroundedOn { get; private set; }
	public Reshape Reshaper { get; private set; }

	private bool _doubleJump = true;
	private GameObject _collider;

	void Start () {
		Reshaper = GetComponent<Reshape>();
		IsGrounded = false;
		CollidesLeft = false;
		CollidesRight = false;
	}

	void Update () {
		var move_x = Input.GetAxis("Horizontal");
		var move = new Vector2(move_x * MaxSpeed, rigidbody2D.velocity.y);

		if (IsGrounded) {
			_doubleJump = true;
		}

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) {
			if (IsGrounded || (!IsGrounded && _doubleJump)) {
				move.y = JumpSpeed;

				if (!IsGrounded) {
					_doubleJump = false;
				}
			}
		}

		if ((CollidesLeft && move.x < 0.0f) || (CollidesRight && move.x > 0.0f)) {
			move.x = 0.0f;
		}

		rigidbody2D.velocity = move;
		Camera.main.orthographicSize = CameraDistance;
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
	}

	void OnCollisionEnter2D (Collision2D theCollision){
		foreach(ContactPoint2D contact in theCollision.contacts) {
			if(contact.normal.y >= 0.5f) {
				IsGrounded = true;
				GroundedOn = theCollision.gameObject;
			}
			if (contact.normal.x <= -0.5f) {
				CollidesRight = true;
				_collider = theCollision.gameObject;
			}
			if (contact.normal.x >= 0.5f) {
				CollidesLeft = true;
				_collider = theCollision.gameObject;
			}
		}

		if(theCollision.gameObject.name == "EndDoor"){
			theCollision.gameObject.GetComponent<ChangeLevel>().ChangeScene();
		}
	}
	
	void OnCollisionExit2D(Collision2D theCollision){
		if (theCollision.gameObject == GroundedOn) {
			GroundedOn = null;
			IsGrounded = false;
		}
		if (theCollision.gameObject == _collider) {
			CollidesLeft = false;
			CollidesRight = false;
			_collider = null;
		}
	}
}
