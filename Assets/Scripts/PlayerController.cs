using UnityEngine;
using System.Collections;

struct PlanetProperties {
	public float radius;
	public float boundaryCondition;
	public float landRadius;
	public Vector3 position;

	public PlanetProperties(Planet p, float playerheight) {
		float radius = p.transform.localScale.x;
		this.radius = playerheight/2+radius;
		this.boundaryCondition = 0.01f;
		this.landRadius = this.radius + 0.2f;
		position = p.transform.position;
	}
}

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
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
	Planet[] planetArray;
	bool stuck = false;

	float deltaMouseX = 0;
	float deltaMouseY = 0;

	float degree_rotation = 1;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	float playerHeight;

	const CursorLockMode cursorLockModeHidden = CursorLockMode.Confined;
	const CursorLockMode cursorLockModeVisible = CursorLockMode.None;

	// Lol
	public GameObject planet;
	PlanetProperties planetProperties;

	Camera firstPersonCam;
	Camera thirdPersonCam;

	// Use this for initialization
	void Start () {
		// Initialize cursor lock
		Cursor.lockState = cursorLockModeHidden;
		Cursor.visible = false;

		Camera[] cams = this.GetComponentsInChildren<Camera> ();
		firstPersonCam = cams [0];
		thirdPersonCam = cams [1];

		planetArray = Component.FindObjectsOfType<Planet> ();
		body = this.gameObject.GetComponent<Rigidbody> ();
		trajectory = new Vector3 (0, 0, 1);
		moveSpeed = 5f;
		jumpSpeed = 10f;

		BoxCollider collider = body.GetComponent<BoxCollider> ();
		playerHeight = 2*collider.bounds.extents.y;

		camerat = trajectory;

		setPlanet (planet.GetComponent<Planet>());
	}

	public void setPlanet(Planet p) {
		this.planet = p.gameObject;
		if (p != null) {
			this.planetProperties = new PlanetProperties (p, playerHeight);
		} else {
			Debug.LogError ("No planet set for player, using default scale of 26");
			this.planetProperties = new PlanetProperties (null, playerHeight);
		}
	}

	// Update is called once per frame
	void Update () {
		// TODO: Figure out what should go here
		deltaMouseX += Input.GetAxis ("Mouse X");
		deltaMouseY += Input.GetAxis ("Mouse Y");
	}

	void OnCollisionEnter(Collision collision) {
		stuck = true;
	}
	void OnCollisionExit(Collision collision) {
		stuck = false;
	}

	bool isPlanetbound() {
		// TODO: Use radius from closest planet or use a trigger to detect landing on a planet
		float dist = Vector3.Distance (body.position, planetProperties.position);
		return dist < planetProperties.landRadius + planetProperties.boundaryCondition;
	}
	float dist(Vector3 v) {
		return Vector3.Distance (body.position, v);
	}

	void land() {
		body.position = (body.position-planetProperties.position).normalized * planetProperties.landRadius + planetProperties.position;
	}

	Vector3 getPlayerUpOnPlanet() {
		return (this.transform.position - planetProperties.position).normalized;
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

		bool switchCamera = Input.GetButtonDown("Switch Camera");
		if (switchCamera) {
			firstPersonCam.enabled = !firstPersonCam.enabled;
			thirdPersonCam.enabled = !thirdPersonCam.enabled;
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
		float pitchCharacterInput = deltaMouseY; deltaMouseY = 0;

		movePlayer (forwardCharacterInput, sidewaysCharacterInput, rotateCharacterInput, jump, sprint);
		//alignFirstPersonCamera (pitchCharacterInput);


		float mindist = dist (planetProperties.position);
		bool changed = false;
		Planet swap = null;
		foreach (Planet p in planetArray) {
			float tdist = dist (p.transform.position);
			if (tdist < mindist) {
				mindist = tdist;
				swap = p;
				changed = true;
			}
		}
		if (changed) {
			setPlanet (swap);
		}

	}

	void alignFirstPersonCamera(float pitchCharacterInput) {
		// TODO: This wont be the first person camera always
		firstPersonCam.transform.Rotate(new Vector3(-pitchCharacterInput, 0, 0));
		float currentPitch = firstPersonCam.transform.localRotation.eulerAngles.x;

		// Clamp camera angle
		const float upperLimit = 300;
		const float lowerLimit = 80;
		currentPitch = currentPitch < 180 ? Mathf.Min(currentPitch, lowerLimit) : Mathf.Max(currentPitch, upperLimit);
		firstPersonCam.transform.localRotation = Quaternion.Euler (currentPitch, 0, 0);
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
		if (rotateCharacterInput != 0) {
			Vector3 trajectory2 = -rotateCharacterInput * Vector3.Cross (camerat, up).normalized;
			camerat = (camerat * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180)).normalized;
		}

		if (isBound || stuck) {
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

			float factor = Mathf.Max (Mathf.Abs (forwardCharacterInput), Mathf.Abs (sidewaysCharacterInput));

			// TODO: Do something smoother than 0.7, such as a linear scale depending on how much weight lies forward
			if (sprint && forwardCharacterInput > Mathf.Abs (sidewaysCharacterInput)) { //To much behviour in comparasion ;)
				body.velocity = factor * sprintFactor * moveSpeed * (forwardOrBackwardVector + sidewaysVector).normalized;
			} else {
				body.velocity = factor * moveSpeed * (forwardOrBackwardVector + sidewaysVector).normalized;
			}
			if (isBound) {
				land ();
			}
			if (jump) {
				body.position += up * 2 * planetProperties.boundaryCondition;
				body.velocity += up * jumpSpeed;
			}
		}


	}
}
