using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofTimer : MonoBehaviour {
	private float timeSinceStart = 0f;
	private float maxTime = 0f;

	void Start() {
		maxTime = gameObject.GetComponent<ParticleSystem>().main.duration;
	}
	
	void Update () {
		timeSinceStart += Time.deltaTime;
		if(timeSinceStart > maxTime) {
			GameObject.Destroy(this);
		}
	}
}
