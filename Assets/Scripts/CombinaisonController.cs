using UnityEngine;
using System.Collections.Generic;

public class CombinaisonController : MonoBehaviour {

	public List<int> Combinaison;
	public delegate void CombinaisonDelegate();
	public event CombinaisonDelegate OnCombinaisonFull;
	public bool Enabled = true;

	private int _nbOfValid = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!Enabled) return;

		var resh = collider.gameObject.GetComponent<Reshape>();
		if (resh == null) return;

		if (_nbOfValid == Combinaison.Count || resh.CurrentShape != Combinaison[_nbOfValid]){ 
			Destroy(collider.gameObject); 
		} else {
			_nbOfValid++;
		}

		if (_nbOfValid == Combinaison.Count && OnCombinaisonFull != null) {
			Enabled = false;
			OnCombinaisonFull();
		}
	}
}
