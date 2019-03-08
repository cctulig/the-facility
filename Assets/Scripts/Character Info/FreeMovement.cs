using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : MonoBehaviour {


    Vector2 acceleration = new Vector2(0, 0);
    Vector2 velocity = new Vector2(0, 0);
    float friction = 0;
    float speed = 0;

    Vector2 rigidAcc, rigidVel;
    float rigidFric, rigidSpd;

    Transform C_transform;
    // Use this for initialization
    void Start() {
        C_transform = gameObject.transform; // this may cause issues at large volumes, try using GetComponent<>() instead at some point
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidAcc = acceleration * Time.fixedDeltaTime;
        rigidVel = velocity * Time.fixedDeltaTime;
        rigidFric = friction * Time.fixedDeltaTime;
        rigidSpd = speed * Time.fixedDeltaTime;


        rigidVel = (Vector2.ClampMagnitude(rigidVel, rigidSpd)); //apply max speed
        if (rigidAcc.magnitude == 0 && rigidFric != 0) {  //if not moving, deccelerate by friction
            rigidVel = Vector2.ClampMagnitude(rigidVel, Mathf.Clamp(rigidVel.magnitude - rigidFric, 0, Mathf.Infinity));
        }
        
        C_transform.Translate(rigidVel);

        velocity = rigidVel / Time.deltaTime;
        //Debug.Log(velocity);
    }

    public void setAcceleration( Vector2 acc ) {
        acceleration = acc;
    }

    public Vector2 getAcceleration() {
        return acceleration;
    }

    public void setVelocty( Vector2 vel ) {
        velocity = vel;
        speed = vel.magnitude;
    }

    public void setVelocityFull( Vector2 vel, float fric ) {
        velocity = vel;
        speed = vel.magnitude;
        friction = fric;
    }

    public void setMovement( Vector2 vel, Vector2 acc ) {
        velocity = vel;
        acceleration = acc;
    }

    public void setMovementFull( float spd, Vector2 acc, float fric ) {
        speed = spd;
        acceleration = acc;
        friction = fric;
    }

    public void setFriction( float fric ) {
        friction = fric;
    }

    public float getFriction() {
        return friction;
    }

    public Vector2 getVelocity() {
        return velocity;
    }
}
