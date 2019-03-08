using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : State {

	public Transform playerPos;
	private Enemy C_enemy;

	void Awake() {
		playerPos = Player.self.transform;
		C_enemy = GetComponent<Enemy> ();

		setup ("Combat");
	}

	void OnEnable() {
		StartCoroutine (MovingToPlayer());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator MovingToPlayer() {
		bool hasLoS = true;
		Vector2 dirToPlayer;
		Vector2 lastPlayerPos = playerPos.position;

		int layerMask = 1 << 11;
		layerMask = ~layerMask;

		while (hasLoS) {
			dirToPlayer = (playerPos.position - transform.position).normalized;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, playerPos.position - transform.position, 20f, layerMask);

			if (hit && hit.collider.CompareTag ("Player")) {
				C_movement.push (dirToPlayer * Movement.enemyAcc);
				lastPlayerPos = playerPos.position;
			} 
			else {
				C_enemy.setLastPlayerPos (lastPlayerPos);
				sm.toState ("Seeking");
				hasLoS = false;
			}
			yield return new WaitForSeconds (0.25f);
		}
	}
}
