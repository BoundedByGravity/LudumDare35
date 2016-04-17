using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class GravitySource : MonoBehaviour {

	HashSet<GravitySink> sinkObjects;

	float gravityConstant = 1;

	// Use this for initialization
	void Start () {
		sinkObjects = new HashSet<GravitySink> ();
		SphereCollider[] colliders = gameObject.GetComponents<SphereCollider> ();
		SphereCollider collider = null;
		foreach(SphereCollider col in colliders) {
			if (col.isTrigger)
				collider = col;
		}
		if(collider == null)
			Debug.LogError("GravitySource for " + gameObject.name + " has none SphereTriggerCollider");
		foreach (Collider hit in Physics.OverlapSphere (transform.position, collider.radius)) {
			GravitySink sinkObj = hit.gameObject.GetComponent<GravitySink> ();
			if(sinkObj != null && hit.gameObject != this.gameObject)
				sinkObjects.Add(sinkObj);
		}
	}

	// Update is called often
	void FixedUpdate () {
		foreach (GravitySink sinkObj in sinkObjects) {
			Rigidbody body = sinkObj.gameObject.GetComponent<Rigidbody> ();
			Vector3 normVectorToPlayer = (this.transform.position - body.transform.position).normalized;
			float dist = Vector3.Distance (this.transform.position, body.transform.position);
			Vector3 force = normVectorToPlayer * gravityConstant * gameObject.GetComponent<Rigidbody> ().mass * body.mass / (dist * dist);
			sinkObj.addForce (force);
		}
	}

	void OnTriggerEnter(Collider collider) {
		GravitySink sinkObj = collider.gameObject.GetComponent<GravitySink> ();
		//Debug.Log ("Trigger enter: " + sinkObj);
		if (sinkObj != null) {
			sinkObjects.Add (sinkObj);
		}
	}

	void OnTriggerExit(Collider collider) {
		GravitySink sinkObj = collider.gameObject.GetComponent<GravitySink> ();
		Debug.Log ("Trigger exit: " + sinkObj);
		//body.AddForce (Vector3.zero); // Remove force?
		if (sinkObj != null) {
			sinkObjects.Remove (sinkObj);
		}
	}
}
