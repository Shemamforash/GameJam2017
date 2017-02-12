using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour {
	public Text walkSpeed;
	public Text chargeSpeed;


	void Update(){
		walkSpeed.text = "Increase Walk Speed (" + Mathf.Round(GameInfo.GetWalkSpeedModifier() * 100) + "%)";
		chargeSpeed.text = "Decrease Charge Time (" + Mathf.Round(GameInfo.GetChargeModifier() * 100) + "%)";
	}

	public void UpdateWalkSpeed(){
		GameInfo.UpgradeWalkSpeed();
	}

	public void UpdateChargeSpeed(){
		GameInfo.UpgradeCharge();
	}

	public void LoadNextLevel(){
		GameInfo.NextLevel();
	}
}
