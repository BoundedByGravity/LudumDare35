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
		if(collider == null)
			Debug.LogError("GravitySource for " + gameObject.name + " has none SphereTriggerCollider");
		foreach (Collider hit in Physics.OverlapSphere (transform.position, collider.radius)) {
			Rigidbody body = hit.gameObject.GetComponent<Rigidbody> ();
			if(body != null && hit.gameObject != this.gameObject)
				bodies.Add(body);
		}
	}

	// Update is called often
	void FixedUpdate () {
		foreach (Rigidbody body in bodies) {
			Vector3 normVectorToPlayer = (this.transform.position - body.transform.position).normalized;
			float dist = Vector3.Distance (this.transform.position, body.transform.position);
			Vector3 force = normVectorToPlayer * gravityConstant * gameObject.GetComponent<Rigidbody> ().mass * body.mass / (dist * dist);
			body.AddForce (force);
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
