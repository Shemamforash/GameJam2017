using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Image bashPowerupBar;
	public Text levelText;
	public Text bearsText;

	enum GoatMoveState {normal, bashing, returning};
	private int goatState = (int)GoatMoveState.normal;

	private const float goatSpeed = 8f;
	private float goatInitialXMetres = 1f;
	private float goatInitialXWorld; 

	private const float goatMaxBashPowerAmountInSeconds = 1f;

	private const string moveUpKey = "w";
	private const string moveDownKey = "s";
	private const string bashKey = "space";

	private float goatXVelocity = 0; // velocity of goat as it is bashing
	private float goatBashPowerupValue = 0f; // [0, 1]

    private Rigidbody2D rb;

    void FixedUpdate() {
    }

	// Use this for initialization
	void Start () {
		goatInitialXWorld = GameInfo.MetresToWorldX(goatInitialXMetres);
		rb = gameObject.GetComponent<Rigidbody2D>();
		transform.localPosition = new Vector3(goatInitialXWorld, 0, 0);
	}

    void OnCollisionEnter2D(Collision2D coll) {
    	GameObject obj = coll.gameObject;
    	if (goatState == (int)GoatMoveState.bashing &&
    	    obj.transform.position.x > transform.position.x) {
	    	goatState = (int)GoatMoveState.returning;
			obj.SendMessage("HandleCollisionWithGoat");    		
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
				break;
			}
			case (int)GoatMoveState.bashing: {
				const float maxDistance = 12f; // how far can the goat go with full powerup
				float distanceOfThisBash = maxDistance * goatBashPowerupValue;

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
					if (goatBashPowerupValue > 0f) {
						goatBashPowerupValue -= 1f * Time.deltaTime;						
					} else {
						goatBashPowerupValue = 0f;
					}
					deltaPosition.x -= (goatSpeed) * Time.deltaTime; // speed at which it returns
				} else {
					goatState = (int)GoatMoveState.normal;
				}

				break;
			}
		}

		const float arbitraryModifier = 75f; // just picked because it feels about right...
		Vector3 velo = deltaPosition * arbitraryModifier;
		rb.velocity = velo;

		bashPowerupBar.fillAmount = goatBashPowerupValue;

		// levelText.text = "Levelll";

	}


}

