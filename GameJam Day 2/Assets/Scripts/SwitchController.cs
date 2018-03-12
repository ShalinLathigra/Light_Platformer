using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {

	public bool active = false;
	Material[] material = new Material[1];

	void OnTriggerEnter2D (Collider2D other){
		if (other.tag.Equals ("PlayerShot")) {
			if (!active) {
				active = true;
				GetComponent<Light> ().color = Color.green;
				material [0] = (Material)Resources.Load ("Materials/On", typeof(Material));
				GetComponent<Renderer> ().materials = material;	
			} else {
				active = false;
				GetComponent<Light> ().color = Color.red;
				material [0] = (Material)Resources.Load ("Materials/Off", typeof(Material));
				GetComponent<Renderer> ().materials = material;	
			}
		}
	}
}
