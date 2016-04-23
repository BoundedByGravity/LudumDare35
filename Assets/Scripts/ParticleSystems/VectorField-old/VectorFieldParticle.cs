using UnityEngine;
using System.Collections;

public class VectorFieldParticle : MonoBehaviour {

	Rigidbody body;
	float speed;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		float speedCoeff = 5;
		float maxSpeed = 2;
		float levels = 1000;
		speed = speedCoeff*Random.Range (1, levels)*(maxSpeed/levels);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log ("Update");
		//Vector3 trajectory = CircleField (transform.position.x, transform.position.z);
		Vector3 trajectory = VectorFields.ConeField (transform.position);
		Debug.DrawRay (body.position, trajectory);
		body.position += trajectory / 100 * speed;
		//body.velocity = Vector3.forward;
		//body.position += Vector3.forward/100;
	}
}
