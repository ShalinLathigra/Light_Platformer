using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour {
	Collider2D collide;
	Collider2D[] collisionArray = new Collider2D[5];
	public bool gravity;
	bool grounded = false;
	int num;
	bool left, right;
	void Update(){
		Transform points = (Transform)this.GetComponent<Transform>();

		if (!grounded) {
			print ("works");
			points.Translate (Vector3.down * Time.deltaTime);
		}
		if (left) {
			points.Translate (Vector3.left*2);
		}
		if (right) {
			points.Translate (Vector3.right*2);
		}
	}
	void move(){

	}
	void OnTriggerStay2D(Collider2D other){
		if (other.tag != "PlayerShot" && other.tag != "Player") {
			grounded = true;
		}
		Transform points = (Transform)this.GetComponent<Transform>();
		Transform p = (Transform)other.GetComponent<Transform>();

		if (p.position.x - points.position.x <1.1&&p.position.x - points.position.x>0) {
				left = true;
				right = false;
		} else if (points.position.x - p.position.x<1.1&&points.position.x - p.position.x>0) {
				right = true;
				left = false;
			}

	}
	void OnTriggerExit2D (Collider2D other){
		if (other.tag != "PlayerShot" && other.tag != "Player") {
			grounded = false;
		}
		if (other.tag == "Player") {
			left = false;
			right = false;
		}
	}
	void OnTriggerEnter2D (Collider2D other){
		Transform points = (Transform)this.GetComponent<Transform>();
		RaycastHit hit;


		collide = GetComponent<Collider2D>();
		string tag = this.tag;
		if (other.tag == "PlayerShot") {
			Shot shot = (Shot)other.GetComponent<Shot> ();

			if (tag.Contains ("Glass")) {
				//if object entering block is shot, will treat it as a shot
				if (tag.Contains ("AngleUpLeft")) {
					if (shot.direction.y == 1) {
						shot.direction = new Vector3 (-1, 0, 0);
					} else if (shot.direction.y == -1) {
						shot.direction = new Vector3 (1, 0, 0);
					} else if (shot.direction.x == 1) {
						shot.direction = new Vector3 (0, -1, 0);
					} else if (shot.direction.x == -1) {
						shot.direction = new Vector3 (0, 1, 0);
					}
				} else if (tag.Contains ("AngleUpRight")) {
					if (shot.direction.y == -1) {
						shot.direction = new Vector3 (-1, 0, 0);
					} else if (shot.direction.y == 1) {
						shot.direction = new Vector3 (1, 0, 0);
					} else if (shot.direction.x == -1) {
						shot.direction = new Vector3 (0, -1, 0);
					} else if (shot.direction.x == 1) {
						shot.direction = new Vector3 (0, 1, 0);
					}
				}
			}
			//print ("Worked");
		} else if (other.tag == "Player") {
			//an attempt to allow the player to move the block.
			Transform p = (Transform)other.GetComponent<Transform>();

			if (p.position.x - points.position.x <1.1&&p.position.x - points.position.x >0) {
					left = true;
				right = false;
			} else if (points.position.x - p.position.x<1.1 && points.position.x - p.position.x>0) {
				right = true;
				left = false;
			}
			
		} else {
			grounded = true;
		}
}
}