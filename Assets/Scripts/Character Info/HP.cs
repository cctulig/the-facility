using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour {
	public int health = 1;


    public bool invulnerable = false;
    public int armor = 0;
    int defaultArmor = 0;

    StateMachine sm;
    private void Awake() {
        sm = GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update () {
		if (health <= 0) {
            Destroy(gameObject);
		}
	}



    public void hit( int power , int dmg) {  //used to enter a hit state, determines which one based on the attack power and unit armor 
        
    }

    public int getHP() {
        return health;

    }

    public void setHP(int targetHP) {
        health = targetHP;

    }

    public void damage(int dmg) {
        health -= dmg;
    }
}
