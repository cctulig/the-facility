using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone1 : Zone {
	
	public Zone1(Sprite[] floortiles, Sprite[] walltiles, GameObject floor, GameObject wall, GameObject exit, int roomamount, int roommax, int roommin, int hallmax, int hallmin, int hallwaywidth, Object[][] enc, int[][] freq){
		setupInfo(floortiles, walltiles, floor, wall, exit, roomamount, roommax, roommin, hallmax, hallmin, hallwaywidth, enc, freq);
	}

	public override void initializeZone() {
		setupLevel ();
		createWalls ();
		convertTiles (2, 1, 3); //Back walls to Normal walls
		convertTiles (0, 1, 4); //Empty to cliffs
		convertTiles(2, 0, 3); //Back Walls to Front Walls
		convertTiles (0, 3, 4); //Empty to cliffs
		convertTiles(2, 4, 3);
		placeLevelExit();
		placeTiles ();
		spawnEncounters ();
	}

	void createWalls() {
		int xPos;
		int hasReachedRoom = 1;
		for (int yPos = 0; yPos < levelDimensions; yPos++) {
			xPos = 1;
			while (levelArray [xPos, yPos] == 0 && xPos < levelDimensions-1) {
				levelArray [xPos, yPos] = 2;
				xPos++;
				if (xPos == levelDimensions-2 && hasReachedRoom > 0) {
					deleteWallRow (yPos);
					hasReachedRoom++;
					break;
				}
			}
			hasReachedRoom--;
		}
	}

	void deleteWallRow(int yPos) {
		for (int xPos = 0; xPos < levelDimensions; xPos++) {
			levelArray [xPos, yPos] = 0;
		}
	}

	void placeTiles () {
		for (int xPos = 0; xPos < levelDimensions; xPos++)
		{
			for (int yPos = 0; yPos < levelDimensions; yPos++)
			{
				if (levelArray [xPos, yPos] == 1) {
					setFloorTile (floorTiles [0], xPos, yPos);
				}
				if (levelArray [xPos, yPos] == 2) {
					setWallTile (wallTiles [16], xPos, yPos, -99);
				}
				if (levelArray [xPos, yPos] == 3) {
					setFloorTile (floorTiles [0], xPos, yPos);
					placeWall (xPos, yPos, 3, 2, cutArray(wallTiles, 0, 7), cutArray(floorTiles, 2, 7), true);
				}
				if (levelArray [xPos, yPos] == 4) {
					placeWall (xPos, yPos, 4, 1, cutArray(wallTiles, 8, 15));
					placeWall (xPos, yPos, 4, 3, cutArray (wallTiles, 8, 15));
				}
				if (levelArray [xPos, yPos] == 5) {
					setExitTile (floorTiles [1], xPos, yPos);
				}
			}
		}
	}
}
