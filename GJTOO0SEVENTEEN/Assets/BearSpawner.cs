using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour {
	public GameObject bearPrefab;

	private GameObject[] bears;

	// Use this for initialization
	void Start () {

		int numBears = GameInfo.GetNumBears();
		bears = new GameObject[numBears];

		for (int i = 0; i < numBears; ++i) {
			bears[i] = new GameObject();

			float screenRight = GameInfo.GetWorldRight(); 
			float screenBottom = GameInfo.GetWorldBottom(); 
			float screenHeight = GameInfo.GetWorldHeight(); 
			const float xScatterOfBearsInMetres = 3f;
			float rangeOfXScatter = GameInfo.MetresToWorldX(xScatterOfBearsInMetres);
			Vector3 position = new Vector3(screenRight + Random.value * rangeOfXScatter, 
			                               screenBottom + Random.value * screenHeight, 
			                               0);
			
			Quaternion rotation = new Quaternion(0, 0, 0, 0);
			bears[i] = Object.Instantiate(bearPrefab, position, rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
