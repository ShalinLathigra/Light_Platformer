using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (BoxCollider2D))]
public class TestBlockScript : MonoBehaviour {
	Controller2D controller;
	Vector3 vel = new Vector3();
	float gravity = -35;
	public bool canMove;
	// Use this for initialization
	BoxCollider2D collider;
	void Start () {
		controller = GetComponent<Controller2D>();
		collider = GetComponent<BoxCollider2D> ();
		if (canMove) {
			GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		} else {
			GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
		}
	}
	// Update is called once per frame
	void Update () {
		if (canMove) {
			if (controller.collisions.above || controller.collisions.below) {
				vel.y = 0;
			}
			vel.y += gravity * Time.deltaTime;
			controller.Move (vel * Time.deltaTime);
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.collider.tag.Equals("Player") && canMove) {
			if (controller.collisions.left || controller.collisions.right) {
				Player player = other.gameObject.GetComponent<Player> ();
				int direction = (int)(player.vel.x / Mathf.Abs (player.vel.x));
				vel.x = player.moveSpeed * direction * 1000f;
			}
		}
	}
}
