using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Util {

    static public Vector3 mousePos() {
        return (Input.mousePosition) - new Vector3(Screen.width / 2, Screen.height / 2, 0);

    }

    static public int bti(bool var) {
        if (var) {
            return 1;

        }else {
            return 0;
        }

    }
}
