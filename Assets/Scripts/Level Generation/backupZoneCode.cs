using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class backupZoneCode : MonoBehaviour {

	public int[,] levelArray = new int[100, 100];
	int[] playerSpawn = { 505, 505 };
	int[] levelExit = { 0, 0 };
	int[] currentInfo = new int[7];

	protected Sprite[] floorTiles;
	protected Sprite[] wallTiles;
	static int tileDimensions = 10;
	int previousDir = 0;

	GameObject Floor;
	GameObject Wall;
	GameObject Exit;

	static int roomAmount;
	static int roomMax;
	static int roomMin;
	static int hallMax;
	static int hallMin;
	static int hallWayWidth;

	protected void setupInfo (Sprite[] floortiles, Sprite[] walltiles, GameObject floor, GameObject wall, GameObject exit, int roomamount, int roommax, int roommin, int hallmax, int hallmin, int hallwaywidth) {
		floorTiles = floortiles;
		wallTiles = walltiles;
		Floor = floor;
		Wall = wall;
		Exit = exit;
		roomAmount = roomamount;
		roomMax = roommax;
		roomMin = roommin;
		hallMax = hallmax;
		hallMin = hallmin;
		hallWayWidth = hallwaywidth;
	}

	protected void setupLevel() {
		createRoom (50, 50, 0, 0);
		for (int room = 0; room < roomAmount - 1; room++) {
			createHallway (currentInfo [0], currentInfo [1], currentInfo [4], currentInfo [5]); 
			createRoom (currentInfo [0], currentInfo [1], currentInfo [2], currentInfo [3]);
			setPlayerSpawn (currentInfo [0] * 10, currentInfo [1] * 10);
		}
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
		setLevelExit (startX, startY, height, width, dirX, dirY);
		pickStartPos (width, height, startX + width * dirX, startY + height * dirY);
	}

	void createHallway(int startX, int startY, int dirX, int dirY) {
		//Debug.Log ("Making Hallway: Current x: " + (5*currentInfo [0]) + ". Current Y: " + (5*currentInfo [1]));
		int length = Random.Range(hallMax, hallMin);
		for (int pos = 0; pos < length; pos++) {
			for (int width = 0; width < hallWayWidth; width++) {
				//levelArray [startX + pos * dirX + width * dirY * -1, startY + pos * dirY + width * dirX * -1] = 1;
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
		//Debug.Log ("Current dir: +" + currentInfo [6] + ", Prev dir: " + previousDir);
		previousDir = currentInfo [6];
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
		if (dir == 0 && previousDir != 2 && yPos - hallMax - roomMax > 0 && xPos + roomMax < 99) {
			return true;
		}
		if (dir == 1 && previousDir != 3 && xPos + hallMax + roomMax < 99 && yPos - hallWayWidth > 0 && yPos + roomMax < 99) {
			return true;
		}
		if (dir == 2 && previousDir != 0 && yPos + hallMax + roomMax < 99 && xPos - hallWayWidth > 0 && xPos + roomMax < 99) {
			return true;
		}
		if (dir == 3 && previousDir != 1 && xPos - hallMax - roomMax > 0 && yPos + roomMax < 99) {
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

	protected void convertTiles(int currentType, int adjacentType, int targetType) {
		for (int xPos = 1; xPos < 99; xPos++) {
			for (int yPos = 1; yPos < 99; yPos++) {
				if (levelArray [xPos, yPos] == currentType && (levelArray [xPos - 1, yPos - 1] == adjacentType || levelArray [xPos, yPos-1] == adjacentType || levelArray [xPos+1, yPos-1] == adjacentType || levelArray [xPos+1, yPos] == adjacentType || levelArray [xPos+1, yPos+1] == adjacentType || levelArray [xPos, yPos+1] == adjacentType || levelArray [xPos-1, yPos+1] == adjacentType || levelArray [xPos-1, yPos] == adjacentType)) {
					levelArray [xPos, yPos] = targetType;
				}
			}
		}
	}

	protected void setFloorTile (Sprite floorSprite, int xPos, int yPos) {
		GameObject floorInstance = Instantiate (Floor);
		floorInstance.transform.position = new Vector2 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions);
		floorInstance.AddComponent<SpriteRenderer> ();
		floorInstance.GetComponent<SpriteRenderer> ().sprite = floorSprite;
	}

	protected void setWallTile (Sprite wallSprite, int xPos, int yPos) {
		GameObject wallInstance = Instantiate (Wall);
		wallInstance.transform.position = new Vector2 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions);
		wallInstance.AddComponent<SpriteRenderer> ();
		wallInstance.GetComponent<SpriteRenderer> ().sprite = wallSprite;
	}

	protected void setExitTile (Sprite exitSprite, int xPos, int yPos) {
		GameObject exitInstance = Instantiate (Exit);
		exitInstance.transform.position = new Vector2 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions);
		exitInstance.AddComponent<SpriteRenderer> ();
		exitInstance.GetComponent<SpriteRenderer> ().sprite = exitSprite;
	}

	void setPlayerSpawn(int xPos, int yPos) {
		if ((xPos < playerSpawn [0] && yPos < playerSpawn [1] + (hallMax + roomMax) * 10) || (xPos < playerSpawn [0] + hallMax * 10 && yPos < playerSpawn [1] - hallMax * 10)) {
			playerSpawn [0] = xPos;
			playerSpawn [1] = yPos;
		}
	}

	void setLevelExit(int xPos, int yPos, int height, int width, int dirX, int dirY){
		if ((xPos > levelExit [0] && yPos > levelExit [1] - hallMax - roomMax) || (xPos > levelExit [0] - hallMax && yPos > levelExit [1] + hallMax)) {
			Debug.Log ("dirX: " + dirX + ", dirY: " + dirY);
			levelExit [0] = xPos + 3 + width * dirX;
			levelExit [1] = yPos + 3 + width * dirY;
		}
	}

	protected void placeLevelExit() {
		levelArray [levelExit [0], levelExit [1]] = 5;
	}

	public int[] getPlayerSpawn () {
		return playerSpawn;
	}

	public abstract void initializeZone (); 

	protected void placeWall(int xPos, int yPos, int type, int adjacentType, Sprite[] walls) {
		if(checkAdj(xPos, yPos, adjacentType, new int[] {0,1,3})) {
			setWallTile (walls [0], xPos, yPos); //Bottom Left Corner, Concave
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {3,5,6})) {
			setWallTile (walls [1], xPos, yPos); //Top Left Corner, Concave
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {4,6,7})) {
			setWallTile (walls [2], xPos, yPos); //Top Right Corner, Concave
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {1,2,4})) {
			setWallTile (walls [3], xPos, yPos); //Bottom Right Corner, Concave
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {6})) {
			setWallTile (walls [4], xPos, yPos); //Top Wall
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {4})) {
			setWallTile (walls [5], xPos, yPos); //Right Wall
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {1})) {
			setWallTile (walls [6], xPos, yPos); //Bottom Wall
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {3})) {
			setWallTile (walls [7], xPos, yPos); //Left Wall
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {0})) {
			setWallTile (walls [8], xPos, yPos); //Bottom Left Corner, Convex
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {5})) {
			setWallTile (walls [9], xPos, yPos); //Top Left Corner, Convex
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {7})) {
			setWallTile (walls [10], xPos, yPos); //Top Right Corner, Convex
		}
		else if(checkAdj(xPos, yPos, adjacentType, new int[] {2})) {
			setWallTile (walls [11], xPos, yPos); //Bottom Right Corner, Convex
		}
	}

	protected bool checkAdj(int xPos, int yPos, int type, int[] checks) {
		for (int i = 0; i < checks.Length; i++) {
			if (checks[i] == 0 && type != levelArray [xPos - 1, yPos - 1]) {
				return false;
			}
			if (checks[i] == 1 && type != levelArray [xPos, yPos - 1]) {
				return false;
			}
			if (checks[i] == 2 && type != levelArray [xPos + 1, yPos - 1]) {
				return false;
			}
			if (checks[i] == 3 && type != levelArray [xPos - 1, yPos]) {
				return false;
			}
			if (checks[i] == 4 && type != levelArray [xPos + 1, yPos]) {
				return false;
			}
			if (checks[i] == 5 && type != levelArray [xPos - 1, yPos + 1]) {
				return false;
			}
			if (checks[i] == 6 && type != levelArray [xPos, yPos + 1]) {
				return false;
			}
			if (checks[i] == 7 && type != levelArray [xPos + 1, yPos + 1]) {
				return false;
			}
		}
		return true;
	}
}
