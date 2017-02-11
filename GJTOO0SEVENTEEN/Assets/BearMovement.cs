using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 

public class BearMovement : MonoBehaviour {

	private const float bearSpeed = 3f;
	private float bearXVelocity = 5f;
	private bool bearHasBeenHit = false;

	private const float timeBearDisappearsInSecs = 0.5f;
	private float bearHitTimer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deltaPosition = new Vector3();
		deltaPosition.x -= bearSpeed * Time.deltaTime;

		if (bearHasBeenHit) {
			bearXVelocity *= 1.4f; // acceleration
			if (bearXVelocity > 100f) {
				bearXVelocity = 100f;	
			}
			deltaPosition.x += bearXVelocity * Time.deltaTime;			
			bearHitTimer += bearHitTimer;
			if (bearHitTimer > timeBearDisappearsInSecs) {

			}
		}
		transform.position += deltaPosition;

		const float screenLeft = -20f; // TODO: it there a way to get this properly?
		const float screenWidth = 26f; // TODO: it there a way to get this properly?

		if (transform.position.x < screenLeft) {
			// the bear has got past!
			if (GameInfo.IncBearGotPast()) {
				SceneManager.LoadScene("Level Up Scene");
			}
		}
	}

	void HandleCollisionWithGoat() {
		bearHasBeenHit = true;
		++GameInfo.bearsKilled;
		print("HandleCollisionWithGoat");
	}
}
