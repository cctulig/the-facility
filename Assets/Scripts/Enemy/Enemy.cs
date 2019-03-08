using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private HP C_HP;
    private Movement C_movement;

	private Vector2 lastPlayerPos;

    void Awake() {
        //DontDestroyOnLoad (this);
    }

    // Use this for initialization
    void Start() {
        // components
        C_HP = GetComponent<HP>();
        C_movement = GetComponent<Movement>();
        //hp
        C_HP.setHP(3);
        
    }

	public void setLastPlayerPos(Vector2 pos) {
		lastPlayerPos = pos;
	}

	public Vector2 getLastPlayerPos() {
		return lastPlayerPos;
	}
}
