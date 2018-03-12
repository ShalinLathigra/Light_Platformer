using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask[] collisionMask;
	int numCollisionMasks;
	const float skinWidth = .015f;
	public int horizontalRayCount;
	public int verticalRayCount;
	private float horizontalRaySpacing;
	private float verticalRaySpacing;

	public bool canJump;

	BoxCollider2D collider;
	RaycastOrigins rayOrigins;
	public CollisionInfo collisions;
	void Start () {
		collider = GetComponent<BoxCollider2D> ();
		numCollisionMasks = collisionMask.Length;
		CalculateRaySpacing ();
	}
	
	public void Move (Vector3 velocity){
		UpdateRayCastOrigins ();
		collisions.Reset();
		if (velocity.x != 0){
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0){
			VerticalCollisions (ref velocity);
		}
		CheckIfCanJump ();
		transform.Translate (velocity);
	}

	void CheckIfCanJump()
	{
		canJump = collisions.below;

	}

	void HorizontalCollisions (ref Vector3 velocity)
	{
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int j = 0; j < numCollisionMasks; j++) {
			for (int i = 0; i < horizontalRayCount; i++) {
				Vector2 rayOrigin = (directionX == -1) ? rayOrigins.botLeft : rayOrigins.botRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask[j]);

				Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength * 5, Color.green);

				if (hit) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}

	void VerticalCollisions (ref Vector3 velocity)
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int j = 0; j < numCollisionMasks; j++) {
			for (int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = (directionY == -1) ? rayOrigins.botLeft : rayOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask [j]);

				Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength * 5, Color.red);

				if (hit) {
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;


					collisions.below = directionY == -1;
					collisions.above = directionY == 1;
				}
			}
		}
	}

	void CalculateRaySpacing ()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.y / (verticalRayCount - 1)	;
	}

	void UpdateRayCastOrigins ()
	{		
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		rayOrigins.botLeft = new Vector2 (bounds.min.x, bounds.min.y);
		rayOrigins.botRight = new Vector2 (bounds.max.x, bounds.min.y);
		rayOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		rayOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	struct RaycastOrigins{
		public Vector2 topLeft, topRight;
		public Vector2 botLeft, botRight;
	}

	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;

		public void Reset(){
			above = below = false;
			left = right = false;
		}
	}
		
}
