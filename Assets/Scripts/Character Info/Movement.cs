using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

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
	void Start () {
        C_rigidbody2D = GetComponent<Rigidbody2D>();
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        simpleMovement();

    }
    
    void simpleMovement() {
        Vector2 velocity = C_rigidbody2D.velocity;

        friction = Mathf.Max(velocity.magnitude * 7, minFriction);    //friction increases with speed

        //friction = Mathf.Min(friction, velocity.magnitude / Time.fixedDeltaTime);

        C_rigidbody2D.AddForce(Vector2.ClampMagnitude(-velocity.normalized * friction, Mathf.Infinity), ForceMode2D.Force);

        acceleration = new Vector2(0, 0);
    }
    
    

    //Movement Commands

    public void fadeInstant( Vector2 maxAcc, Vector2 minAcc, float dur = .02f ) {
        C_rigidbody2D.AddForce(maxAcc/7 - C_rigidbody2D.velocity, ForceMode2D.Impulse);

        fadePush(maxAcc, minAcc, dur);
    }
    
    public void Instant( Vector2 acc, float dur = .02f ) {
        fadeInstant(acc, acc, dur);
    }

    public void push(Vector2 acc, float dur = .02f) {   //pushes the object for one frame
        fadePush(acc, acc, dur);
    }
    
    public void fadePush(Vector2 maxAcc, Vector2 minAcc, float dur = .02f ) {

        acceleration += maxAcc;

        duration = dur;

        if (duration == .02f) {
            duration = Time.fixedDeltaTime;
        }

        C_rigidbody2D.AddForce(maxAcc, ForceMode2D.Force);
        duration -= Time.fixedDeltaTime;

        if (dur > 0.02f) {
            StartCoroutine(push(maxAcc, minAcc, duration));
        }
    }
    

    IEnumerator push( Vector2 maxAcc, Vector2 minAcc, float dur) {

        Vector2 intermediateVelocity;

        for (float i = dur; i > 0; i-= Time.fixedDeltaTime){

            yield return new WaitForFixedUpdate();

            intermediateVelocity = maxAcc - (maxAcc - minAcc) * (1 - i / dur);

            Debug.Log(intermediateVelocity);

            C_rigidbody2D.AddForce(intermediateVelocity, ForceMode2D.Force);

            acceleration += intermediateVelocity;
        }
    }


    //static methods n stuff
    
    public static readonly float PlayerAcc = 150;

    public static readonly float AttackAcc = 100;

    public static readonly float DashAcc = 250;

    public static readonly float enemyAcc = 150;





    public static KeyCode upKey = KeyCode.W;  //defines key codes
    public static KeyCode downKey = KeyCode.S;
    public static KeyCode rightKey = KeyCode.D;
    public static KeyCode leftKey = KeyCode.A;
    public static KeyCode dash = KeyCode.Space;

    public static Vector2 wasd() {
        return new Vector2(Util.bti(Input.GetKey(rightKey)) - Util.bti(Input.GetKey(leftKey)), Util.bti(Input.GetKey(upKey)) - Util.bti(Input.GetKey(downKey))).normalized;
    }


}


