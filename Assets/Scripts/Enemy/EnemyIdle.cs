using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : State {

	public Transform playerPos;

	void Awake() {
		playerPos = Player.self.transform;

		setup ("Idle");
	}

	void OnEnable() {
		StartCoroutine (SearchingForPlayerLoS());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SearchingForPlayerLoS() {
		bool search = true;
		int layerMask = 1 << 11;
		layerMask = ~layerMask;

		while (search) {
			RaycastHit2D hit = Physics2D.Raycast(transform.position, playerPos.position - transform.position, 20f, layerMask);
			if (hit && hit.collider.CompareTag ("Player")) {
				sm.toState ("Combat");
				search = false;
			} else if ((playerPos.position - transform.position).magnitude > 40) {
				sm.toState ("Inactive");
				search = false;
			}
			yield return new WaitForSeconds (1.0f);
		}
	}
}