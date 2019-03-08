using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour {

	public int[,] levelArray = new int[100, 100];
	int[] currentInfo = new int[7];
	/* Current Info:
	 * 0 = Current X Pos
	 * 1 = Current Y Pos
	 * 2 = Current X Room Dir
	 * 3 = Current Y Room Dir
	 * 4 = Current X Hall Dir
	 * 5 = Current Y Hall Dir
	 * 6 = Current Direction (0=down, 1=right, 2=up, 3=left)
	 */

	public Sprite[] floorTiles;
	public Sprite[] wallTiles;
	public static int tileDimensions = 10;

	public GameObject Floor;
	public GameObject Wall;

	//Info for how many rooms to make, room size, hallway length, etc.
	public static int roomAmount = 3;
	public static int roomMax = 4;
	public static int roomMin = 4;
	public static int hallMax = 3;
	public static int hallMin = 3;
	public static int hallWayWidth = 1;

	// Use this for initialization
	void Start () {
		setupLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setupLevel() {
		//Debug.Log ("Before 1st room: Current x: " + currentInfo [0] + ". Current Y: " + currentInfo [1]);
		createRoom (50, 50, 0, 0);
		//Debug.Log ("After 1st room: Current x: " + currentInfo [0] + ". Current Y: " + currentInfo [1]);
		for (int room = 0; room < roomAmount-1; room++) {
			Debug.Log ("Before Hallway: Current x: " + (5*currentInfo [0]) + ". Current Y: " + (5*currentInfo [1]));
			createHallway (currentInfo [0], currentInfo [1], currentInfo [4], currentInfo [5]); 
			Debug.Log ("After Hallway: Current x: " + (5*currentInfo [0]) + ". Current Y: " + (5*currentInfo [1]));
			createRoom (currentInfo [0], currentInfo [1], currentInfo [2], currentInfo [3]);
			//Debug.Log ("After 2nd room: Current x: " + currentInfo [0] + ". Current Y: " + currentInfo [1]);

		}
		createWalls ();
		convertTiles (2, 1, 3); //Back walls to Normal walls
		convertTiles (0, 1, 4); //Empty to cliffs
		placeTiles ();
	}

	void createRoom(int startX, int startY, int dirX, int dirY) {
		int width = Random.Range (roomMin, roomMax);
		int height = Random.Range (roomMin, roomMax);
		int count = 0;
		for (int xPos = 0; xPos < width; xPos++) {
			for (int yPos = 0; yPos < height; yPos++) {
				levelArray [startX + xPos + width * dirX, startY + yPos + height * dirY] = 1;
			}
		}
		pickStartPos (width, height, startX + width * dirX, startY + height * dirY);
	}

	void createHallway(int startX, int startY, int dirX, int dirY) {
		Debug.Log ("Making Hallway: Current x: " + (5*currentInfo [0]) + ". Current Y: " + (5*currentInfo [1]));
		int length = Random.Range(hallMax, hallMin);
		for (int pos = 0; pos < length; pos++) {
			for (int width = 0; width < hallWayWidth; width++) {
				levelArray [startX + pos * dirX + width * dirY * -1, startY + pos * dirY + width * dirX * -1] = 1;
				currentInfo [0] = startX + pos * dirX;
				currentInfo [1] = startY + pos * dirY;
			}
		}

	}
		
	void pickStartPos(int width, int height, int startX, int startY) {
		bool isPossible = false;
		int randPos = 0;
		while (!isPossible) {
			randPos = Random.Range (0, (width + height) * 2 - 4);
			determinePosition (randPos, width, height, startX, startY);
			isPossible = checkDirection (currentInfo[0], currentInfo[1], currentInfo[6]);
		}
		Debug.Log ("After: Current x: " + (5*currentInfo [0]) + ". Current Y: " + (5*currentInfo [1]));

	}
		
	void determinePosition(int pos, int width, int height, int startX, int startY) {
		if (pos >= 0 && pos < width - 1) {
			updateCurrentInfo (startX + pos, startY, 0, -1, 0, -1, 0);
		} else if (pos >= width - 1 && pos < width + height - 2) {
			updateCurrentInfo (startX + width - 1, startY + pos - width + 1, 0, 0, 1, 0, 1);
		} else if (pos >= width + height - 2 && pos < width * 2 + height - 3) {
			updateCurrentInfo (startX + width - (pos - width - height) - 3, startY + height - 1, 0, 0, 0, 1, 2);
		} else {
			updateCurrentInfo (startX, startY + height - (pos - width * 2 - height) - 4, -1, 0, -1, 0, 3);
		}
	}

	bool checkDirection(int xPos, int yPos, int dir) {
		if (dir == 0 && yPos - hallMax - roomMax > 0 && xPos + roomMax < 99) {
			return true;
		}
		if (dir == 1 && xPos + hallMax + roomMax < 99 && yPos - hallWayWidth > 0 && yPos + roomMax < 99) {
			return true;
		}
		if (dir == 2 && yPos + hallMax + roomMax < 99 && xPos - hallWayWidth > 0 && xPos + roomMax < 99) {
			return true;
		}
		if (dir == 3 && xPos - hallMax - roomMax > 0 && yPos + roomMax < 99) {
			return true;
		}
		return false;
	}

	void updateCurrentInfo(int x, int y, int wX, int wY, int hX, int hY, int dir) {
		currentInfo [0] = x;
		currentInfo [1] = y;
		currentInfo [2] = wX;
		currentInfo [3] = wY;
		currentInfo [4] = hX;
		currentInfo [5] = hY;
		currentInfo [6] = dir;
	}

	void createWalls() {
		int xPos;
		int hasReachedRoom = 1;
		for (int yPos = 0; yPos < 100; yPos++) {
			xPos = 1;
			while (levelArray [xPos, yPos] == 0 && xPos < 99) {
				levelArray [xPos, yPos] = 2;
				xPos++;
				if (xPos == 98 && hasReachedRoom > 0) {
					deleteWallRow (yPos);
					hasReachedRoom++;
					break;
				}
			}
			hasReachedRoom--;
		}
	}

	void deleteWallRow(int yPos) {
		for (int xPos = 0; xPos < 100; xPos++) {
			levelArray [xPos, yPos] = 0;
		}
	}

	void convertTiles(int currentType, int adjacentType, int targetType) {
		for (int xPos = 1; xPos < 99; xPos++) {
			for (int yPos = 1; yPos < 99; yPos++) {
				if (levelArray [xPos, yPos] == currentType && (levelArray [xPos - 1, yPos - 1] == adjacentType || levelArray [xPos, yPos-1] == adjacentType || levelArray [xPos+1, yPos-1] == adjacentType || levelArray [xPos+1, yPos] == adjacentType || levelArray [xPos+1, yPos+1] == adjacentType || levelArray [xPos, yPos+1] == adjacentType || levelArray [xPos-1, yPos+1] == adjacentType || levelArray [xPos-1, yPos] == adjacentType)) {
					levelArray [xPos, yPos] = targetType;
				}
			}
		}
	}

	void placeTiles () {
		for (int xPos = 0; xPos < 100; xPos++)
		{
			for (int yPos = 0; yPos < 100; yPos++)
			{
				if (levelArray [xPos, yPos] == 1) {
					setFloorTile (floorTiles [0], xPos, yPos);
				}
				if (levelArray [xPos, yPos] == 2) {
					setWallTile (wallTiles [1], xPos, yPos);
				}
				if (levelArray [xPos, yPos] == 3) {
					setWallTile (wallTiles [0], xPos, yPos);
				}
				if (levelArray [xPos, yPos] == 4) {
					setWallTile (wallTiles [2], xPos, yPos);
				}
			}
		}
	}

	void setFloorTile (Sprite floorSprite, int xPos, int yPos) {
		GameObject floorInstance = Instantiate (Floor);
		floorInstance.transform.position = new Vector2 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions);
		floorInstance.AddComponent<SpriteRenderer> ();
		floorInstance.GetComponent<SpriteRenderer> ().sprite = floorSprite;
	}

	void setWallTile (Sprite wallSprite, int xPos, int yPos) {
		GameObject wallInstance = Instantiate (Wall);
		wallInstance.transform.position = new Vector2 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions);
		wallInstance.AddComponent<SpriteRenderer> ();
		wallInstance.GetComponent<SpriteRenderer> ().sprite = wallSprite;
	}
}