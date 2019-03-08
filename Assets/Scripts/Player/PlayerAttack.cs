using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : State {
    
    int repetitions = 0;
    float prevTime;
    float comboCooldown = .6f;

    public GameObject SlashAttack;

    // Use this for initialization
    void Awake() {
        setup("Attack");
    }

    void OnEnable() {
        createSlashAttack();
        

        if (Time.time - prevTime > comboCooldown - duration - repetitions * .2f) {
            repetitions = 0;
        }
        if (repetitions < 3) {
            repetitions++;
        }



        if (repetitions <= 2) {

            duration = .2f; //set how long the attack animation will last
        } else {
            duration = 1f;
        }


        C_movement.fadeInstant(Util.mousePos().normalized * Movement.AttackAcc, new Vector2(0, 0), duration);    //push self when attacking

    }

    void OnDisable() {
        if(repetitions >= 3) {
            repetitions = 0;
        }

        prevTime = Time.time;
    }


    // Update is called once per frame
    void Update() {
        countdownTo("Default");

        if (Input.GetMouseButtonDown(0)) { //if you attack while attacking, queue up another
            SendMessage("queueAttack");
        }
    }

    

    void createSlashAttack() {
        GameObject slashInstance = Instantiate (SlashAttack); //create new slash object
        slashInstance.layer = gameObject.layer; //set it to the same layer, so it doesnt hurt self
        slashInstance.GetComponent<Slash>().setOwnerTransform(gameObject.transform);
    }

    //getters and setters
}
