using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMovement : MonoBehaviour {

    // variables

    //movement information:                 abreviation:|    description:
    float speed = 0;                            //spd   |  speed = the target/max speed --> changing this valus does not directly change the speed, it only changes the max
    Vector2 acceleration = new Vector2(0,0);    //acc   |  acceleration = change in velocity per frame --> if acc > 2 * speed, then speed is instantaneous
    float friction = 0f;                        //fric  |  decreases speed when not moving
    float duration = 0;                           //dur   |  the durration a force lasts (# of fixed update frames)
    float maxFriction;
    float minFriction = 200;

    //private Dictionary<string, State> forces = new Dictionary<string, State>();


    private List<int[]> forces = new List<int[]>();



    //Components
    private Rigidbody2D C_rigidbody2D;

    // Use this for initialization
    void Start() {
        C_rigidbody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate() {
        simpleMovement();

    }



    void addForce( Vector2 acc, Vector2 max ) {
        int[] temp = new int[2];
        temp[0] = 6;
        temp[1] = 0;

        forces.Add(temp);

    }


    void simpleMovement() {
        //C_rigidbody2D.AddForce(new Vector2(75, 0), ForceMode2D.Force);

        //C_rigidbody2D.AddForce(acceleration, ForceMode2D.Force);

        Vector2 velocity = C_rigidbody2D.velocity;

        friction = (2 / .04f + velocity.magnitude * 5);    //friction increases with speed

        maxFriction = Mathf.Min(velocity.magnitude / Time.fixedDeltaTime, Mathf.Max(acceleration.magnitude * 1.25f, 200));

        C_rigidbody2D.AddForce(Vector2.ClampMagnitude(-velocity.normalized * friction, maxFriction), ForceMode2D.Force);

        acceleration = new Vector2(0, 0);
    }


    //getters and setters

    public void setSpeed( float spd ) {
        speed = spd;
    }

    public float getSpeed() {
        return speed;
    }

    public void setMovement( float spd, float fric ) {
        speed = spd;
        friction = fric;
    }

    public void setFriction( float fric ) {
        friction = fric;
    }

    public float getFriction() {
        return friction;
    }

    public Vector2 getVelocity() {
        return C_rigidbody2D.velocity;
    }


    //Movement Commands

    public void push( Vector2 acc, float dur = .02f ) {   //pushes the object for one frame
        fadePush(acc, acc, dur);
    }

    public void fadePush( Vector2 maxAcc, Vector2 minAcc, float dur = .02f ) {

        acceleration += maxAcc;

        duration = dur;

        if (duration == .02f) {
            duration = Time.fixedDeltaTime;
        }

        C_rigidbody2D.AddForce(maxAcc, ForceMode2D.Force);
        duration -= Time.fixedDeltaTime;

        if (duration >= 0 && dur != 0.02f) {
            StartCoroutine(push(maxAcc, minAcc, duration));
        }
    }


    IEnumerator push( Vector2 maxAcc, Vector2 minAcc, float dur ) {

        Vector2 intermediateVelocity;

        for (float i = dur; i > 0; i -= Time.fixedDeltaTime) {

            yield return new WaitForFixedUpdate();

            intermediateVelocity = maxAcc - (maxAcc - minAcc) * (i / dur);

            C_rigidbody2D.AddForce(intermediateVelocity, ForceMode2D.Force);

            acceleration += intermediateVelocity;
        }
    }


    //static methods n stuff


    public static readonly float PlayerSpd = 24;
    public static readonly float PlayerAcc = 200;


    public static readonly float AttackAcc = 350;

    public static readonly float DashSpd = 72;
    public static readonly float DashAcc = 400;


    public static readonly float EnemySpd = 12;
    public static readonly float EnemyAcc = 400;





    public static KeyCode upKey = KeyCode.W;  //defines key codes
    public static KeyCode downKey = KeyCode.S;
    public static KeyCode rightKey = KeyCode.D;
    public static KeyCode leftKey = KeyCode.A;
    public static KeyCode dash = KeyCode.Space;

    public static Vector2 wasd() {
        return new Vector2(Util.bti(Input.GetKey(rightKey)) - Util.bti(Input.GetKey(leftKey)), Util.bti(Input.GetKey(upKey)) - Util.bti(Input.GetKey(downKey))).normalized;
    }


}


//Old Code

/*

private void updateMovement() {

    if (duration != 0) {   //dictates when to move: (once per frame if constant force, once if for a duration)
        if (C_rigidbody2D.velocity.magnitude < speed) {
            C_rigidbody2D.AddForce(acceleration * Time.timeScale, ForceMode2D.Force);  //apply acceleration
        }

    } else {
        acceleration = new Vector2(0, 0);
    }

    if (duration > 0) {
        duration -= Time.fixedDeltaTime;
    }

    if (acceleration.magnitude == 0 || C_rigidbody2D.velocity.magnitude > speed) {  //if not moving, deccelerate by friction or if above max speed
        C_rigidbody2D.velocity = (Vector2.ClampMagnitude(C_rigidbody2D.velocity, Mathf.Clamp(C_rigidbody2D.velocity.magnitude - friction * Time.timeScale, 0, Mathf.Infinity)));
    }
}
*/



/*
public void clampSpeed(float spd = -1) {
    if (spd == -1) {
        C_rigidbody2D.velocity = (Vector2.ClampMagnitude(C_rigidbody2D.velocity, speed)); //restrict to max speed
    } else {
        C_rigidbody2D.velocity = (Vector2.ClampMagnitude(C_rigidbody2D.velocity, spd)); //restrict to desired speed
    }
}

//public void 


public void push( Vector2 acc, float dur = 1, ) {   //pushes the object for one frame
    acceleration = acc;
    duration = dur;
}
*/
