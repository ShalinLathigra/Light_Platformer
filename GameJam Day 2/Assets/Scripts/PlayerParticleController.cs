using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour {

	float lifeTime;
	// Use this for initialization
	void Awake () {
		lifeTime = GetComponent<ParticleSystem>().main.duration + .5f;
	}

	// Update is called once per frame
	void Update () {
		lifeTime = Mathf.Max (0, lifeTime - Time.deltaTime);
		if (lifeTime == 0f) {
			Destroy (this.gameObject);
		}
	}
}