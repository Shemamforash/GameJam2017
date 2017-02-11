using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

	public static int levelNum = 0;
	public static int bearsKilled = 0;

	public const int numLevels = 5;
	private static int bearsGotPast = 0;

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
