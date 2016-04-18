using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Planet))]
public class Orbit : MonoBehaviour {

	public Planet planet;
	public float orbitSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		this.transform.RotateAround (planet.transform.position, Vector3.up, orbitSpeed);
	}
}
