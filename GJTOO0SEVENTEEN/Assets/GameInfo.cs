using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 

public static class GameInfo {

	//
	// Metres to world coords
	//

	public static float screenWidthInMetres = 8f;
	public static float screenHeightInMetres = ((float)Screen.height / (float)Screen.width) * screenWidthInMetres;

	public static float GetSizeOfMetreInWorldUnits() {
		return GetWorldWidth() / screenWidthInMetres;
	}

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

	public static float goatInitialXMetres = 1f;

	private static int levelNum = 0;
	private static int totalBearsKilled = 0;
	private static int bearPoints = 0;
	private static int totalBearsGotPast = 0;
	private static int bearsGotPast = 0;
	public static int numBearsStillOnLevel = 0;

	public static int GetNumBears() {
		const int numBearsToIncPerLevel = 5;
		int result = 5 + levelNum * numBearsToIncPerLevel;
		numBearsStillOnLevel = result;
		return result;
	}

	public static void NextLevel(){
		bearsGotPast = 0;
		++levelNum;
		SceneManager.LoadScene("Game");
	}

	public static void ResetGame() {
		levelNum = 0;		
		bearsGotPast = 0;
		bearPoints = 0;
		totalBearsKilled = 0;
		numBearsStillOnLevel = 0;
		totalBearsGotPast = 0;
	}

	public static void IncBearsKilled() {
		++bearPoints;
		totalBearsKilled++;
		numBearsStillOnLevel--;
		CheckIfGameIsOver();		
	}

	public static void IncBearGotPast() {
		bearsGotPast++;
		totalBearsGotPast++;
		numBearsStillOnLevel--;
		if (bearsGotPast > 6) {
			SceneManager.LoadScene("Game Over");
		}
		CheckIfGameIsOver();
	}

	public static void CheckIfGameIsOver() {
		if (numBearsStillOnLevel <= 0) {
			SceneManager.LoadScene("Level Up");
		}
	}

	private static float chargeModifier = 1;
	private static float walkSpeedModifier = 1;
	private static float chargeModifierIncrement = 0.95f;
	private static float walkModifierIncrement = 1.05f;

	public static void UpgradeCharge(){
		if(bearPoints > 0){
			chargeModifier *= chargeModifierIncrement;
			--bearPoints;
		}
	}

	public static void UpgradeWalkSpeed() {
		if(bearPoints > 0){
			walkSpeedModifier *= walkModifierIncrement;
			--bearPoints;
		}
	}

	public static int GetBearPoints() {
		return bearPoints;
	}

	public static float GetWalkSpeedModifier(){
		return walkSpeedModifier;
	}

	public static float GetChargeModifier(){
		return chargeModifier;
	}
}
