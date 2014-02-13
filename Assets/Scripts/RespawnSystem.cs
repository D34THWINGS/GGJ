using UnityEngine;
using System.Collections;

public class RespawnSystem : MonoBehaviour {
	
	public Transform Spawn;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if(collider.name == "Player") {
			print ("FUCK");
			collider.gameObject.transform.position = Spawn.position;
			collider.rigidbody2D.velocity = Vector2.zero;
		}
	}
}
