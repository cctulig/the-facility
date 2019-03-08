using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {
	Zone currentZone;
	int zoneNumber = 1;

	public Sprite[] floorTiles;
	public Sprite[] wallTiles;
	public GameObject Floor;
	public GameObject Wall;
	public GameObject Exit;
	public AstarPath Path;

	public GameObject Player;
	public static Object Enemy;
	public static Object Crystal;

	public Object[][] encounter;
	int[][] frequency = new int[2][] 
	{
		new int[] { 3, 2 },
		new int[] { 5 }
	};

	Zone[] zones;

	void Awake() {
		Enemy = Resources.Load("Prefabs/Enemy");
		Crystal = Resources.Load("Prefabs/Destroyable Object");

		encounter = new Object[2][] 
		{
			new Object[] { Enemy, Crystal},
			new Object[] { Enemy } 
		};
	}

	// Use this for initialization
	void Start () {
		Debug.Log (encounter[0][0]);
		zones = new Zone[2];
		zones [0] = null;
		zones[1] = new Zone1(floorTiles, wallTiles, Floor, Wall, Exit, 15, 6, 12, 6, 6, 3, encounter, frequency); //Numbers: Room amount, Room max, Room min, Hall Max, Hall Min, Hall width

		setupZone ();
		//zones [zoneNumber].initializeZone ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void refreshZone(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		zones [zoneNumber].initializeZone ();
	}

	void setupZone() {
		zones [zoneNumber].initializeZone ();
		//Path.Scan ();
		int[] playerSpawnPos = zones [zoneNumber].getPlayerSpawn ();
		Debug.Log("Player X: " + playerSpawnPos[0] + ", Player Y: " + playerSpawnPos[1]);
		Player.transform.position = new Vector3 (playerSpawnPos [0], playerSpawnPos [1], Player.transform.position.z);
		//Enemy.transform.position = new Vector3 (playerSpawnPos [0]+10, playerSpawnPos [1]+10, Player.transform.position.z+5);
		//Crystal.transform.position = new Vector3 (playerSpawnPos [0]+20, playerSpawnPos [1]+20, Player.transform.position.z+5);
	}
}
