using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class GameInfo {

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


	public static int levelNum = 0;
	public static int numBearsToIncPerLevel = 0;

	public static int bearsKilled = 0;

	public const int numLevels = 5;
	private static int bearsGotPast = 0;

	public static int GetNumBears() {
		return 10 + levelNum * numBearsToIncPerLevel;
	}

	public static void ResetGame() {
		levelNum = 0;		
		bearsGotPast = 0;
	}

	public static bool IncBearGotPast() {
		bearsGotPast++;
		if (bearsGotPast > 6) {
			return true;
		}
		return false;
	}
}
