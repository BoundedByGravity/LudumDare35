using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

struct PlanetProperties {
	public float radius;
	public float boundaryCondition;
	public float landRadius;
	public Vector3 position;

	public PlanetProperties(Planet p, float playerHeight) {
		float radius = p.transform.localScale.x;
		this.radius = playerHeight/2 + radius;
		this.boundaryCondition = 0.2f;
		this.landRadius = this.radius + 0.4f;
		this.position = p.transform.position;
	}
}

[RequireComponent(typeof(GravitySink))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour {
	// TODO: Divide move/physics logic and interaction/input logic

	Rigidbody body;

	GravitySink gravitySink;

	GameObject bullet;

	// Initial trajectory vector, will change
	Vector3 trajectory;

	// Constants
	[Range (1,20)] public float moveSpeed = 5f;
	[Range (1,20)] public float jumpSpeed = 10f;
	float sprintFactor = 2f;
	Planet[] planetArray;
	bool stuck = false;

	float deltaMouseX = 0;
	float deltaMouseY = 0;

	float degree_rotation = 1;
	//Vector3 prevAddedMoveVelocity;
	//GravitySink gravitySink;

	float playerHeight;

	public bool canInteract {
		get;
		private set;
	}

	const CursorLockMode cursorLockModeHidden = CursorLockMode.Locked;
	const CursorLockMode cursorLockModeVisible = CursorLockMode.None;

	// Lol
	public Planet planet;
	PlanetProperties planetProperties;

	Animator animator;

	// Use this for initialization
	void Start () {
		// Initialize cursor lock
		Cursor.lockState = cursorLockModeHidden;
		Cursor.visible = false;

		planetArray = Component.FindObjectsOfType<Planet> ();
		body = this.gameObject.GetComponent<Rigidbody> ();

		// Assumes the player is starting in the upward position, above a planet (x, y+k*radius, z)
		trajectory = body.transform.rotation * Vector3.forward;

		BoxCollider collider = body.GetComponent<BoxCollider> ();
		playerHeight = 2*collider.bounds.extents.y;

		//camerat = trajectory;

		if(this.planet != null) {
			setPlanet (this.planet);
		}

		animator = this.GetComponentInChildren<Animator> ();

		// Initializes the camera
		InitializeCameras();
		nextCamera ();
	}

	void InitializeCameras() {
		// This didn't do what I thought it could do (avoid rendering far-away objects)
		// Something that does this is still needed
		Camera[] cameras = this.GetComponentsInChildren<Camera> ();
		float[] distances = new float[32];
		distances[1] = 10;
		distances[2] = 10;
		distances[3] = 10;
		distances[4] = 10;
		distances[5] = 10;
		distances[6] = 10;
		distances[7] = 10;
		distances[8] = 10;
		distances[9] = 10;
		distances[10] = 10;
		distances[11] = 10;
		cameras[0].layerCullDistances = distances;
		cameras[1].layerCullDistances = distances;
		Debug.Log ("Set up cameras");
	}

	public void setPlanet(Planet p) {
		this.planet = p;
		if (p != null) {
			this.planetProperties = new PlanetProperties (p, playerHeight);
		} else {
			Debug.LogError ("No planet set for player since passed planet was null, player control should not work");
		}
	}

	// Update is called once per frame
	void Update () {
		// TODO: Figure out what should go here
		deltaMouseX += Input.GetAxis ("Mouse X");
		deltaMouseY += Input.GetAxis ("Mouse Y");

		bool switchCamera = Input.GetButtonDown("Switch Camera");
		if (switchCamera) {
			nextCamera ();
		}

		interact ();

		bool cancel = Input.GetButtonDown("Cancel");
		if (cancel) {
			if (Cursor.lockState == CursorLockMode.Confined || Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = cursorLockModeVisible;
				// We do not want to exit to the main menu, we want a pause menu with the option to exit to the main menu
				//SceneManager.LoadScene("Main Menu");
			} else {
				Cursor.lockState = cursorLockModeHidden;
			}
			Cursor.visible = !Cursor.visible;
		}
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
		//body.rotation.SetLookRotation(getPlayerUpOnPlanet());
	}

	Vector3 getPlayerUpOnPlanet() {
		return (this.transform.position - planetProperties.position).normalized;
	}

	void printInput() {
		// Method useful in debugging input
		foreach(string input in new string[]{"Fire1", "Fire2", "Fire3", "Sprint", "Jump"}) {
			if (Input.GetButton (input)) {
				Debug.Log (input);
			}
		}
	}

	void nextCamera() {
		Camera[] cameras = this.GetComponentsInChildren<Camera> ();
		int i = 0;
		int nextEnabled = 0;
		foreach(Camera camera in cameras) {
			if (camera.enabled) {
				camera.enabled = false;
				nextEnabled = (i + 1) % cameras.Length;
			}
			i++;
		}
		//Debug.Log ("Switching to camera " + nextEnabled);
		cameras [nextEnabled].enabled = true;
		InitializeCameras();
	}

	Camera activeCamera() {
		Camera[] cameras = this.GetComponentsInChildren<Camera> ();
		foreach (Camera camera in cameras) {
			if (camera.enabled) {
				return camera;
			}
		}
		return cameras[0];
	}

	void FixedUpdate() {


		float horizontalLeftStick = Input.GetAxis ("Horizontal");
		float verticalLeftStick = Input.GetAxis ("Vertical");


		//Debug.Log (horizontalLeftStick + " " + verticalLeftStick);

		bool jump = Input.GetButton("Jump");
		bool sprint = Input.GetButton ("Sprint");

		float forwardCharacterInput = verticalLeftStick;
		float sidewaysCharacterInput = horizontalLeftStick;
		float rotateCharacterInput = deltaMouseX; deltaMouseX = 0;
		float pitchCharacterInput = deltaMouseY; deltaMouseY = 0;

		movePlayer (forwardCharacterInput, sidewaysCharacterInput, rotateCharacterInput, jump, sprint);
		if (pitchCharacterInput != 0) {
			alignFirstPersonCamera (pitchCharacterInput);
		}

		closestPlanet ();
	}

	void closestPlanet() {
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

	// Interaction logic
	void interact() {
		const float maxDistance = 5f;

		Camera camera = activeCamera ();

		// Move this into oninput part when done with debugging
		Vector3 origin = camera.transform.position - transform.up * transform.localScale.y * .1f;
		Vector3 direction = camera.transform.forward;
		Debug.DrawRay (origin, direction*maxDistance, Color.red);

		Ray ray = new Ray(origin, direction);
		RaycastHit hit;
		int lifeLayer = 1 << LayerMask.NameToLayer ("Life");
		if (Physics.Raycast (ray, out hit, maxDistance, lifeLayer)) {
			Life life = hit.collider.transform.GetComponent<Life> ();
			if (life != null) {
				canInteract = true;
				if (Input.GetButton ("Interact")) {
					life.interact (gameObject);
				}
			}
		} else {
			canInteract = false;
		}
	}

	void alignFirstPersonCamera(float pitchCharacterInput) {
		// TODO: This wont be the first person camera always
		Camera[] cameras = this.GetComponentsInChildren<Camera> ();
		//Debug.Log ("The first chosen camera is: " + cameras[0].name);
		Camera firstPersonCam = cameras [0];
		firstPersonCam.transform.Rotate(new Vector3(-pitchCharacterInput, 0, 0));
		float currentPitch = firstPersonCam.transform.localRotation.eulerAngles.x;

		// Clamp camera angle
		const float upperLimit = 300;
		const float lowerLimit = 80;
		currentPitch = currentPitch < 180 ? Mathf.Min(currentPitch, lowerLimit) : Mathf.Max(currentPitch, upperLimit);
		firstPersonCam.transform.localRotation = Quaternion.Euler (currentPitch, 0, 0);
	}

	void movePlayer(float forwardCharacterInput, float sidewaysCharacterInput, float rotateCharacterInput, bool jumpInput, bool sprintInput) {
		/* Moves the player according to input arguments (which are all actual controller input)
		 *  As it turns out, making a good model of input is hard, and the following requirements must be met:
		 *  - Should be able to walk around a planet (given simulated gravity)
		 *  - Should not run faster diagonally than forward/sideways
		 *  - Should be able to sprint, but only in a forward direction.
		 */
		bool isBound = isPlanetbound ();
		Vector3 up = getPlayerUpOnPlanet ();

		//Debug.Log ("isBound: " + isBound + ", dist: " + Vector3.Distance(body.position, Vector3.zero));
		trajectory = (trajectory - Vector3.Project (trajectory, up)).normalized;

		// This obviously doesn't do what we think it does provided how the character can be placed at an angle with the ground after collisions
		body.rotation = Quaternion.LookRotation (trajectory, up);

		if (rotateCharacterInput != 0) {
			Vector3 trajectory2 = -rotateCharacterInput * Vector3.Cross (trajectory, up).normalized;
			trajectory = (trajectory * Mathf.Cos (degree_rotation * Mathf.PI / 180) + trajectory2 * Mathf.Sin (degree_rotation * Mathf.PI / 180)).normalized;
		}

		GravitySink gravitySink = this.GetComponent<GravitySink> ();

		if (isBound || stuck) {
			// Måns is love, Måns is life
			// We should only do this when grounded, otherwise jumping will be weird
			// Also, we should inherit the planet velocity when grounded and jumping

			// Don't apply gravity when grounded
			gravitySink.setAcceptsForce (false);

			// Calculate how much the planet has moved since last cycle, and move the player along with it.
			Vector3 dv = planet.transform.position - planetProperties.position;
			planetProperties = new PlanetProperties (planet, playerHeight);
			this.transform.position += dv;

			// Moves the character forward/backwards
			Vector3 forwardOrBackwardVector = forwardCharacterInput * trajectory;
			Vector3 tradjectoryside = Vector3.Cross (-trajectory, up).normalized;
			Vector3 sidewaysVector = sidewaysCharacterInput * tradjectoryside;

			float factor = Mathf.Max (Mathf.Abs (forwardCharacterInput), Mathf.Abs (sidewaysCharacterInput));

			// TODO: Do something smoother than 0.7, such as a linear scale depending on how much weight lies forward
			if (sprintInput && forwardCharacterInput > Mathf.Abs (sidewaysCharacterInput)) { //To much behviour in comparasion ;)
				body.velocity = factor * sprintFactor * moveSpeed * (forwardOrBackwardVector + sidewaysVector).normalized;
			} else {
				body.velocity = factor * moveSpeed * (forwardOrBackwardVector + sidewaysVector).normalized;
			}

			if (forwardCharacterInput > 0) {
				animator.Play("Walking");
			}

			if (isBound) {
				land ();
			}
			if (jumpInput) {
				jump ();
			}
		} else {
			gravitySink.setAcceptsForce (true);
		}
	}

	void jump() {
		Vector3 up = getPlayerUpOnPlanet ();
		body.position += up * 2 * planetProperties.boundaryCondition;
		body.velocity += up * jumpSpeed;
	}
}
