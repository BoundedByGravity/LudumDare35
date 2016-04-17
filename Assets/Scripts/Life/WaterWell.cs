using UnityEngine;
using System.Collections;

public class WaterWell : Life {

	// Use this for initialization
	void Start () {
		StartCoroutine (Popup(1f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void interact(GameObject player) {
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if (child.name == "Water")
				StartCoroutine (shrinkObject (child.gameObject, .2f));
		}
	}

	IEnumerator shrinkObject(GameObject obj, float drink) {
		Vector3 original = obj.transform.localScale;
		if (original.y >= drink) {
			obj.transform.localScale = new Vector3 (original.x, original.y - drink, original.z);
			obj.transform.position += Vector3.down * drink * obj.transform.localScale.y;

			if (obj.transform.localScale.y < drink) {
				StartCoroutine (fadeOutAndDestroy (gameObject, 1f));
			}
		}
		yield return null;
	}
}
