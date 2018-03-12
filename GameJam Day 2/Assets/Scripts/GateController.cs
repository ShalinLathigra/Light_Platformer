using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

	public SwitchController[] switches;
	public float moveSpeed;
	float maxLifeTime = .5f;
	float lifeTime = -1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool done = true;
		for (int i = 0; i < switches.Length; i++) {
			done = done && switches [i].active;
		}
		if (done && lifeTime < 0f) {
			GetComponent<BoxCollider2D> ().isTrigger = true;
			lifeTime = maxLifeTime;
		}
		if (lifeTime > 0f) {
			transform.position += transform.rotation * Vector3.up * moveSpeed * Time.deltaTime;
			lifeTime = Mathf.Max (0f, lifeTime - Time.deltaTime);
		}
	}
}
