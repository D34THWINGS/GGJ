using UnityEngine;
using System.Collections;

public class BunnyBlock : MonoBehaviour {

	public int DestroyTime = 2;
	public int RespawnTime = 5;

	private Transform origin;


	void OnCollisionEnter2D(Collision2D theCollision) {
		if(theCollision.gameObject.name == "Player") {
			StartCoroutine(DestroyBlock());
			StartCoroutine(CreateBlock());
		}
	}
	
	private IEnumerator DestroyBlock() {
		yield return new WaitForSeconds(DestroyTime);
		ChangeStatus(false);
	}
	
	private IEnumerator CreateBlock() {
		yield return new WaitForSeconds(RespawnTime);
		ChangeStatus(true);
	}
	
	private void ChangeStatus(bool status) {
		gameObject.collider2D.enabled = status;
		gameObject.transform.parent.GetComponent<Collider2D>().enabled = status;
		gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled = status;
	}
}
