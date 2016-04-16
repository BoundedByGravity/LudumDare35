using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody body;
	GravitySink gravitySink;

	// Initial trajectory vector, will change
	Vector3 trajectory = new Vector3 (0, 0, 1);

	// Constants
	float radius = 26;	// TODO: Load from radius of parent
	float moveSpeed = 3f;
	float startjumpspeed = 1f;
	float deaccelerationofjump = 0.1f;
	bool jumping;
	float jumpspeed;
	float height;
	float goingfwd;
	Vector3 surfpos;


	float degree_rotation = 3;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	// Use this for initialization
	void Start () {
		body = this.gameObject.GetComponent<Rigidbody> ();
		gravitySink = this.gameObject.GetComponent<GravitySink> ();
		//jumping = false;
		//prevAddedMoveVelocity = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: Figure out what should go here
	}

	bool isPlanetbound() {
		// TODO: Use radius from closest planet or use a trigger to detect landing on a planet
		float dist = Vector3.Distance (body.position, Vector3.zero);
		return dist < radius + 1;
	}

	void land() {
		body.position = body.position.normalized * radius;
	}

	float getNextHeight(float height, float jumpspeed) {
		return Mathf.Max (0, height + jumpspeed);
	}

	void FixedUpdate() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		bool jump = Input.GetKeyDown("space");

		bool isBound = isPlanetbound ();

		Debug.Log ("isBound: " + isBound + ", dist: " + Vector3.Distance(body.position, Vector3.zero));

		// This line must be before the rest, for whatever reason.
		body.gameObject.transform.rotation = Quaternion.LookRotation (trajectory, body.position);

		if (isBound && jump) {
			// Must be "up" relative to player
			body.position += new Vector3 (0, 2, 0);
			body.velocity = new Vector3(0, 5, 0);
		} else {
			if (isBound) {
				// Måns is love, Måns is life

				//if (horizontal != 0) {
					Vector3 trajectory2 = -horizontal * Vector3.Cross (trajectory, body.position.normalized);
					trajectory = trajectory * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180);
				/**}
				if (vertical != 0) {*/
					body.velocity = (moveSpeed * vertical) * trajectory;
					trajectory = (trajectory - Vector3.Project (trajectory, body.position)).normalized;
				//}

				//land ();
			}
		}
	}
}
