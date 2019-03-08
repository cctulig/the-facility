using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Zone : MonoBehaviour {

	static int roomAmount;
	static int roomMax;
	static int roomMin;
	static int hallMax;
	static int hallMin;
	static int hallWayWidth;

	public int[,] levelArray = new int[levelDimensions, levelDimensions];
	int[] playerSpawn = { levelDimensions*10, levelDimensions*10 };
	int[] levelExit = { 0, 0 };
	int[] currentInfo = new int[8];

	static Object[][] encounters;
	static int[][] frequency;
	int[,] enemyRooms;
	int[,] enemyPos = new int[levelDimensions, levelDimensions];

	protected Sprite[] floorTiles;
	protected Sprite[] wallTiles;
	static int tileDimensions = 10;
	protected static int levelDimensions = 200;
	int previousDir = -1;

	GameObject Floor;
	GameObject Wall;
	GameObject Exit;

	protected void setupInfo (Sprite[] floortiles, Sprite[] walltiles, GameObject floor, GameObject wall, GameObject exit, int roomamount, int roommax, int roommin, int hallmax, int hallmin, int hallwaywidth, Object[][] enc, int[][] freq) {
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
		encounters = enc;
		frequency = freq;
		enemyRooms = new int[roomAmount, 6];
	}

	protected void setupLevel() {
		createRoom (levelDimensions/2, levelDimensions/2, 0, 0, 0);
		for (int room = 0; room < roomAmount - 1; room++) {
			createHallway (currentInfo [0], currentInfo [1], currentInfo [4], currentInfo [5], currentInfo[7]); 
			createRoom (currentInfo [0], currentInfo [1], currentInfo [2], currentInfo [3], room+1);
			setPlayerSpawn (currentInfo [0] * 10, currentInfo [1] * 10);
		}
	}

	void createRoom(int startX, int startY, int dirX, int dirY, int roomNum) {
		int width = Random.Range (roomMin, roomMax);
		int height = Random.Range (roomMin, roomMax);
		int count = 0;
		for (int xPos = 0; xPos < width; xPos++) {
			for (int yPos = 0; yPos < height; yPos++) {
				levelArray [startX + xPos + width * dirX, startY + yPos + height * dirY] = 1;
			}
		}
		updateEnemyRooms (roomNum, startX, startY, width, height, dirX, dirY);
		setLevelExit (startX, startY, height, width, dirX, dirY);
		pickStartPos (width, height, startX + width * dirX, startY + height * dirY);
	}

	void createHallway(int startX, int startY, int dirX, int dirY, int hdir) {
		//Debug.Log ("Making Hallway: Current x: " + (5*currentInfo [0]) + ". Current Y: " + (5*currentInfo [1]));
		int length = Random.Range(hallMax, hallMin);
		for (int pos = 0; pos < length; pos++) {
			for (int width = 0; width < hallWayWidth; width++) {
				levelArray [startX + pos * dirX + width * dirY * hdir, startY + pos * dirY + width * dirX * hdir] = 1;
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
			isPossible = checkDirection (currentInfo[0], currentInfo[1], currentInfo[6], randPos, width, height);
		}
		//Debug.Log ("Current dir: +" + currentInfo [6] + ", Prev dir: " + previousDir);
		previousDir = currentInfo [6];
	}

	void determinePosition(int pos, int width, int height, int startX, int startY) {
		if (pos >= 0 && pos < width - 1) {
			updateCurrentInfo (startX + pos, startY, 0, -1, 0, -1, 0, -1);
		} else if (pos >= width - 1 && pos < width + height - 2) {
			updateCurrentInfo (startX + width - 1, startY + pos - width + 1, 0, 0, 1, 0, 1, 1);
		} else if (pos >= width + height - 2 && pos < width * 2 + height - 3) {
			updateCurrentInfo (startX + width - (pos - width - height) - 3, startY + height - 1, 0, 0, 0, 1, 2, 1);
		} else {
			updateCurrentInfo (startX, startY + height - (pos - width * 2 - height) - 4, -1, 0, -1, 0, 3, -1);
		}
	}

	bool checkDirection(int xPos, int yPos, int dir, int pos, int width, int height) {
		int buffer = 5;
		if (dir == 0 && previousDir != 2 && pos <= width - hallWayWidth && yPos - hallMax - roomMax > buffer && xPos + roomMax < levelDimensions-buffer) {
			return true;
		}
		if (dir == 1 && previousDir != 3 && pos - width + 1 <= height - hallWayWidth && xPos + hallMax + roomMax < levelDimensions-buffer && yPos - hallWayWidth > buffer && yPos + roomMax < levelDimensions-buffer) {
			return true;
		}
		if (dir == 2 && previousDir != 0 && pos - width - height + 2 >= hallWayWidth - 1 && yPos + hallMax + roomMax < levelDimensions-buffer && xPos - hallWayWidth > buffer && xPos + roomMax < levelDimensions-buffer) {
			return true;
		}
		if (dir == 3 && previousDir != 1 && pos - width*2 - height + 3 >= hallWayWidth - 1 && xPos - hallMax - roomMax > buffer && yPos + roomMax < levelDimensions-buffer) {
			return true;
		}

		return false;
	}

	void updateCurrentInfo(int x, int y, int wX, int wY, int hX, int hY, int dir, int hdir) {
		currentInfo [0] = x;
		currentInfo [1] = y;
		currentInfo [2] = wX;
		currentInfo [3] = wY;
		currentInfo [4] = hX;
		currentInfo [5] = hY;
		currentInfo [6] = dir;
		currentInfo [7] = hdir;
	}

	protected void convertTiles(int currentType, int adjacentType, int targetType) {
		for (int xPos = 1; xPos < levelDimensions-1; xPos++) {
			for (int yPos = 1; yPos < levelDimensions-1; yPos++) {
				if (levelArray [xPos, yPos] == currentType && (levelArray [xPos - 1, yPos - 1] == adjacentType || levelArray [xPos, yPos-1] == adjacentType || levelArray [xPos+1, yPos-1] == adjacentType || levelArray [xPos+1, yPos] == adjacentType || levelArray [xPos+1, yPos+1] == adjacentType || levelArray [xPos, yPos+1] == adjacentType || levelArray [xPos-1, yPos+1] == adjacentType || levelArray [xPos-1, yPos] == adjacentType)) {
					levelArray [xPos, yPos] = targetType;
				}
			}
		}
	}

	protected void setFloorTile (Sprite floorSprite, int xPos, int yPos, int zPos = 60) {
		GameObject floorInstance = Instantiate (Floor);
		floorInstance.transform.position = new Vector3 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions, zPos);
		floorInstance.AddComponent<SpriteRenderer> ();
		floorInstance.GetComponent<SpriteRenderer> ().sprite = floorSprite;
	}

	protected void setWallTile (Sprite wallSprite, int xPos, int yPos, int zPos) {
		GameObject wallInstance = Instantiate (Wall);
		wallInstance.transform.position = new Vector3 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions, zPos);
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
		
	protected void placeWall(int xPos, int yPos, int type, int adjacentType, Sprite[] walls, Sprite[] floors = null, bool invisWall = false) {
		if(checkAdj(xPos, yPos, adjacentType, new int[] {0}) && !checkAdj(xPos, yPos, adjacentType, new int[] {1}) && !checkAdj(xPos, yPos, adjacentType, new int[] {3})) {
			setWallTile (walls [1], xPos, yPos, 48); //Top Right Corner, Convex
			if(invisWall) setFloorTile(floors[1], xPos, yPos + 1, -50);
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {5}) && !checkAdj(xPos, yPos, adjacentType, new int[] {3}) && !checkAdj(xPos, yPos, adjacentType, new int[] {6})) {
			setWallTile (walls [3], xPos, yPos, 50); //Bottom Right Corner, Convex
			if(invisWall) setFloorTile(floors[2], xPos, yPos - 1, 48);
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {7}) && !checkAdj(xPos, yPos, adjacentType, new int[] {4}) && !checkAdj(xPos, yPos, adjacentType, new int[] {6})) {
			setWallTile (walls [5], xPos, yPos, 50); //Bottom Left Corner, Convex
			if(invisWall) setFloorTile(floors[4], xPos, yPos - 1, 48);
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {2}) && !checkAdj(xPos, yPos, adjacentType, new int[] {1}) && !checkAdj(xPos, yPos, adjacentType, new int[] {4})) {
			setWallTile (walls [7], xPos, yPos, 48); //Top Left Corner, Convex
			if(invisWall) setFloorTile(floors[5], xPos, yPos + 1, -50);
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {1})) {
			setWallTile (walls [0], xPos, yPos, -50); //Top Wall
			if(invisWall) setFloorTile(floors[0], xPos, yPos + 1, -50);
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {6})) {
			setWallTile (walls [4], xPos, yPos, 50); //Bottom Wall
			if(invisWall) setFloorTile(floors[3], xPos, yPos - 1, 48);
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {4})) {
			setWallTile (walls [6], xPos, yPos, 49); //Left Wall
		}
		if(checkAdj(xPos, yPos, adjacentType, new int[] {3})) {
			setWallTile (walls [2], xPos, yPos, 49); //Right Wall
		}
		/*if(!checkAdj(xPos, yPos, adjacentType, new int[] {0}) && !checkAdj(xPos, yPos, adjacentType, new int[] {1}) && !checkAdj(xPos, yPos, adjacentType, new int[] {2}) && !checkAdj(xPos, yPos, adjacentType, new int[] {3}) && !checkAdj(xPos, yPos, adjacentType, new int[] {4}) && !checkAdj(xPos, yPos, adjacentType, new int[] {5}) && !checkAdj(xPos, yPos, adjacentType, new int[] {6}) && !checkAdj(xPos, yPos, adjacentType, new int[] {7})) {
			setWallTile (walls [7], xPos, yPos, 57); //Default Wall
		} */
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

	void updateEnemyRooms(int room, int xPos, int yPos, int width, int height, int xDir, int yDir) {
		enemyRooms [room, 0] = xPos;
		enemyRooms [room, 1] = yPos;
		enemyRooms [room, 2] = width;
		enemyRooms [room, 3] = height;
		enemyRooms [room, 4] = xDir;
		enemyRooms [room, 5] = yDir;
	}

	protected void spawnEncounters(){
		for (int room = 0; room < roomAmount; room++) {
			int encNum = Random.Range (0, encounters.GetLength(0));
			int[] tempFreq = new int[encounters [encNum].Length];
			for (int i = 0; i < tempFreq.Length; i++) {
				tempFreq [i] = Random.Range (capMin (frequency [encNum] [i] - 1, 0), frequency [encNum] [i] + 2);
			}
			for (int enemy = 0; enemy < tempFreq.Length; enemy++) {
				for (int amount = 0; amount < tempFreq [enemy]; amount++) {
					int randX = enemyRooms [room, 0] + (enemyRooms [room, 2] - 1) * enemyRooms [room, 4] + Random.Range(0, enemyRooms [room, 2] - 1);
					int randY = enemyRooms [room, 1] + (enemyRooms [room, 3] - 1) * enemyRooms [room, 5] + Random.Range(0, enemyRooms [room, 3] - 1);
					if (enemyPos [randX, randY] == 0) {
						spawnEnemy (encounters [encNum] [enemy], randX, randY, 0);
						enemyPos [randX, randY] = 1;
					}
				}
			}
		}
	}

	void spawnEnemy(Object enemy, int xPos, int yPos, int zPos = 5){
		GameObject enemyInstance = Instantiate ((GameObject)enemy);
		enemyInstance.transform.position = new Vector3 ((float)xPos * (float)tileDimensions, (float)yPos * (float)tileDimensions, zPos);
	}

	static int capMin(int num, int min){
		if (num < min) {
			return min;
		}
		return num;
	}

	protected Sprite[] cutArray(Sprite[] fullArray, int start, int end) {
		Sprite[] temp = new Sprite[end - start + 1];
		for(int i = 0; i < temp.Length; i++) {
			temp [i] = fullArray [start + i];
		}
		return temp;
	}
}
