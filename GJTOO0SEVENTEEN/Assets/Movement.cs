using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	enum GoatMoveState {normal, bashing, returning};
	private int goatState = (int)GoatMoveState.normal;

	private const float goatSpeed = 5f;
	private const float goatInitalX = -8f; // TODO: just arbitrarily chosen, is there a more robust way?
	private const float goatMaxBashPowerAmountInSeconds = 1f;

	private const string moveUpKey = "w";
	private const string moveDownKey = "s";
	private const string bashKey = "space";

	private float goatXVelocity = 0; // velocity of goat as it is bashing
	private float goatBashPowerupValue = 0f; // [0, 1]

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(5, 5, 0);
		transform.localPosition = new Vector3(goatInitalX, 0, 0);
	}

    void OnCollisionEnter2D(Collision2D coll) {
		print("OnCollisionEnter2D goat state: " + goatState);
    	if (goatState == (int)GoatMoveState.bashing) {
	    	goatState = (int)GoatMoveState.returning;
			coll.gameObject.SendMessage("HandleCollisionWithGoat");    		
    	}
    }

	void Update () {
		Vector3 deltaPosition = new Vector3();
		switch (goatState) {
			case (int)GoatMoveState.normal: {

				// 
				// regular up and down movement
				//

				bool movedVertically = false;
				if (Input.GetKey(moveUpKey)) {
					deltaPosition.y += goatSpeed * Time.deltaTime;
					movedVertically = true;
				} 
				if (Input.GetKey(moveDownKey)) {
					deltaPosition.y -= goatSpeed * Time.deltaTime;
					movedVertically = true;
				}			

				// 
				// handle if the bash key is used
				//

				if (!movedVertically) {
					if (Input.GetKeyDown(bashKey)) {
						// key initially pressed down
						goatBashPowerupValue = 0;

					} else if (Input.GetKey(bashKey)) {
						// key held down

						float amountToIncrementPowerup = 1 * (goatMaxBashPowerAmountInSeconds * Time.deltaTime);
						goatBashPowerupValue += amountToIncrementPowerup;
						if (goatBashPowerupValue > 1) {
							goatBashPowerupValue = 1;
						}

					} else if (Input.GetKeyUp(bashKey)) {
						// key released
						goatXVelocity = goatSpeed; // set an inital velocity for the bash
						goatState = (int)GoatMoveState.bashing;
					}
				}
				break;
			}
			case (int)GoatMoveState.bashing: {
				const float maxDistance = 12f; // how far can the goat go with full powerup
				float distanceOfThisBash = maxDistance * goatBashPowerupValue;

				if (transform.position.x < (goatInitalX + distanceOfThisBash)) {
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
				if (transform.position.x > goatInitalX) {
					deltaPosition.x -= (goatSpeed) * Time.deltaTime; // speed at which it returns
				} else {
					goatState = (int)GoatMoveState.normal;
				}

				break;
			}
		}

		// transform.position += deltaPosition;
		transform.Translate(deltaPosition);
	}

}
