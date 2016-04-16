using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class GravitySource : MonoBehaviour {

	HashSet<Rigidbody> bodies;

	float gravityConstant = 1;

	// Use this for initialization
	void Start () {
		bodies = new HashSet<Rigidbody> ();
		SphereCollider[] colliders = gameObject.GetComponents<SphereCollider> ();
		SphereCollider collider = null;
		foreach(SphereCollider col in colliders) {
			if (col.isTrigger)
				collider = col;
		}
		foreach (Collider hit in Physics.OverlapSphere (transform.position, collider.radius)) {
			if(hit.gameObject != this.gameObject)
				bodies.Add(hit.gameObject.GetComponent<Rigidbody>());
		}
	}

	void Update() {
		Debug.Log(bodies.Count);
	}

	// Update is called often
	void FixedUpdate () {
		if (bodies.Count > 0) {
			foreach (Rigidbody body in bodies) {
				Vector3 normVectorToPlayer = (this.transform.position - body.transform.position).normalized;
				float dist = Vector3.Distance (this.transform.position, body.transform.position);
				Vector3 force = normVectorToPlayer * gravityConstant * gameObject.GetComponent<Rigidbody> ().mass * body.mass / (dist * dist);
				body.AddForce (force);
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		Rigidbody body = collider.gameObject.GetComponent<Rigidbody> ();
		if (body != null) {
			bodies.Add (body);
		}
	}

	void OnTriggerExit(Collider collider) {
		Rigidbody body = collider.GetComponent<Rigidbody> ();
		//body.AddForce (Vector3.zero); // Remove force?
		bodies.Remove (body);
	}
}
