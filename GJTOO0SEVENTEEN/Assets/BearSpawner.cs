using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour {
	public GameObject bearPrefab;

	private GameObject[] bears;
	private float timeCounterSeconds = 0f;
	private float timeUntilNextSpawn = 0.8f;
	private int currentBearIndex = 0;
	private int numBears;

	void Start () {
		numBears = GameInfo.GetNumBears();
		bears = new GameObject[numBears];
	}
	
	void Update () {
		timeCounterSeconds += Time.deltaTime;

		if (timeCounterSeconds > timeUntilNextSpawn && currentBearIndex < numBears) {
			timeCounterSeconds = 0;
			timeUntilNextSpawn = 0.4f + Random.value * 0.6f;

			float screenRight = GameInfo.GetWorldRight(); 
			float screenBottom = GameInfo.GetWorldBottom(); 
			float screenHeight = GameInfo.GetWorldHeight(); 
			const float xScatterOfBearsInMetres = 6f;
			float rangeOfXScatter = GameInfo.MetresToWorldX(xScatterOfBearsInMetres);

			bears[currentBearIndex] = new GameObject();
			Vector3 position = new Vector3(screenRight, 
			                               screenBottom + Random.value * screenHeight, 
			                               0);
			Quaternion rotation = new Quaternion(0, 0, 0, 0);
			bears[currentBearIndex] = Object.Instantiate(bearPrefab, position, rotation);
			++currentBearIndex;
		}
	}
}
