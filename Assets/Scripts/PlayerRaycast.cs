using UnityEngine;
using System.Collections;

public class PlayerRaycast : MonoBehaviour {

	void Update() {
		const float maxDistance = 5f;

		// Move this into oninput part when done with debugging
		Vector3 origin = transform.position - transform.up * transform.localScale.y * .5f;
		Vector3 direction = transform.forward;
		Debug.DrawRay (origin, direction*maxDistance, Color.red);

		if (Input.GetKeyDown(KeyCode.E)) {
			Ray ray = new Ray(origin, direction);
			RaycastHit hit;
			int lifeLayer = 1 << LayerMask.NameToLayer ("Life");
			if (Physics.Raycast(ray, out hit, maxDistance, lifeLayer)){
				Life life = hit.collider.transform.GetComponent<Life> ();
				if (life != null) {
					life.interact (gameObject);
				}
			}
		}
	}
}
