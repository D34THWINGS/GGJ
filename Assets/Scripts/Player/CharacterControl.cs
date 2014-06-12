using UnityEngine;
using System.Collections;
using XRay.Objects;
using XRay.UI;

namespace XRay.Player {
    public class CharacterControl : MonoBehaviour {
        [HideInInspector] public bool Jump = false; // Condition for whether the player should jump.
        [HideInInspector] public bool IsDead = false; // If player is dead or not
        [HideInInspector] public Reshape Reshaper; // Public accessor/setter for reshping component

        public delegate void DieDelegate();
        public event DieDelegate OnDie;

        public float MoveForce = 365f; // Amount of force added to move the player left and right.
        public float MaxSpeed = 5f; // The fastest the player can travel in the x axis.
        public float JumpForce = 1000f; // Amount of force added when the player jumps.
        public float CameraDistance = 8f; // Camera distance from player.
        public float RespawnTime = 10000f; // Camera distance from player.

        // Grounding
        [HideInInspector] public Collider2D IsGrounded; // Condition whether the player is on the ground
        public Transform GroundTransform; // Position of the ground detector
        public float GroundRadius = 0.3f; // Radius of the ground detector
        public LayerMask GroundLayers; // Layers which are considered as ground

        public GameObject Cone;

        private bool _doubleJump = true;

        public void Start() {
            Reshaper = GetComponent<Reshape>();
        }

        public void FixedUpdate() {
            if (!StaticVariables.CantMove) {
                // Cache the horizontal input.
                var h = Input.GetAxis("Horizontal");

                if (!Mathf.Sign(h).Equals(Mathf.Sign(rigidbody2D.velocity.x)) && !h.Equals(0f)) {
                    rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
                }

                // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
                if (h*rigidbody2D.velocity.x < MaxSpeed) {
                    if (IsGrounded)
                        rigidbody2D.AddForce(Vector2.right*h*MoveForce);
                    else
                        rigidbody2D.AddForce(Vector2.right*h*(MoveForce/10));
                }

                // If the player's horizontal velocity is greater than the maxSpeed...
                if (Mathf.Abs(rigidbody2D.velocity.x) > MaxSpeed)
                    // ... set the player's velocity to the maxSpeed in the x axis.
                    rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x)*MaxSpeed,
                                                       rigidbody2D.velocity.y);

                // If the player should jump...
                if (Jump) {
                    // Add a vertical force to the player.
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
                    rigidbody2D.AddForce(new Vector2(0f, JumpForce));

                    // Make sure the player can't jump again until the jump conditions from Update are satisfied.
                    Jump = false;
                }

                if (StaticVariables.HasPower(StaticVariables.Powers.Vision)) {
                    Cone.SetActive(true);
                }
            }
        }

        public void Update() {
			if (StaticVariables.CantMove) return;

            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            IsGrounded = Physics2D.OverlapArea(
                new Vector2(GroundTransform.position.x - 0.48f, GroundTransform.position.y - 0.1f),
                new Vector2(GroundTransform.position.x + 0.48f, GroundTransform.position.y + 0.1f),
                GroundLayers
                );

            // If the player is grounded, reset the double jump
            if (IsGrounded) {
                _doubleJump = false;
            }

            // If the jump button is pressed and the player is grounded then the player should jump.
            if (Input.GetButtonDown("Jump") && (IsGrounded || !_doubleJump)) {
                Jump = true;
                if (!IsGrounded)
                    _doubleJump = true;
            }

            // Death effects
            var mb = Camera.main.GetComponent<MotionBlur>();
            if (mb)
                mb.enabled = IsDead;
            var gse = Camera.main.GetComponent<GrayscaleEffect>();
            if (gse)
                gse.enabled = IsDead;

            // Apply camera distance
            Camera.main.orthographicSize = CameraDistance;
        }

        public void OnCollisionEnter2D(Collision2D theCollision) {
            if (theCollision.gameObject.tag == "Deadly" && !IsDead) {
                // Kill player
                IsDead = true;

                // Start the respawn timer
                StartCoroutine(Respawn(RespawnTime));

                // Cast die event
                if (OnDie != null)
                    OnDie();
            }
        }

        public void Teleport(Transform point) {}

        /// <summary>
        /// Respawn the player at the specified waitTime.
        /// </summary>
        /// <param name="waitTime">Wait time.</param>
        public IEnumerator Respawn(float waitTime) {
            // Wait for respawn time
            yield return new WaitForSeconds(waitTime/1000);

            // Respawn
            IsDead = false;
        }

        /// <summary>
        /// Get the parent object of the object.
        /// </summary>
        /// <param name="childObject">The child object.</param>
        /// <returns>The parent game object.</returns>
        private GameObject GetParent(GameObject childObject) {
            if (childObject == null)
                return null;
            return childObject.transform.parent != null ? childObject.transform.parent.gameObject : null;
        }
    }
}