using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody body;

	GravitySink gravitySink;

	GameObject bullet;

	// Initial trajectory vector, will change
	Vector3 trajectory = new Vector3 (0, 0, 1);

	// Constants
	float radius = 26;	// TODO: Load from radius of parent
	[Range (1,20)] public float moveSpeed;
	[Range (1,20)] public float jumpSpeed;
	float jumpspeed;
	float boundcnd;
	float landradius;

	float deltaMouseX = 0;
	float deltaMouseY = 0;

	float degree_rotation = 3;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	const CursorLockMode cursorLockModeHidden = CursorLockMode.Confined;
	const CursorLockMode cursorLockModeVisible = CursorLockMode.None;

	// Use this for initialization
	void Start () {
		moveSpeed = 7f;
		jumpSpeed = 3f;
		boundcnd = radius/100f;
		landradius = radius * 1.005f;
		body = this.gameObject.GetComponent<Rigidbody> ();
		Cursor.lockState = cursorLockModeHidden;
		Cursor.visible = false;
		//jumping = false;
		//prevAddedMoveVelocity = new Vector3 (0, 0, 0);
	}

	// Update is called once per frame
	void Update () {
		// TODO: Figure out what should go here
		deltaMouseX += Input.GetAxis ("Mouse X");
		deltaMouseY += Input.GetAxis ("Mouse Y");
	}

	bool isPlanetbound() {
		// TODO: Use radius from closest planet or use a trigger to detect landing on a planet
		float dist = Vector3.Distance (body.position, Vector3.zero);
		return dist < landradius + boundcnd;
	}

	void land() {
		body.position = body.position.normalized * landradius;
	}

	Vector3 getPlayerUpOnPlanet(Vector3 planetPos) {
		return (this.transform.position - planetPos).normalized;
	}

	void FixedUpdate() {
		float horizontalLeftStick = Input.GetAxis ("Horizontal");
		float verticalLeftStick = Input.GetAxis ("Vertical");
		bool jump = Input.GetButtonDown("Jump");

		bool cancel = Input.GetButtonDown("Cancel");
		if (cancel) {
			if (Cursor.lockState == CursorLockMode.Confined || Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = cursorLockModeVisible;
			} else {
				Cursor.lockState = cursorLockModeHidden;
			}
			Cursor.visible = !Cursor.visible;
		}
		Camera camera = this.GetComponentInChildren<Camera>();
		//camera.transform.Rotate (new Vector3 (-dmousey, dmousex, 0));

		if (Input.GetButtonDown ("Fire1")) {
			//Instantiate (bullet, transform.position, transform.rotation);
		}

		float forwardCharacterInput = verticalLeftStick;
		float sidewaysCharacterInput = horizontalLeftStick;
		float rotateCharacterInput = deltaMouseX; deltaMouseX = 0;

		movePlayer (forwardCharacterInput, sidewaysCharacterInput, rotateCharacterInput, jump);
	}

	void movePlayer(float forwardCharacterInput, float sidewaysCharacterInput, float rotateCharacterInput, bool jump) {
		bool isBound = isPlanetbound ();
		Vector3 up = getPlayerUpOnPlanet (Vector3.zero);

		//Debug.Log ("isBound: " + isBound + ", dist: " + Vector3.Distance(body.position, Vector3.zero));
		trajectory = (trajectory - Vector3.Project (trajectory, up)).normalized;
		body.rotation = Quaternion.LookRotation (trajectory, up);

		if (isBound) {
			// TODO: Better solution wanted, but better solution hard

			// Måns is love, Måns is life
			if (rotateCharacterInput != 0) {
				Vector3 trajectory2 = -rotateCharacterInput * Vector3.Cross (trajectory, up).normalized;
				trajectory = trajectory * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180);
			}

			// Moves the character forward/backwards
			Vector3 forwardSpeed = (moveSpeed * forwardCharacterInput) * trajectory;

			// Moves the character sideways
			//Vector3 sidewaySpeed = ?

			body.velocity = forwardSpeed; // + sidewaySpeed;

			land ();
			if (jump) {
				body.position += up * 2 * boundcnd;
				body.velocity += up * jumpSpeed;
			}

		}
	}
}
