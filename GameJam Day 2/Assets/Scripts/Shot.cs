using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
	public GameObject parentObj;
	public Vector3 direction;
	public float shotSpeed;

	bool returning;
	float maxLifeTimer = 1f;
	float glassLifeIncrease = .5f;
	float lifeTimer;

	// Use this for initialization
	void Start () {
		returning = false;
		lifeTimer = maxLifeTimer;
	}

	// Update is called once per frame
	void Update () {
		lifeTimer = Mathf.Max (0, lifeTimer - Time.deltaTime);
		if (lifeTimer > 0 && !returning) {
			transform.Translate (shotSpeed * direction * Time.deltaTime);
		} else {
			transform.position += shotSpeed / 2 * (parentObj.transform.position - transform.position) * Time.deltaTime;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (!col.collider.tag.Equals (this.tag)) {
			if (col.collider.tag.Equals ("Player")) {
				parentObj.GetComponent<Player> ().removeShot ();
				Destroy (this.gameObject);
			} else if (col.collider.tag.Contains("Glass")){
				lifeTimer += glassLifeIncrease;
				string tag = col.collider.tag;

				if(tag.Contains("AngleUpLeft")){
					if (direction.y == 1) {
						direction = new Vector3 (-1, 0, 0);
					} else if (direction.y == -1) {
						direction = new Vector3 (1, 0, 0);
					} else if (direction.x == 1) {
						direction = new Vector3 (0, -1, 0);
					} else if (direction.x == -1) {
						direction = new Vector3 (0, 1, 0);
					}
				}else if (tag.Contains("AngleUpRight")){
					if (direction.y == -1) {
						direction = new Vector3 (-1, 0, 0);
					} else if (direction.y == 1) {
						direction = new Vector3 (1, 0, 0);
					} else if (direction.x == -1) {
						direction = new Vector3 (0, -1, 0);
					} else if (direction.x == 1) {
						direction = new Vector3 (0, 1, 0);
					}
				}else if(tag.Contains("Clear")){

				}
			} else {
				Instantiate (Resources.Load ("Prefabs/Explosion/PlayerLightExplosion", typeof(GameObject)), transform.position, transform.rotation);
				returning = true;
			}
		} else {
			
		}
	}
}