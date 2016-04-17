using UnityEngine;
using System.Collections;

public class Tree : Life {

	// Use this for initialization
	void Start () {
		StartCoroutine (Popup(1f));
	}
	
	public override void interact(GameObject player) {
		StartCoroutine (fadeOutAndDestroy (gameObject, 2f));

		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if (child.name == "Tree") {
				Rigidbody body = child.gameObject.AddComponent<Rigidbody> ();
				body.AddExplosionForce (400f, player.transform.position, 5f);
			}
		}
	}
}
