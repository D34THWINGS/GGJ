using UnityEngine;
using System.Collections;

public class Reshape : MonoBehaviour {
	public int OriginShape = 0;
	public Vector3 OriginSize;
	public PhysicsMaterial2D Glass;

	public int CurrentShape {
		get {
			return _currentShape;
		}
		set {
			if (value == _currentShape) return;

			if (value == 0) {
				// Morph to square
				if (_currentShape == 1) {
					Destroy(GetComponent<CircleCollider2D>());
				} else if (_currentShape == 2) {
					// Collider for triangle
				}
				var box = gameObject.AddComponent<BoxCollider2D>();
				box.sharedMaterial = material;
			} else if (value == 1) {
				// Morph to circle
				if (_currentShape == 0) {
					Destroy(GetComponent<BoxCollider2D>());
				} else if (_currentShape == 2) {
					// Collider for triangle
				}
				var box = gameObject.AddComponent<CircleCollider2D>();
				box.sharedMaterial = Glass;
			} else {
				return;
			}
			PreviousShape = _currentShape;
			_currentShape = value;
		}
	}
	public int PreviousShape { get; private set;}

	private int _currentShape = 0;
	private Animator animator;
	private PhysicsMaterial2D material;

	void Start () {
		CurrentShape = OriginShape;
		OriginSize = transform.localScale;
		animator = GetComponent<Animator>();
		material = collider2D.sharedMaterial;
	}

	void Update () {		
		animator.SetInteger("Shape", CurrentShape);
		animator.SetInteger("PreviousShape", PreviousShape);	
	}
}
