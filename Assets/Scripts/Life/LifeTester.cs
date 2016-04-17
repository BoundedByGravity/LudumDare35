using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeTester : MonoBehaviour {
	
	Planet planet;

	// Use this for initialization
	void Start () {
		planet = GameObject.Find ("Planet").GetComponent<Planet> ();
		planet.Spawn ("Gran", Random.Range(10,150), .5f);
		planet.Spawn ("WaterWell", Random.Range (10, 20), .4f);
	}
}
