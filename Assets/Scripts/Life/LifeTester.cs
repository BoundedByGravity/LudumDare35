using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeTester : MonoBehaviour {
	
	Planet planet;

	// Use this for initialization
	void Start () {
		planet = GameObject.Find ("Planet").GetComponent<Planet> ();
		planet.Spawn (Resources.Load("Tree"), Random.Range(10,150), .5f);
		planet.Spawn (Resources.Load("WaterWell"), Random.Range (10, 20), .4f);
		planet.Spawn (Resources.Load("Stone"), Random.Range (10, 20), .4f);
	}
}
