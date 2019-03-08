using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeeking : State {

	private Enemy C_enemy;

	void Awake() {
		C_enemy = GetComponent<Enemy> ();

		setup ("Seeking");
	}

	void OnEnable() {
		//StartCoroutine (SeekingPlayer());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*IEnumerator SeekingPlayer() {

	}*/
}
