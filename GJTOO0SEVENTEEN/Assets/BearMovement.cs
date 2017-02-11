using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : MonoBehaviour {

	private const float bearSpeed = 3f;
	private float bearXVelocity = 5f;
	private bool bearHasBeenHit = false;

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
		}
		transform.position += deltaPosition;

		const float screenLeft = -12f;
		const float screenWidth = 26f;
		if (transform.position.x < screenLeft) {
			Vector3 translation = new Vector3(screenWidth, 0, 0);
			transform.Translate(translation);
		}
	}

	void HandleCollisionWithGoat() {
		bearHasBeenHit = true;
		print("HandleCollisionWithGoat");
	}

}
