using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Image bashPowerupBar;
	public AudioSource goatSound;
	// public Text levelText;
	// public Text bearsText;

	enum GoatMoveState {normal, bashing, returning};
	private int goatState = (int)GoatMoveState.normal;
	public GameObject lavaPrefab;
	private const float goatSpeed = 8f;
	private float goatWalkSpeed;
	private float goatInitialXWorld; 

	private float goatMaxBashPowerAmountInSeconds = 4f;

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

    void OnTriggerStay2D(Collider2D coll) {
    	GameObject obj = coll.gameObject;
    	if (goatState == (int)GoatMoveState.bashing &&
    	    obj.transform.position.x >= transform.position.x) {

	    	goatState = (int)GoatMoveState.returning;
			BearMovement bm = obj.GetComponent<BearMovement>();
			bm.BearHasBeenHit();
    	}
    }

	float goatBash = 0;

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
				
				if(Input.GetKey(bashKey)){
					if (goatBashPowerupValue < 1) {
						goatBashPowerupValue += Time.deltaTime / goatMaxBashPowerAmountInSeconds;
					} else {
						goatBashPowerupValue = 1;
					}
				}

				if (Input.GetKeyUp(bashKey)) {
					// key released
					if(goatBashPowerupValue == 1){
						Vector3 offset = new Vector3(transform.position.x + 1f, transform.position.y, 0);
						GameObject.Instantiate(lavaPrefab, offset, transform.rotation);
					} else {
						goatXVelocity = goatSpeed; // set an inital velocity for the bash
						goatBash = goatBashPowerupValue;
						goatState = (int)GoatMoveState.bashing;
					}
					goatBashPowerupValue = 0;
				}
				break;
			}
			case (int)GoatMoveState.bashing: {
				const float maxDistanceMetres = 7f; // how far can the goat go with full powerup
				float minDistanceMetres = GameInfo.goatInitialXMetres + 1f; 

				float targetXPosMetresOfThisBash = minDistanceMetres + (maxDistanceMetres - minDistanceMetres) * goatBash;
				float distanceOfThisBash = GameInfo.MetresToWorldX(targetXPosMetresOfThisBash);

				if (transform.position.x < distanceOfThisBash) {
					goatXVelocity *= 1.5f; // acceleration
					if (goatXVelocity > 100f) {
						goatXVelocity = 100f;
					}
					deltaPosition.x = goatXVelocity * Time.deltaTime;
				} else {
					Vector3 newPos = new Vector3(distanceOfThisBash, transform.position.y, 0);
					transform.position = newPos;			
					goatState = (int)GoatMoveState.returning;
				}
				break;
			}
			case (int)GoatMoveState.returning: {
				if (transform.position.x > goatInitialXWorld) {
					// if (goatBashPowerupValue > 0f) {
						// goatBashPowerupValue -= 1f * Time.deltaTime;						
					// } else {
						goatBash = 0f;
					// }
					deltaPosition.x -= (goatWalkSpeed) * Time.deltaTime; // speed at which it returns
				} else {
					Vector3 newPos = new Vector3(goatInitialXWorld, transform.position.y, 0);
					transform.position = newPos;		
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

		float newYPosition = rb.position.y + deltaPosition.y;
		float worldTop = GameInfo.GetWorldTop();
		float worldBottom = GameInfo.MetresToWorldY(0);
		float distanceToTop = transform.localScale.y * 1.7f;
		float distanceToBottom = transform.localScale.y * 1.5f;
		float epsilon = 0.0001f;		

		if (newYPosition <= worldTop - distanceToTop && newYPosition >= worldBottom + distanceToBottom) {
			const float arbitraryModifier = 75f; // just picked because it feels about right...
			Vector3 velo = deltaPosition * arbitraryModifier;
			rb.velocity = velo;
		} else if (newYPosition > worldTop - distanceToTop) {
			rb.position = new Vector3(goatInitialXWorld, worldTop - distanceToTop - epsilon, 0);
		} else if (newYPosition < worldBottom + distanceToBottom) {
			rb.position = new Vector3(goatInitialXWorld, worldBottom + distanceToBottom + epsilon, 0);
		}
		bashPowerupBar.fillAmount = goatBashPowerupValue;
	}
}

