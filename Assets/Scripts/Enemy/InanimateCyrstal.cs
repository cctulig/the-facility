using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InanimateCyrstal : MonoBehaviour {

	public Sprite[] damageStates;

	private HP C_HP;
	private SpriteRenderer currentSprite;
	private static int maxHP = 6;

	// Use this for initialization
	void Start () {
		C_HP = GetComponent<HP> ();
		currentSprite = GetComponent<SpriteRenderer> ();

		C_HP.setHP (maxHP);
		currentSprite.sprite = damageStates [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (C_HP.getHP () < 2 * maxHP / 3 && C_HP.getHP () > maxHP / 3) {
			currentSprite.sprite = damageStates [1];
		} 
		else if (C_HP.getHP () < maxHP / 3) {
			currentSprite.sprite = damageStates [2];
		}
	}
}
