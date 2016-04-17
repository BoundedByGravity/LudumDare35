﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody body;
	// Initial trajectory vector, will change
	Vector3 trajectory = new Vector3 (0, 0, 1);

	// Constants
	float radius = 26;	// TODO: Load from radius of parent
	[Range (1,20)] public float moveSpeed;
	[Range (1,20)] public float jumpSpeed;
	float jumpspeed;


	float degree_rotation = 3;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	// Use this for initialization
	void Start () {
		moveSpeed = 7f;
		jumpSpeed = 3f;
		body = this.gameObject.GetComponent<Rigidbody> ();
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
		return dist < radius + 0.2;
	}

	void land() {
		body.position = body.position.normalized * (radius+0.1f);
	}

	Vector3 getPlayerUpOnPlanet(Vector3 planetPos) {
		return (this.transform.position - planetPos).normalized;
	}

	void FixedUpdate() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		bool jump = Input.GetButtonDown("Jump");

		bool isBound = isPlanetbound ();
		Vector3 up = getPlayerUpOnPlanet (Vector3.zero);

		//Debug.Log ("isBound: " + isBound + ", dist: " + Vector3.Distance(body.position, Vector3.zero));
		trajectory = (trajectory - Vector3.Project (trajectory, up)).normalized;
		body.rotation = Quaternion.LookRotation (trajectory, up);

		if (isBound) {
			// TODO: Better solution wanted, but better solution hard

			// Måns is love, Måns is life
			if (horizontal != 0) {
				Vector3 trajectory2 = -horizontal * Vector3.Cross (trajectory, up).normalized;
				trajectory = trajectory * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180);
			}

			body.velocity = (moveSpeed * vertical) * trajectory;

			land ();
			if (jump) {
				// TODO: Use planetpos instead of zero-vector
				// This line must be before the rest, for whatever reason.

				body.position += up * .2f;
				body.velocity += up * jumpSpeed;
			}

		}



	}
}
