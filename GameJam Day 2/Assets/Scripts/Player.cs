using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
	public float moveSpeed = 6;
	float gravity = -35;
	public float jumpVelocity = 15;
	float friction = 64f;
	float maxTime = 1f;
	float currentTime = 0f;

	public Vector3 vel;
	public GameObject wall;

	Controller2D controller;

	GameObject[] shots = new GameObject[5];
	GameObject shot_Prefab;
	int numShots = 0;
	int shotDirection = 1;
	float shotSpeed = 20;

	Vector2 dirPlayerToMouse;
	RaycastHit2D hit;
	RaycastHit mouseHit;

	void Start(){
		controller = GetComponent<Controller2D> ();
		shot_Prefab = (GameObject)(Resources.Load ("Prefabs/Shots/playerShot 1", typeof(GameObject)));
	} 
	void Update(){
		Transform f;
		Vector3 v = new Vector3 (0.0f, 2.5f, 0.0f);
		if (this.GetComponent<Transform>().position.y < -50) {
			this.GetComponent<Transform> ().SetPositionAndRotation (v, this.GetComponent<Transform> ().rotation);
			print ("Oh no, you fell to your inevitable doom in the void!!! Try again, maybe that'll work this time.");
		}

		Move ();
		if (Input.GetKeyDown (KeyCode.Space) && numShots < 5) {
			Shoot (shotDirection);
		}
		if (Input.GetMouseButtonDown (0) && currentTime == 0) {
			CheckTeleport ();
		}

		currentTime = Mathf.Max (0, currentTime - Time.deltaTime);
	}
	void CheckTeleport(){
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (tempRay, out mouseHit, 100f);
		Vector3 mousePosition = new Vector3 (mouseHit.point.x, mouseHit.point.y, 1f);
		Ray2D mouseRay = new Ray2D (transform.position + (mousePosition - transform.position) * .5f,  mousePosition - transform.position);
		Debug.DrawRay (transform.position + (mousePosition - transform	.position).normalized * .5f,  mousePosition - transform.position);

		hit = Physics2D.Raycast (mouseRay.origin, mouseRay.direction);
		if (hit.collider != null) {
			Vector3 target = new Vector3 (hit.point.x, hit.point.y, 1f);
			Teleport (target - (target - transform.position).normalized * 1f);
		} else {
			Teleport (new Vector3 (mousePosition.x, mousePosition.y, 1f));
		}
	}
	void Teleport(Vector3 targetPos){

		//Trigger Particle Effect
		float angleInRadians = Mathf.Atan2(transform.position.y - targetPos.y, transform.position.x - targetPos.x);

		Instantiate(Resources.Load("Prefabs/Explosion/PlayerLightTeleport"), transform.position, Quaternion.FromToRotation(targetPos, transform.position));

		targetPos.z = transform.position.z;
		transform.position = Vector3.MoveTowards (transform.position, targetPos, 500 * Time.deltaTime);
		currentTime = maxTime;
	}

	public void removeShot(){
		numShots--;
	}

	void Shoot(int direction){
		GameObject temp = (GameObject)Instantiate (shot_Prefab, this.transform.position + new Vector3(direction,0.2f,0.2f), shot_Prefab.transform.rotation);

		temp.GetComponent<Shot> ().parentObj = this.gameObject;
		temp.GetComponent<Shot> ().direction = new Vector3(0, 	direction, 0);
		temp.GetComponent<Shot> ().shotSpeed = shotSpeed;
		temp.GetComponent<Shot> ().shotSpeed = shotSpeed;

		shots [numShots++] = temp;
	}

	void Move(){
		if (controller.collisions.above || controller.collisions.below) {
			vel.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if ((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) && controller.canJump) {
			vel.y = jumpVelocity;
		}

		if (input.x != 0) {
			vel.x = input.x * moveSpeed;
		} else if (vel.x != 0) {
			vel.x -= Mathf.Sign (vel.x) * friction * Time.deltaTime;
			if (Mathf.Abs (vel.x) < .5f) {
				vel.x = 0;
			}
		}

		if (controller.canJump && vel.y < 0) {
			vel.y = 0;
		} else {
			vel.y += gravity * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			vel.y += 25 * gravity * Time.deltaTime;
		}
		controller.Move (vel * Time.deltaTime);
		if (vel.x != 0){
			shotDirection = (int)(vel.x / Mathf.Abs(vel.x));
		}
	}
}
