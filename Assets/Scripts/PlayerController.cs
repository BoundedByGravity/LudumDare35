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
	float moveSpeed = 7f;
	float jumpSpeed = 3f;
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
		return dist < radius + 0.1;
	}

	void land() {
		body.position = body.position.normalized * radius;
	}

	float getNextHeight(float height, float jumpspeed) {
		return Mathf.Max (0, height + jumpspeed);
	}

	Vector3 getPlayerUpOnPlanet(Vector3 planetPos) {
		return (this.transform.position - planetPos).normalized;
	}

	void FixedUpdate() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		bool jump = Input.GetButtonDown("Jump");

		bool isBound = isPlanetbound ();

		//Debug.Log ("isBound: " + isBound + ", dist: " + Vector3.Distance(body.position, Vector3.zero));

		if (isBound) {
			if (jump) {
				// TODO: Use planetpos instead of zero-vector
				// This line must be before the rest, for whatever reason.

				Vector3 up = getPlayerUpOnPlanet(Vector3.zero);
				body.position += up*.2f;
				body.velocity += up*jumpSpeed;
			} else {
				// TODO: Better solution wanted, but better solution hard

				// Måns is love, Måns is life
				Vector3 trajectory2 = -horizontal * Vector3.Cross (trajectory, body.position).normalized;
				trajectory = trajectory * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180);

				trajectory = (trajectory - Vector3.Project (trajectory, body.position)).normalized;
				body.velocity = (moveSpeed * vertical) * trajectory;
				
				land ();
			}
		}

		trajectory = (trajectory - Vector3.Project (trajectory, body.position)).normalized;
		body.gameObject.transform.rotation = Quaternion.LookRotation (trajectory, body.position);
	}
}
