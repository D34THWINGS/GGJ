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
				// Square
			} else if (value == 1) {
			// Circle
			} else {
				return;
			}

			_currentShape = value;
		}
	}
	public int PreviousShape {
		get {
			return _previousShape;
		}
		set {
			_previousShape = value;
		}
	}

	private int _currentShape;
	private int _previousShape;
	private Animator animator;
	
	void Start () {
		animator = GetComponent<Animator>();	
	}

	void Update () {		
		animator.SetInteger("Shape", CurrentShape);
		animator.SetInteger("PreviousShape", PreviousShape);	
	}
}
