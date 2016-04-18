using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeTester : MonoBehaviour {
	
	PopulatePlanet populatePlanet;

	// Use this for initialization
	void Start () {
		populatePlanet = GameObject.Find ("Planet").GetComponent<PopulatePlanet> ();
		populatePlanet.Spawn (Resources.Load("Tree"), Random.Range(10,150), .5f);
		populatePlanet.Spawn (Resources.Load("WaterWell"), Random.Range (10, 20), .4f);
		populatePlanet.Spawn (Resources.Load("Stone"), Random.Range (10, 20), .4f);
	}
}
