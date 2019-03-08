using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEncounters : MonoBehaviour {

	GameObject[][] encounters;
	static int[][] frequency;
	int[,] enemyRooms;
	int[,] enemyPos;
	int numRooms;

	public SpawnEncounters(GameObject[][] enc, int[][] freq, int[,] rooms, int num) {
		encounters = enc;
		frequency = freq;
		enemyRooms = rooms;
		numRooms = num;
	}

	void spawnEncounters(){
		for (int room = 0; room < numRooms; room++) {
			int encNum = Random.Range (0, encounters.GetLength(0)-1);
			int[] tempFreq = new int[encounters [encNum].Length];
			for (int i = 0; i < tempFreq.Length; i++) {
				tempFreq [i] = Random.Range (capMin (frequency [encNum] [i] - 1, 0), frequency [encNum] [i] + 1);
			}
			for (int enemy = 0; enemy < tempFreq.Length; enemy++) {
				for (int amount = 0; amount < tempFreq [enemy]; amount++) {
					int randX = Random.Range(enemyRooms[room, 0], enemyRooms[room, 0] + enemyRooms[room, 2] - 1);
					int randY = Random.Range(enemyRooms[room, 1], enemyRooms[room, 1] + enemyRooms[room, 3] - 1);
				}
			}
		}
	}

	void spawnEnemy(GameObject enemy, int xPos, int yPos){

	}

	static int capMin(int num, int min){
		if (num < min) {
			return min;
		}
		return num;
	}
}
