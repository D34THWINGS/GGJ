using UnityEngine;
using System.Collections;

public class WeightMsgAnim : MonoBehaviour {

	private float time;
	public int Long;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		gameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y+0.1f, transform.localPosition.z);
		if(time>=Long){
			Destroy(gameObject);
		}
	}
}
