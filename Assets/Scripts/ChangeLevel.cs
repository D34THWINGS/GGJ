using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {
	public int NextLevel = 0;

	public void ChangeScene () {
		Application.LoadLevel(NextLevel);
	}
}
