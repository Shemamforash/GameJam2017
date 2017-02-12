using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {
	public Text bearsKilled, level;
	void Start () {
		bearsKilled.text = "Bears Killed: " + GameInfo.GetTotalBearsKilled();
		level.text = "Level reached: " + GameInfo.GetLevelNo();
	}
}
