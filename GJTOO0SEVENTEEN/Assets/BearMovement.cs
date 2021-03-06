﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 

public class BearMovement : MonoBehaviour
{

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

    public void SetHealth(int health)
    {
        this.health = health;
    }

    private void SetColor()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        switch (health)
        {
            case 0: // he dead
                break;
            case 1:
                sr.color = Color.white;
                bearSpeed = 2f;
                break;
            case 2:
                sr.color = new Color(1f, 0.3f, 0f);
                bearSpeed = 1.25f;
                break;
            case 3:
                sr.color = Color.red;
                bearSpeed = 0.75f;
                break;
            default:
                Debug.Log("Bear has incorrect health" + health);
                break;
        }
        ChangeSpeed();
    }

    private void ChangeSpeed()
    {
        float rangeOfSpeed = 1f;
        float speedBuffer = 0.25f;
        float ranInRange = Random.Range(0f, rangeOfSpeed) - (rangeOfSpeed / 2);
        bearSpeed = bearSpeed + ranInRange + speedBuffer;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.tag = "Bear";

        // const float rangeOfSpeed = 2f;
        // bearSpeed = 2f + Random.value * rangeOfSpeed;
        SetColor();
    }


    void Update()
    {
        Vector3 deltaPosition = new Vector3();
        deltaPosition.x -= bearSpeed * Time.deltaTime;

        if (bearHasBeenHit)
        {
            // bearXVelocity *= 1.4f; // acceleration
            // if (bearXVelocity > 100f) {
            // 	bearXVelocity = 100f;	
            // }	
            // deltaPosition.x += bearXVelocity * Time.deltaTime;			
            // bearHitTimer += Time.deltaTime;
            // if (bearHitTimer > timeBearDisappearsInSecs) {
            // 	gameObject.SetActive(false);
            // }
            // HandleDamage();

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

        if (transform.position.x < screenRight)
        {
            enteredMapForFirstTime = true;
        }
        else if (enteredMapForFirstTime && !bearHasBeenHit)
        {
            // the bear has been flown off the map somehow
            enteredMapForFirstTime = false;
            // gameObject.SetActive(false);
            // print("Bear killed by flying off the map");
            // GameInfo.IncBearsKilled();
        }

        if (transform.position.x < GameInfo.MetresToWorldX(GameInfo.goatInitialXMetres))
        {
            bearSpeed *= 1.08f;
        }

        if (!bearGotPastGoat && transform.position.x < screenLeft)
        {
            // the bear has gotten past the goat!
            bearGotPastGoat = true;
            GameInfo.IncBearGotPast();
            GameObject.Destroy(gameObject);
        }
    }

    private bool bearDead =false;

    public bool BearHasBeenHit()
    {
        if (health != 0)
        {
            --health;
            SetColor();
            bearHasBeenHit = true;
        }
        if (health == 0 && !bearDead)
        {
            bearDead = true;
            GameObject.Instantiate(poofObject, transform.position, transform.rotation);
            GameInfo.IncBearsKilled();
            GameObject.Destroy(gameObject);
            return true;
        }
        return false;
    }

    // void HandleCollisionWithGoat() {
    // BearHasBeenHit();
    // }
}
