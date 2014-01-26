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

		if (Input.GetKeyDown(KeyCode.Space)) {
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

		if (Input.GetKeyDown(KeyCode.Q) && Reshaper.CurrentShape != 1) {
			Reshaper.PreviousShape = Reshaper.CurrentShape;
			Reshaper.CurrentShape = 1;
			print ("To Circle !");
		}
		if (Input.GetKeyDown(KeyCode.E) && Reshaper.CurrentShape != 0) {
			Reshaper.PreviousShape = Reshaper.CurrentShape;
			Reshaper.CurrentShape = 0;
			print ("To Square !");
		}

		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -CameraDistance);
	}

	void OnCollisionEnter2D (Collision2D theCollision){
		foreach(ContactPoint2D contact in theCollision.contacts) {
			if(contact.normal.y >= 1.0f) {
				IsGrounded = true;
				GroundedOn = theCollision.gameObject;
			}
			if (contact.normal.x <= -1.0f) {
				print(contact.normal.x);
				CollidesRight = true;
				_collider = theCollision.gameObject;
			}
			if (contact.normal.x >= 1.0f) {
				print(contact.normal.x);
				CollidesLeft = true;
				_collider = theCollision.gameObject;
			}
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
