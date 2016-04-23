using UnityEngine;
using System.Collections;

public class VectorFieldEffector : MonoBehaviour {

	public Rigidbody particleRef;

	// Use this for initialization
	void Start () {
		for (int n = 0; n < 100; n++) {
			Vector3 position = Quaternion.AngleAxis (Random.Range (0, 360), Vector3.up) * Vector3.forward;
			Rigidbody particleClone = (Rigidbody)Instantiate (particleRef, position, transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {
		//Debug.Log ("Trigger enter");
	}

	void OnTriggerExit() {
		//Debug.Log ("Trigger exit");
	}
}
