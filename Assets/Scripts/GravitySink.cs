﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravitySink : MonoBehaviour {

	bool autoApply = true;
	Rigidbody body;
	Vector3 force;
	Vector3 prevForce;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	public void addForce(Vector3 force) {
		this.force += force;
	}

	public void setAutoApply(bool autoApply) {
		this.autoApply = autoApply;
	}
	
	/*public Vector3 getForce() {
		return force == Vector3.zero? prevForce : force;
	}

	void FixedUpdate() {
		this.prevForce = force;
		this.force = Vector3.zero;
	}*/
	void FixedUpdate() {
		if (autoApply) {
			body.AddForce (force);
		}
		force = Vector3.zero;
	}
}
