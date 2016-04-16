using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Galaxy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] planets = new GameObject[4];
		for (int i = 0; i < planets.Length; i++) {
			planets [i] = Instantiate (Resources.Load ("Planet"), (i+1) * (i+1) * 2 * Random.Range(100, 120) * Random.onUnitSphere, Quaternion.identity) as GameObject;
			planets [i].transform.localScale *= 2 * (planets.Length - i);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
