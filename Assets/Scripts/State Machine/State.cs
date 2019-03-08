using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {


    protected StateMachine sm;
    protected Movement C_movement;
    protected HP C_hp;

    public bool activated = false;
    public bool interupted = false;
    protected string Name;

    protected float duration = 0;

    public float cooldown = 0;

    public string info;

    protected string stateName;

    protected void setup(string name) {

        enabled = false;

        sm = GetComponent<StateMachine>();
        C_movement = GetComponent<Movement>();
        C_hp = GetComponent<HP>();

        sm.addState(name, this);

        stateName = name;
        
    }

    protected void countdownTo( string state, int cooldown = 0, bool forced = false) {   //returns to the selected state after the duration (state of "Retutn" returns to previous state)
        if (duration > 0) {
            duration-= Time.deltaTime;
        } else {

            if (state == "Return") {
                sm.returnState(forced);
            } else {
                sm.toState(state, cooldown, forced);
            }

        }

    }

    public virtual void getInfo(float[] info) {
    }


}

