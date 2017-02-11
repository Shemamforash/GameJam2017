using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 

public class BearMovement : MonoBehaviour {

	private const float bearSpeed = 1.5f;
	private float bearXVelocity = 5f;
	private bool bearHasBeenHit = false;

	private bool enteredMapForFirstTime = false;
	private bool bearGotPastGoat = false;

    private Rigidbody2D rb;

	private const float timeBearDisappearsInSecs = 0.08f;
	private float bearHitTimer = 0f;

	public GameObject poofObject;

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		gameObject.tag = "Bear";

	}
	
    void OnCollisionEnter2D(Collision2D coll) {
    	if (coll.gameObject.tag == "Bear") {
    		// this should only happen when there has first been a collision with the goat otherwise
    		// the bears can kill each other by just walking around by themselves
			// BearHasBeenHit();
			DestroyBear();
    	}
    }

	void DestroyBear(){
		GameObject.Instantiate(poofObject, transform.position, transform.rotation);
		GameObject.Destroy(gameObject);
		print("Bear killed!!");
	}

	void Update () {
		Vector3 deltaPosition = new Vector3();
		deltaPosition.x -= bearSpeed * Time.deltaTime;

		if (bearHasBeenHit) {
			// bearXVelocity *= 1.4f; // acceleration
			// if (bearXVelocity > 100f) {
			// 	bearXVelocity = 100f;	
			// }	
			// deltaPosition.x += bearXVelocity * Time.deltaTime;			
			// bearHitTimer += Time.deltaTime;
			// if (bearHitTimer > timeBearDisappearsInSecs) {
			// 	gameObject.SetActive(false);
			// }
			DestroyBear();
			
		}
		transform.position += deltaPosition;

		float screenLeft = GameInfo.MetresToWorldX(0);
		float screenRight = GameInfo.GetWorldRight(); 
		float screenWidth = GameInfo.GetWorldWidth(); // TODO: it there a way to get this properly?

		if (transform.position.x < screenRight) {
			enteredMapForFirstTime = true;
		} else if (enteredMapForFirstTime && !bearHasBeenHit) {
			// the bear has been flown off the map somehow
			enteredMapForFirstTime = false;
			print("Bear killed by flying off the map");
			GameInfo.IncBearsKilled();
		}

		if (!bearGotPastGoat && transform.position.x < screenLeft) {
			// the bear has gotten past the goat!
			bearGotPastGoat = true;
			print("Bear got past!!");
			GameInfo.IncBearGotPast();
		}
	}

	public void BearHasBeenHit() {
		bearHasBeenHit = true;
		GameInfo.IncBearsKilled();
	}

	// void HandleCollisionWithGoat() {
		// BearHasBeenHit();
	// }
}
