using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour{

    public static Effects instance;

    private void Awake() {
        instance = this;

        Application.targetFrameRate = 60;
    }

    int duration = 0;

    public static void time(float speed) {
        Time.timeScale = speed;
        Time.fixedDeltaTime = speed * 0.02f;
    }
    
    
    

}
