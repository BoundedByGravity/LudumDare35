using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Planet))]
public class Orbit : MonoBehaviour {

	public GameObject bodyToOrbit;
	public float orbitSpeed;

	// Use this for initialization
	void Start () {
		if (bodyToOrbit == null) {
			Debug.LogWarning (this + ": Orbit was set on entity, but no object was set to orbit");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if (bodyToOrbit != null) {
			this.transform.RotateAround (bodyToOrbit.transform.position, Vector3.up, orbitSpeed);
		}
	}
}
