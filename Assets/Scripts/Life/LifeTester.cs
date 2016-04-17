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
	}

	float timer = 2f;
	float time = 2f;
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0) {
			foreach(Life life in lives) {
				if (life != null)
					life.interact (gameObject);
			}
			time = timer;
		}
	}
}
