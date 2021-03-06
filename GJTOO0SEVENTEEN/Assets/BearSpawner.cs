﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BearSpawner : MonoBehaviour {
	public GameObject bearPrefab;
	public Text levelNo, bearsKilled;

	private GameObject[] bears;
	private float timeCounterSeconds = 0f;
	private float timeUntilNextSpawn = 0.8f;
	private int currentBearIndex = 0;
	private int numBears;

	// Use this for initialization
	void Start () {
		levelNo.text = "Level: " + GameInfo.GetLevelNo();
		numBears = GameInfo.GetNumBears();
		bears = new GameObject[numBears];
	}
	
	void Update () {
		bearsKilled.text = "Bears Killed: " + GameInfo.GetBearsKilledThisLevel() + "/" + numBears;

		timeCounterSeconds += Time.deltaTime;

		if (timeCounterSeconds > timeUntilNextSpawn && currentBearIndex < numBears) {
			timeCounterSeconds = 0;
			timeUntilNextSpawn = 0.4f + Random.value * 0.6f;

			float screenRight = GameInfo.GetWorldRight(); 
			float screenBottom = GameInfo.MetresToWorldY(0); 
			float screenHeight = GameInfo.GetWorldHeight(); 
			float screenTop = GameInfo.GetWorldTop();
			const float xScatterOfBearsInMetres = 6f;
			float rangeOfXScatter = GameInfo.MetresToWorldX(xScatterOfBearsInMetres);

			float distanceToTop = transform.localScale.y * 1.7f;
			float distanceToBottom = transform.localScale.y * 1.5f;

			bears[currentBearIndex] = new GameObject();
			Vector3 position = new Vector3(screenRight, 
			                               screenBottom + distanceToBottom + Random.value * (screenHeight - distanceToTop - distanceToBottom), 
			                               0);
			Quaternion rotation = new Quaternion(0, 0, 0, 0);
			bears[currentBearIndex] = Object.Instantiate(bearPrefab, position, rotation);
			int ran = Random.Range(0, 10);
			int levelOffest = (int)Mathf.Floor((float)GameInfo.GetLevelNo() / 5f);
			if(ran < 7 - levelOffest){
				ran = 1;
			} else if(ran < 9 - levelOffest){
				ran = 2;
			} else {
				ran = 3;
			}
			bears[currentBearIndex].GetComponent<BearMovement>().SetHealth(ran);
			++currentBearIndex;
		}
	}
}
