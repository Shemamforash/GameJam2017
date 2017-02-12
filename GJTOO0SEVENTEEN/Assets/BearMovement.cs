using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 

public class BearMovement : MonoBehaviour {

	private float bearSpeed = 1.5f;
	private float bearXVelocity = 5f;
	private bool bearHasBeenHit = false;

	private bool enteredMapForFirstTime = false;
	private bool bearGotPastGoat = false;

    private Rigidbody2D rb;

	private const float timeBearDisappearsInSecs = 0.08f;
	private float bearHitTimer = 0f;

	public GameObject poofObject;

	private int health = 1;

	public void SetHealth(int health){
		this.health = health;
	}

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		gameObject.tag = "Bear";

		const float rangeOfSpeed = 2f;
		bearSpeed = 2f + Random.value * rangeOfSpeed;
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
		if(health == 0){
			GameObject.Instantiate(poofObject, transform.position, transform.rotation);
			GameObject.Destroy(gameObject);
			print("Bear killed!!");
		} else {
			--health;
		}
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

		// if (rb.velocity.x > 0 && bearHitTimer != 0) {
		// 	// bit of a hack to detect when the bear has been hit (perhaps by the physics engine)
		// 	print("greater than zero");
		// 	bearHasBeenHit = true;
		// }

		float screenLeft = GameInfo.MetresToWorldX(0);
		float screenRight = GameInfo.GetWorldRight(); 
		float screenWidth = GameInfo.GetWorldWidth();

		if (transform.position.x < screenRight) {
			enteredMapForFirstTime = true;
		} else if (enteredMapForFirstTime && !bearHasBeenHit) {
			// the bear has been flown off the map somehow
			enteredMapForFirstTime = false;
			// gameObject.SetActive(false);
			// print("Bear killed by flying off the map");
			// GameInfo.IncBearsKilled();
		}

		if (transform.position.x < GameInfo.MetresToWorldX(GameInfo.goatInitialXMetres)) {
			bearSpeed *= 1.1f;
		}

		if (!bearGotPastGoat && transform.position.x < screenLeft) {
			// the bear has gotten past the goat!
			bearGotPastGoat = true;
			gameObject.SetActive(false);
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
