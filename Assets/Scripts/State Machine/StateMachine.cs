using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {

    //holds any information that needs to be transfered between objects (make sure to keep track of which is which)
    private int[] info = new int[10];

    //allows for states to reference each other (give names to each, just added a bunch of extras in case they are needed
    
        
       

    private string lastState = "Default";

    private Dictionary<string, State> states = new Dictionary<string, State>();



    public void addState(string name, State state) {
        states.Add(name, state);
    }

    public bool toState(string state, float cooldown = 0, bool forced  = false, float[] info = null) {        //change to state # with cooldown (0 for none)    forces transition (and doesnt terminate) if true

        Debug.Log("switching to state: " + state);

        if (states[state].cooldown <= 0 || forced) {  
            foreach (KeyValuePair<string, State> index in states) {//access the key and value both separately from states dictionary
                
                if (index.Value.enabled) {                //dissables the current state appropriotely

                    if (index.Key != state) {                     //cant set the previous state to the new state (prevents loops)

                        lastState = index.Key;
                    }

                    index.Value.interupted = forced;
                    index.Value.enabled = false;
                }
            }

            if (states[state] != null) {
                states[state].enabled = true;   //enable target state
                states[state].getInfo(info);
            } else {
                Debug.Log("State not found, returning to previous state instead");
                returnState(forced);
            }
            return true;                    //return true if successful, else false
        } else {
            return false;
        }


    }

    public bool toState( string state, float[] info, float cooldown = 0, bool forced = false ) {        //change to state # with cooldown (0 for none)    forces transition (and doesnt terminate) if true

        return toState(state, cooldown, forced, info);
    

    }

    public void returnState(bool forced ) {
         toState(lastState, 0, forced);

    }

    public void cooldown(string state, float cooldown ) {
        states[state].cooldown = cooldown;
    }
		
    
    void Start() {
        foreach (KeyValuePair<string, State> index in states) {//access the key and value both separately from states dictionary
            if (index.Key == "Default") {
                index.Value.enabled = true;
            }
        }
    }

    void Update() {//apply cooldowns
        foreach (KeyValuePair<string, State> index in states) {
            if(index.Value.cooldown > 0) {
                index.Value.cooldown -= Time.deltaTime;
            }
        }
    }
}
