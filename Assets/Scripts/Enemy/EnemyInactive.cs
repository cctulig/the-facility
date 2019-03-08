using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInactive : State {

	public Transform playerPos;

	void Awake() {
		playerPos = Player.self.transform;

		setup ("Inactive");
		setup ("Default");
	}

	void OnEnable() {
		StartCoroutine (SearchingForPlayer());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator SearchingForPlayer() {
		while ((playerPos.position - transform.position).magnitude > 40) {
			yield return new WaitForSeconds (1.0f);
		}
		sm.toState ("Idle");
	}
}
