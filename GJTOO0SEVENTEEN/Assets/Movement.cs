using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Image bashPowerupBar;
	// public Text levelText;
	// public Text bearsText;

	enum GoatMoveState {normal, bashing, returning};
	private int goatState = (int)GoatMoveState.normal;

	private const float goatSpeed = 8f;
	private float goatWalkSpeed;
	private float goatInitialXWorld; 

	private float goatMaxBashPowerAmountInSeconds = 5f;

	private const string moveUpKey = "w";
	private const string moveDownKey = "s";
	private const string bashKey = "space";

	private float goatXVelocity = 0; // velocity of goat as it is bashing
	private float goatBashPowerupValue = 0f; // [0, 1]

    private Rigidbody2D rb;

	void Start () {
		goatWalkSpeed = goatSpeed * GameInfo.GetWalkSpeedModifier();
		goatMaxBashPowerAmountInSeconds *= GameInfo.GetChargeModifier();
		goatInitialXWorld = GameInfo.MetresToWorldX(GameInfo.goatInitialXMetres);
		rb = gameObject.GetComponent<Rigidbody2D>();
		transform.localPosition = new Vector3(goatInitialXWorld, 0, 0);
	}

    void OnTriggerEnter2D(Collider2D coll) {
    	GameObject obj = coll.gameObject;
    	if (goatState == (int)GoatMoveState.bashing &&
    	    obj.transform.position.x >= transform.position.x) {
	    	goatState = (int)GoatMoveState.returning;
			BearMovement bm = obj.GetComponent<BearMovement>();
			bm.BearHasBeenHit();
			// obj.SendMessage("HandleCollisionWithGoat");    		
    	}
    }

	void Update () {
		Vector3 deltaPosition = new Vector3();

		switch (goatState) {
			case (int)GoatMoveState.normal: {

				// 
				// regular up and down movement
				//

				if (Input.GetKey(moveUpKey)) {
					deltaPosition.y += goatSpeed * Time.deltaTime;
				} 
				if (Input.GetKey(moveDownKey)) {
					deltaPosition.y -= goatSpeed * Time.deltaTime;
				}			

				// 
				// handle if the bash key is used
				//

				if (goatBashPowerupValue < 1) {
					goatBashPowerupValue += Time.deltaTime / goatMaxBashPowerAmountInSeconds;
				} else {
					goatBashPowerupValue = 1;
				}

				if (Input.GetKeyUp(bashKey)) {
					// key released
					goatXVelocity = goatSpeed; // set an inital velocity for the bash
					goatState = (int)GoatMoveState.bashing;
				}
				break;
			}
			case (int)GoatMoveState.bashing: {
				const float maxDistanceMetres = 8f; // how far can the goat go with full powerup
				float distanceOfThisBash = GameInfo.MetresToWorldX(maxDistanceMetres) * goatBashPowerupValue;

				if (transform.position.x < (goatInitialXWorld + distanceOfThisBash)) {
					goatXVelocity *= 1.5f; // acceleration
					if (goatXVelocity > 100f) {
						goatXVelocity = 100f;
					}
					deltaPosition.x = goatXVelocity * Time.deltaTime;
				} else {
					goatState = (int)GoatMoveState.returning;
				}
				break;
			}
			case (int)GoatMoveState.returning: {
				if (transform.position.x > goatInitialXWorld) {
					// if (goatBashPowerupValue > 0f) {
						// goatBashPowerupValue -= 1f * Time.deltaTime;						
					// } else {
						goatBashPowerupValue = 0f;
					// }
					deltaPosition.x -= (goatWalkSpeed) * Time.deltaTime; // speed at which it returns
				} else {
					goatState = (int)GoatMoveState.normal;
				}

				break;
			}
		}

		Vector3 newScale = transform.localScale;
		if (newScale.x > 0 && goatState == (int)GoatMoveState.returning) {
			newScale.x *= -1;
		} else if (newScale.x < 0 && goatState != (int)GoatMoveState.returning) {
			newScale.x *= -1;
		}
		transform.localScale = newScale;

		const float arbitraryModifier = 75f; // just picked because it feels about right...
		Vector3 velo = deltaPosition * arbitraryModifier;
		rb.velocity = velo;

		bashPowerupBar.fillAmount = goatBashPowerupValue;

	}


}

