using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : State {

    /*info
     *[0] = momentum (max 3000, 1000 per level)
     * 
    */
    private float moveSpeed = Movement.PlayerAcc;
    private float maxMoveSpeed;
    

    public bool attackQueued = false;

    private Rigidbody2D C_rigidbody2D;


    //components

    //private StateMachine sm;

    void Awake() {
        C_rigidbody2D = GetComponent<Rigidbody2D>();
        setup("Default");
    }
    

    void Update() {
        
        if (Input.GetMouseButtonDown(0) || attackQueued) {  //if attacking, switch to attacking state
            sm.toState("Attack", 10);
            attackQueued = false;
        }
        if (Input.GetKeyDown(Movement.dash)) {              //if dashing, switch to dashing state
            sm.toState("Dash");
        }
    }


    void FixedUpdate() {

        movementInputs();
    }


    //sub functions




    //movement options

    void movementInputs() { //move using wasd
        
        C_movement.push(Movement.wasd() * moveSpeed);
        
    }
    
    
    public void queueAttack() {
        attackQueued = true;
    }
    //getters and setters

    public void setMovement( float spd, float maxSpd ) {
        moveSpeed = spd;
        maxMoveSpeed = maxSpd;
    }
}





  