using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State {

    int distance = 4;
    Vector2 direction;

    int traveled = 0;
    bool touchingWall;

    public ContactPoint2D[] contacts = new ContactPoint2D[10];
    //ContactPoint2D[] prevColliders = new ContactPoint2D[10];

    BoxCollider2D C_collider;
    Rigidbody2D C_body;

    ContactFilter2D filter = new ContactFilter2D();
    public Collider2D[] colliders = new Collider2D[10];
    Collider2D[] prevColliders = new Collider2D[10];
    int number = 0;
    int prevNumber = 0;

    int state = 0;

    private void Awake() {
        
        setup("Dash");

        colliders = new Collider2D[10];
        C_collider = GetComponent<BoxCollider2D>();
        C_body = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        number = 0;
        C_collider.isTrigger = true;
        direction = Util.mousePos().normalized;
        traveled = 0;
        C_hp.invulnerable = true;
        StartCoroutine("dash");

    }
    

    IEnumerator dash() {
        Debug.Log("starting Dash corotine");

        for (float u = 0; u < .3; u += Time.deltaTime) { 
            Debug.Log(u);
            checkCollisions();
            
            //C_movement.push(direction * 100);
            //C_movement.clampSpeed();

            if (newCollision()) {   //if there is a new collision (if the player hits an enemy), slash

                yield return new WaitForFixedUpdate();

                //Time.timeScale = .1f;
                //Time.fixedDeltaTime = .1f * 0.02f;
                
                SendMessage("createSlashAttack");

                for (float y = 0; y < .05; y += Time.deltaTime) {

                    //C_movement.push(direction * 100);
                    checkCollisions();
                    checkForWall();
                    yield return new WaitForFixedUpdate();
                    u += Time.deltaTime;
                }

                //Time.timeScale = 1f;
                //Time.fixedDeltaTime = 1f * 0.02f;

                C_movement.push(direction * 100);
                //C_movement.clampSpeed();

            }

            yield return new WaitForFixedUpdate();

            checkForWall(); //exits if the player hits a wall
        }


        sm.toState("Default");

    }

    void checkForWall() {

        for (int i = 0; i < number; i++) {  //checks if colliding with a wall
            if(colliders[i] != null) {
                if (colliders[i].gameObject.tag == "Wall") {
                    sm.toState("Default");
                    StopCoroutine("dash");
                }
            }
        }
    }

    void OnDisable() {
        //C_movement.clampSpeed(0);
        Time.timeScale = 1;
        C_hp.invulnerable = false;
        C_collider.isTrigger = false;

    }
    

    bool newCollision() {
        if (number <= prevNumber) {
            Debug.Log("same");
            return false;
        }else {
            Debug.Log("different");
            return true;
        }
    }

    void checkCollisions() {
        prevNumber = number;
        number = C_collider.OverlapCollider(filter, colliders);
    }
    
}
