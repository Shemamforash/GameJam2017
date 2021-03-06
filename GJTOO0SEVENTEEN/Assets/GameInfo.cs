﻿using System.Collections;
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
		return GetWorldTop() - MetresToWorldY(0);
	}
	public static float GetWorldWidth() {
		return GetWorldRight() - MetresToWorldX(0);
	}

	public static float GetWorldRight() {
		return MetresToWorldX(screenWidthInMetres);
	}
	public static float GetWorldTop() {
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
	private static int numBearsKilledThisLevel = 0;

	public static int GetTotalBearsKilled(){
		return totalBearsKilled;
	}
	public static int GetNumBears() {
		const int numBearsToIncPerLevel = 5;
		int result = 5 + levelNum * numBearsToIncPerLevel;
		numBearsStillOnLevel = result;
		return result;
	}

	public static int GetLevelNo(){
		return levelNum + 1;
	}

	public static void NextLevel(){
		bearAccumulator = 0;
		bearsGotPast = 0;
		numBearsKilledThisLevel = 0;
		++levelNum;
		SceneManager.LoadScene("Game");
	}

	public static void ResetGame() {
		walkSpeedModifier = 0;
		chargeModifier = 0;
		bearAccumulator = 0;
		numBearsKilledThisLevel = 0;
		levelNum = 0;		
		bearsGotPast = 0;
		bearPoints = 0;
		totalBearsKilled = 0;
		numBearsStillOnLevel = 0;
		totalBearsGotPast = 0;
	}

	private static int bearAccumulator = 0;

	public static void IncBearsKilled() {
		++numBearsKilledThisLevel;
		++bearAccumulator;
		if(bearAccumulator == 5){
			bearAccumulator = 0;
			++bearPoints;
		}
		totalBearsKilled++;
		CheckIfGameIsOver("inc");		
	}

	public static void IncBearGotPast() {
		bearsGotPast++;
		Debug.Log(bearsGotPast);
		totalBearsGotPast++;
		if (totalBearsGotPast >= 10) {
			SceneManager.LoadScene("Game Over");
		} else {
			CheckIfGameIsOver("past");			
		}
	}

	public static int GetBearsKilledThisLevel(){
		return numBearsKilledThisLevel;

	}

	public static void CheckIfGameIsOver(string origin) {
		numBearsStillOnLevel--;
		Debug.Log(origin + "numBearsStillOnLevel: " + numBearsStillOnLevel);
		if (numBearsStillOnLevel <= 0) {
			SceneManager.LoadScene("Level Up");
		}
	}

	private static float chargeModifier = 1;
	private static float walkSpeedModifier = 1;
	private static float chargeModifierIncrement = 0.02f;
	private static float walkModifierIncrement = 0.02f;

	public static void UpgradeCharge(){
		if(bearPoints > 0){
			chargeModifier -= chargeModifierIncrement;
			--bearPoints;
		}
	}

	public static void UpgradeWalkSpeed() {
		if(bearPoints > 0){
			walkSpeedModifier += walkModifierIncrement;
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
