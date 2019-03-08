using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour {

	KeyCode nextLevel = KeyCode.N;

	public GameObject Control;

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("Worked!");
			if (Input.GetKey (nextLevel)) {
				Control.GetComponent<Controller> ().refreshZone ();
			}
		}
	}
}
