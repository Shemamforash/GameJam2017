using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 


public class StartGame : MonoBehaviour {
	public void LoadGame(){
		GameInfo.ResetGame();
		SceneManager.LoadScene("Game");
	}

	public void ToMainMenu(){
		SceneManager.LoadScene("Menu");
	}

	public void ToInstructionMenu(){
		SceneManager.LoadScene("How to Play");
	}
}
