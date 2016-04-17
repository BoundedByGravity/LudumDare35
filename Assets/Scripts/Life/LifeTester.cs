using UnityEngine;
using System.Collections;

public class LifeTester : MonoBehaviour {
	
	Life life;

	// Use this for initialization
	void Start () {
		life = GameObject.FindGameObjectWithTag ("Lifeform").GetComponent<Life>();
		print (life);
		life.interact ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
