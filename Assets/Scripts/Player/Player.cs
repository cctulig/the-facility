using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static GameObject self;



	private HP C_HP;
    private Movement C_movement;
    private PlayerMovement C_playerMovement;

	void Awake() {
        self = gameObject;
	}

    // Use this for initialization
    void Start () {
    // components
		C_HP = GetComponent<HP> ();
        C_movement = GetComponent<Movement>();
        C_playerMovement = GetComponent<PlayerMovement>();
    //hp
        C_HP.setHP(6);
        
	}
    

}
