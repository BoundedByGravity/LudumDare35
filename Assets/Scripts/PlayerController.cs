using UnityEngine;
using System.Collections;

struct PlanetProperties {
	public float radius;
	public float boundaryCondition;
	public float landRadius;

	public PlanetProperties(float radius) {
		this.radius = 1+radius;
		this.boundaryCondition = this.radius * 0.005f;
		this.landRadius = this.radius * 1.005f;
	}
}

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody body;

	GravitySink gravitySink;

	GameObject bullet;

	// Initial trajectory vector, will change
	Vector3 trajectory;
	Vector3 camerat;

	// Constants
	[Range (1,20)] public float moveSpeed;
	[Range (1,20)] public float jumpSpeed;
	float sprintFactor = 2f;
	float jumpspeed;

	float deltaMouseX = 0;
	float deltaMouseY = 0;

	float degree_rotation = 3;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	const CursorLockMode cursorLockModeHidden = CursorLockMode.Confined;
	const CursorLockMode cursorLockModeVisible = CursorLockMode.None;

	// Lol
	public GameObject planet;
	PlanetProperties planetProperties;


	// Use this for initialization
	void Start () {
		trajectory = new Vector3 (0, 0, 1);
		moveSpeed = 5f;
		jumpSpeed = 3f;
		body = this.gameObject.GetComponent<Rigidbody> ();
		camerat = trajectory;
		Cursor.lockState = cursorLockModeHidden;
		Cursor.visible = false;

		if (planet != null) {
			planetProperties = new PlanetProperties (planet.transform.localScale.x);
		} else {
			Debug.LogWarning ("No planet set for player, using default scale of 26");
			planetProperties = new PlanetProperties (26);
		}

		Debug.Log (planetProperties.radius);
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
		return dist < planetProperties.landRadius + planetProperties.boundaryCondition;
	}

	void land() {
		body.position = body.position.normalized * planetProperties.landRadius;
	}

	Vector3 getPlayerUpOnPlanet() {
		return (this.transform.position - planet.transform.position).normalized;
	}

	void FixedUpdate() {
		bool cancel = Input.GetButtonDown("Cancel");
		if (cancel) {
			if (Cursor.lockState == CursorLockMode.Confined || Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = cursorLockModeVisible;
			} else {
				Cursor.lockState = cursorLockModeHidden;
			}
			Cursor.visible = !Cursor.visible;
		}
		//Camera camera = this.GetComponentInChildren<Camera>();
		//camera.transform.Rotate (new Vector3 (-dmousey, dmousex, 0));

		// Useful for checking which button is which
		/*
		foreach(string input in new string[]{"Fire1", "Fire2", "Fire3", "Sprint", "Jump"}) {
			if (Input.GetButton (input)) {
				Debug.Log (input);
			}
		}
		*/

		float horizontalLeftStick = Input.GetAxis ("Horizontal");
		float verticalLeftStick = Input.GetAxis ("Vertical");

		//Debug.Log (horizontalLeftStick + " " + verticalLeftStick);

		bool jump = Input.GetButtonDown("Jump");
		bool sprint = Input.GetButton ("Sprint");

		float forwardCharacterInput = verticalLeftStick;
		float sidewaysCharacterInput = horizontalLeftStick;
		float rotateCharacterInput = deltaMouseX; deltaMouseX = 0;

		movePlayer (forwardCharacterInput, sidewaysCharacterInput, rotateCharacterInput, jump, sprint);
	}

	void movePlayer(float forwardCharacterInput, float sidewaysCharacterInput, float rotateCharacterInput, bool jump, bool sprint) {
		/* Moves the player according to input arguments (which are all actual controller input)
		 *  As it turns out, making a good model of input is hard, and the following requirements must be met:
		 *  - Should be able to walk around a planet (given simulated gravity)
		 *  - Should not run faster diagonally than forward/sideways
		 *  - Should be able to sprint
		 * All are fulfilled.
		 */
		bool isBound = isPlanetbound ();
		Vector3 up = getPlayerUpOnPlanet ();

		//Debug.Log ("isBound: " + isBound + ", dist: " + Vector3.Distance(body.position, Vector3.zero));
		trajectory = (trajectory - Vector3.Project (trajectory, up)).normalized;
		camerat = (camerat - Vector3.Project (camerat, up)).normalized;
		body.rotation = Quaternion.LookRotation (camerat, up);

		if (isBound) {
			// Måns is love, Måns is life

			trajectory = camerat;
			if (rotateCharacterInput != 0) {
				Vector3 trajectory2 = -rotateCharacterInput * Vector3.Cross (trajectory, up).normalized;
				trajectory = trajectory * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180);
			}
			camerat = trajectory;

			// Moves the character forward/backwards
			Vector3 forwardOrBackwardVector = forwardCharacterInput * trajectory;
			Vector3 tradjectoryside = Vector3.Cross (-trajectory, up).normalized;
			Vector3 sidewaysVector = sidewaysCharacterInput * tradjectoryside;

			// TODO: Do something smoother than 0.7, such as a linear scale depending on how much weight lies forward
			if (sprint && forwardCharacterInput > 0.7f) {
				body.velocity = sprintFactor * moveSpeed * (forwardOrBackwardVector + sidewaysVector).normalized;
			} else {
				body.velocity = moveSpeed * (forwardOrBackwardVector + sidewaysVector).normalized;
			}

			land ();
			if (jump) {
				body.position += up * 2 * planetProperties.boundaryCondition;
				body.velocity += up * jumpSpeed;
			}
		} else {
			if (rotateCharacterInput != 0) {
				Vector3 trajectory2 = -rotateCharacterInput * Vector3.Cross (camerat, up).normalized;
				camerat = camerat * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180);
			}
		}
	}
}
