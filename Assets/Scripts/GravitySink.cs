using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravitySink : MonoBehaviour {

	bool autoApply = true;
	Rigidbody body;
	Vector3 force;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	void setForce(Vector3 force) {
		this.force = force;
	}
	
	Vector3 getForce() {
		return force;
	}

	void setAutoApply(bool autoApply) {
		this.autoApply = autoApply;
	}

	void FixedUpdate() {
		if (autoApply) {
			this.body.AddForce (this.force);
		}
	}
}
