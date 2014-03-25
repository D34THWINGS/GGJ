using UnityEngine;
using System.Collections;

public class ParticleLayer : MonoBehaviour {

	public string SortingLayer = "Foreground";
	public int SortingOrder = 0;

	// Use this for initialization
	void Start () {
		particleSystem.renderer.sortingLayerName = SortingLayer;
		particleSystem.renderer.sortingOrder = SortingOrder;
	}
}
