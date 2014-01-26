using UnityEngine;
using System.Collections;

public class Reshape : MonoBehaviour {
	public int CurrentShape {
		get {
			return _currentShape;
		}
		set {
			if (value == _currentShape) return;

			if (value == 0) {
				if (_currentShape == 1) {
					Destroy(GetComponent<CircleCollider2D>());
				} else if (_currentShape == 2) {

				}
				var box = gameObject.AddComponent<BoxCollider2D>();
				//box.size = new Vector2(1.0f, 1.0f);
			} else if (value == 1) {
				if (_currentShape == 0) {
					Destroy(GetComponent<BoxCollider2D>());
				} else if (_currentShape == 2) {
					
				}
				var box = gameObject.AddComponent<CircleCollider2D>();
				//box.size = new Vector2(1.0f, 1.0f);
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
	
	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update () {		
		animator.SetInteger("Shape", CurrentShape);
		animator.SetInteger("PreviousShape", PreviousShape);	
	}
}
