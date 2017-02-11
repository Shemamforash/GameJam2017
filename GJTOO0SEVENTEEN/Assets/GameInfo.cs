using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

	//
	// Metres to world coords
	//

	public static float screenWidthInMetres = 8f;
	public static float screenHeightInMetres = ((float)Screen.height / (float)Screen.width) * screenWidthInMetres;

	public static float MetresToWorldX(float x) {
		float pixelsPerMetre = (float)Screen.width / screenWidthInMetres;
		Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(pixelsPerMetre * x, 0, Camera.main.nearClipPlane));
		return p.x;
	}
	public static float MetresToWorldY(float y) {
		float pixelsPerMetre = (float)Screen.width / screenWidthInMetres;
		Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(0, pixelsPerMetre * y, Camera.main.nearClipPlane));
		return p.y;
	}

	public static float GetWorldHeight() {
		return MetresToWorldY(0) - GetWorldBottom();
	}
	public static float GetWorldWidth() {
		return GetWorldRight() - MetresToWorldX(0);
	}

	public static float GetWorldRight() {
		return MetresToWorldX(screenWidthInMetres);
	}
	public static float GetWorldBottom() {
		return MetresToWorldY(screenHeightInMetres);
	}


	//
	// Game state
	//

	private static int levelNum = 0;
	private static int bearsKilled = 0;
	private static int bearsGotPast = 0;
	public static int numBearsStillOnLevel = 0;

	public static int GetNumBears() {
		const int numBearsToIncPerLevel = 5;
		int result = 5 + levelNum * numBearsToIncPerLevel;
		numBearsStillOnLevel = result;
		return result;
	}

	public static void ResetGame() {
		levelNum = 0;		
		bearsGotPast = 0;
		bearsKilled = 0;
		numBearsStillOnLevel = 0;
	}

	public static void IncBearsKilled() {
		bearsKilled++;
		numBearsStillOnLevel--;
		CheckIfGameIsOver();		
	}

	public static void IncBearGotPast() {
		bearsGotPast++;
		numBearsStillOnLevel--;
		if (bearsGotPast > 6) {
			Debug.Log("GAME OVER");
			// GAME OVER
			// you let too many bears past you muppet
		}
		CheckIfGameIsOver();
	}

	public static void CheckIfGameIsOver() {
		if (numBearsStillOnLevel <= 0) {
			Debug.Log("NEW LEVEL");			
			// Level is over - start the next level
		}
	}
}
