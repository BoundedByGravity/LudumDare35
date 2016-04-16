using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody body;
	float moveSpeed = 20.0f;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	// Use this for initialization
	void Start () {
		body = this.gameObject.GetComponent<Rigidbody> ();
		//gravitySink = this.GetComponent<GravitySink> ();
		//prevAddedMoveVelocity = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool isPlanetbound() {
		float dist = Vector3.Distance (body.position, Vector3.zero);
		// TODO: Use radius from sphere
		return dist < 30;
	}

	void FixedUpdate() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		float alpha = horizontal;
		float beta = vertical;

		body.rotation = Quaternion.FromToRotation(Vector3.up, body.position);
		body.position = body.position.normalized * 26;
		
		if (isPlanetbound()) {
			Vector3 vec = new Vector3((body.position.y + body.position.z) * alpha, 
			                          -body.position.x * alpha - body.position.z * beta, 
			                          (body.position.x + body.position.y)* beta).normalized;
			//Vector3 direction = new Vector3 (horizontal, 0, vertical).normalized;
			body.velocity = vec * moveSpeed;
		}
	}
}
