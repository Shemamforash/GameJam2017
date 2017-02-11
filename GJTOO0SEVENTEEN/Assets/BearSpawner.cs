using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour {
	public GameObject bearPrefab;

	private const int numBears = 4;
	private GameObject[] bears = new GameObject[numBears];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numBears; ++i) {
			bears[i] = new GameObject();
			const float screenRight = 12f; // TODO: Can we get this value properly?
			const float screenBottom = -5f; // TODO: Can we get this value properly?
			const float screenHeight = 10f; // TODO: Can we get this value properly?
			const float rangeOfXScatter = 5f;
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
