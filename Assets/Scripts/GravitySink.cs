using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravitySink : MonoBehaviour {
	Rigidbody body;
	Vector3 force;
	Vector3 prevForce;

	bool acceptsForce = true;
	bool autoApply = true;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}

	public void addForce(Vector3 force) {
		// TODO: There is no need to even calculate the force if it is never applied
		//       So the logic that calculates gravity should probably take the enabled
		//	     state of the GravitySink into consideration.
		if (acceptsForce) {
			this.force += force;
		}
	}

	public void setAcceptsForce(bool acceptsForce) {
		this.acceptsForce = acceptsForce;
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
			force = Vector3.zero;
		}
	}
}
