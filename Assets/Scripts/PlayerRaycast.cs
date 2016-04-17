using UnityEngine;
using System.Collections;

public class PlayerRaycast : MonoBehaviour {

	void Update() {
		const float maxDistance = 5f;

		// Move this into oninput part when done with debugging
		Vector3 origin = gameObject.transform.position;
		Vector3 direction = gameObject.transform.forward;
		Debug.DrawRay (origin, direction*maxDistance, Color.red);

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = new Ray(origin, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, maxDistance)){
				if (hit.transform.tag == "life") {
					print ("Hit lifeform: " + hit.transform.name);
				}
			}
		}
	}
}
