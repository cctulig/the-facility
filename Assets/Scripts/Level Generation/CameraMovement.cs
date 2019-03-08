using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform C_player;

	public float mouseX, mouseY, playerX, playerY, camX, camY, offsetX, offsetY;
	float goalX, goalY;
	float clampDistance = 8;
	public Vector3 clamp;
	public Vector3 moveCam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		camX = gameObject.transform.position.x;
		camY = gameObject.transform.position.y;
        if (C_player != null) {
            playerX = C_player.position.x;
            playerY = C_player.position.y;
        }
		mouseX = Input.mousePosition.x - Screen.width/2;
		mouseY = Input.mousePosition.y - Screen.height/2;

		clamp = new Vector2 (mouseX / 20, mouseY / 20);

		goalX = Vector2.ClampMagnitude(clamp, clampDistance).x + playerX;
		goalY = Vector2.ClampMagnitude(clamp, clampDistance).y + playerY;

		offsetX = goalX - camX;
		offsetY = goalY - camY;

		moveCam = new Vector3 (offsetX, offsetY, 0) / 5;
		gameObject.transform.Translate (moveCam * Time.deltaTime/.02f);
	}
}