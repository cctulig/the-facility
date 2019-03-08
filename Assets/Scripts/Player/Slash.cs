using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {

    private Transform ownerTransform;
    Transform C_transform;
    
    Vector3 initialCameraDir;

    float baseSize = 125;
    float slashSpeed = 2;
    float slashFriction = .25f;

    int frame = 0;
    float deathTimer = .15f;

    // Use this for initialization
    void Start () {

        initialCameraDir = Util.mousePos().normalized;
        C_transform = GetComponent<Transform>();
        
        C_transform.localScale = new Vector3(4, 4, 1) * baseSize;
    }

    // Update is called once per frame
    void Update() {
        frame++;
        deathTimer -= Time.deltaTime;
        C_transform.position = ownerTransform.position + initialCameraDir * 5;
        if (deathTimer <= 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitCollider) {
        Debug.Log("collided");
        if (hitCollider.gameObject.layer != gameObject.layer) { //checks if the object is on the enemy layer

            Debug.Log("different Layer");
            hitCollider.GetComponent<HP>().damage(1);
            hitCollider.GetComponent<HP>().hit(1, 1);
        }
    }

//getters and setters
    public void setOwnerTransform(Transform trans) {
        ownerTransform = trans;
    }
}
