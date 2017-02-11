using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 

public class BearMovement : MonoBehaviour {

	private const float bearSpeed = 3f;
	private float bearXVelocity = 5f;
	private bool bearHasBeenHit = false;

	private const float timeBearDisappearsInSecs = 0.08f;
	private float bearHitTimer = 0f;

	void Start () {
		gameObject.tag = "Bear";
	}
	
    void OnCollisionEnter2D(Collision2D coll) {
    	if (coll.gameObject.tag == "Bear") {
			BearHasBeenHit();
    	}
    }

	void Update () {
		Vector3 deltaPosition = new Vector3();
		deltaPosition.x -= bearSpeed * Time.deltaTime;

		if (bearHasBeenHit) {
			bearXVelocity *= 1.4f; // acceleration
			if (bearXVelocity > 100f) {
				bearXVelocity = 100f;	
			}	
			deltaPosition.x += bearXVelocity * Time.deltaTime;			
			bearHitTimer += Time.deltaTime;
			if (bearHitTimer > timeBearDisappearsInSecs) {
				gameObject.SetActive(false);
			}
		}
		transform.position += deltaPosition;

		const float screenLeft = -20f; // TODO: it there a way to get this properly?
		const float screenWidth = 26f; // TODO: it there a way to get this properly?

		if (transform.position.x < screenLeft) {
			// the bear has gotten past the goat!
			if (GameInfo.IncBearGotPast()) {
				SceneManager.LoadScene("Level Up Scene");
			}
		}
	}

	void BearHasBeenHit() {
		bearHasBeenHit = true;
		++GameInfo.bearsKilled;
	}

	void HandleCollisionWithGoat() {
		BearHasBeenHit();
	}
}
