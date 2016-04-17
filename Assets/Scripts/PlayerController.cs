using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody body;
	float radius = 26;
	Vector3 trajectory = new Vector3 (0, 0, 1);
	float moveSpeed = 0.5f;
	float startjumpspeed = 1f;
	float deaccelerationofjump = 0.1f;
	bool jumping;
	float jumpspeed;
	float height;
	float goingfwd;
	Vector3 surfpos;


	float degree_rotation = 3;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	// Use this for initialization
	void Start () {
		body = this.gameObject.GetComponent<Rigidbody> ();
		jumping = false;
		//gravitySink = this.GetComponent<GravitySink> ();
		//prevAddedMoveVelocity = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

	}

	/*bool isPlanetbound() {
		float dist = Vector3.Distance (body.position, Vector3.zero);
		// TODO: Use radius from sphere
		return dist < radius+10;
	}

	void land() {
		body.position = body.position.normalized * radius;
	}*/

	float getNextHeight(float height, float jumpspeed) {
		return Mathf.Max (0, height + jumpspeed);
	}

	void FixedUpdate() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		bool jump = Input.GetKeyDown("space");



		if (!jumping) {
			// Måns is love, Måns is life
			if (horizontal != 0) {
				Vector3 trajectory2 = -horizontal * Vector3.Cross(trajectory, body.position.normalized);
				trajectory = trajectory*Mathf.Cos (degree_rotation*Mathf.PI/180) + trajectory2*Mathf.Sin(degree_rotation*Mathf.PI/180);
			}
			if (vertical != 0) {
				body.position += (moveSpeed * vertical) * trajectory;
				trajectory = (trajectory - Vector3.Project (trajectory, body.position)).normalized;
			}
			//body.rotation = Quaternion.FromToRotation(Vector3.forward, trajectory);
			Debug.Log (Vector3.Dot (trajectory, body.position));
			body.gameObject.transform.rotation = Quaternion.LookRotation(trajectory, body.position);
			Debug.Log (body.gameObject.transform.rotation);

			if (jump) {
				jumping = true;
				goingfwd = vertical;
				jumpspeed = startjumpspeed;
				height = getNextHeight (0, jumpspeed);
				surfpos = body.position;
				body.position = surfpos * (radius + height) / radius;
			}
			Debug.Log (Vector3.Dot (trajectory, body.position));
			body.gameObject.transform.rotation = Quaternion.LookRotation(trajectory, body.position);
			Debug.Log (body.gameObject.transform.rotation);

			//land ();
		} else {
			if (goingfwd != 0) {
				surfpos += (moveSpeed * goingfwd) * trajectory;
				trajectory = (trajectory - Vector3.Project (trajectory, surfpos)).normalized;
			}
			jumpspeed -= deaccelerationofjump;
			height = getNextHeight (height, jumpspeed);
			body.position = surfpos * (radius + height) / radius;
			if (height == 0) {
				jumping = false;
			}
			Debug.Log (Vector3.Dot (trajectory, body.position));
			body.gameObject.transform.rotation = Quaternion.LookRotation(trajectory, body.position);
			Debug.Log (body.gameObject.transform.rotation);
		}
	}

	void MovePlayer1(float vertical, float horizontal) {
		// Broken	
		if(vertical != 0) {
			body.rotation *= new Quaternion(Mathf.Sin(vertical), 0, 0, Mathf.Cos(vertical));
		}
		
		if(horizontal != 0) {
			body.rotation *= new Quaternion(0, Mathf.Sin(horizontal), 0, Mathf.Cos (horizontal));
		}
		
		var qx = body.rotation.x;
		var qy = body.rotation.y;
		var qz = body.rotation.z;
		var qw = body.rotation.w;
		body.position = new Vector3(2 * (qy * qw + qz * qx) * radius, 2 * (qz * qy - qw * qx) * radius, ((qz * qz + qw * qw) - (qx * qx + qy * qy)) * radius);
		body.rotation = Quaternion.FromToRotation(Vector3.up, body.position);
	}
}
