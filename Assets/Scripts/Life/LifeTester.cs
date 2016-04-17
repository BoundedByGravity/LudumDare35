using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeTester : MonoBehaviour {
	
	List<Life> lives;

	// Use this for initialization
	void Start () {
		GameObject[] lifeArray = GameObject.FindGameObjectsWithTag ("Lifeform");
		lives = new List<Life> ();
		for (int i = 0; i < lifeArray.Length; i++) {
			lives.Add (lifeArray [i].GetComponent<Life>());
			print ("Lifeform found " + lifeArray[i]);
		}
		foreach(Life life in lives) {
			life.interact (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
