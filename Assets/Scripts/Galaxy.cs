using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Galaxy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] planets = new GameObject[12];
		planets[0] = Instantiate (Resources.Load ("Planet"), Vector3.zero, Quaternion.identity) as GameObject;
		for (int i = 1; i < planets.Length; i++) {
			planets [i] = Instantiate (Resources.Load ("Planet"), 2 * (planets.Length - i) * Random.Range(100, 120) * Random.onUnitSphere, Quaternion.identity) as GameObject;
			planets [i].transform.localScale *= 2 * (planets.Length - i);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
