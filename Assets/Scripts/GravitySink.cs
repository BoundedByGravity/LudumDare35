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

	public void addForce(Vector3 force) {
		this.force += force;
	}

	public void setAutoApply(bool autoApply) {
		this.autoApply = autoApply;
	}
	
	Vector3 getForce() {
		return force;
	}

	void FixedUpdate() {
		if (autoApply) {
			this.body.AddForce (this.force);
		}
		this.force = Vector3.zero;
	}
}
