using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : State {

    private void Awake() {
        setup("Hit");           //hit state is when the enemy is recoiling from being hit 
    }

    /*
    void OnEnable() {
        Debug.Log(info);

        if(info == "light") {   //decides what happens when it is hit (light medium , or heavy knockback)
            lightHit();
        } else if(info == "heavy") {
            heavyHit();
        } else {
            mediumHit();
        }

    }

    // Update is called once per frame
    void Update () {
        countdownTo("Return", 0, false);

	}

    void lightHit() {

        duration = 1;
        C_movement.setMovement(20, 2f);
        C_movement.pushDuration(Util.mousePos().normalized * 20, 2);
    }

    void mediumHit() {

        duration = 5;
        C_movement.setMovement(40, 2f);
        C_movement.pushDuration(Util.mousePos().normalized * 70, 2);
    }

    void heavyHit() {

        duration = 60;
        C_movement.setMovement(70, 2f);
        C_movement.pushDuration(Util.mousePos().normalized * 100, 2);
    }
    */

}
