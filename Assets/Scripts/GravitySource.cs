using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class GravitySource : MonoBehaviour {

	HashSet<GravitySink> sinkObjects;

	float gravityConstant = 1;
	float sourceRadius;

	// Use this for initialization
	void Start () {
		sinkObjects = new HashSet<GravitySink> ();
		AddAllSinks ();
	}

	void AddAllSinks() {
		foreach (GravitySink sink in GameObject.FindObjectsOfType<GravitySink>()) {
			sinkObjects.Add(sink);
			Debug.Log(this + ", " + sink);
		}
	}

	// Update is called often
	void FixedUpdate () {
		foreach (GravitySink sinkObj in sinkObjects) {
			Rigidbody body = sinkObj.gameObject.GetComponent<Rigidbody> ();
			Vector3 normVectorToPlayer = (this.transform.position - body.transform.position).normalized;
			float dist = Vector3.Distance (this.transform.position, body.transform.position);
			Vector3 force = normVectorToPlayer * gravityConstant * gameObject.GetComponent<Rigidbody> ().mass * body.mass / (dist * Mathf.Sqrt(dist));
			sinkObj.addForce (force);
		}
	}
}
