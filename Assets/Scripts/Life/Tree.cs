using UnityEngine;
using System.Collections;

public class Tree : Life {

	// Use this for initialization
	void Start () {
		StartCoroutine (Popup(1f));
	}
	
	public override void interact(GameObject player) {
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			child.parent = transform.parent;
			if (child.name == "Tree") {
				Rigidbody body = child.gameObject.AddComponent<Rigidbody> ();
				body.AddExplosionForce (400f, player.transform.position, 5f);
			} 
			StartCoroutine (fadeOutAndDestroy (child.gameObject, 2f));
		}
	}

	IEnumerator fadeOutAndDestroy(GameObject obj, float time) {
		Destroy (obj, time);
		Renderer render = obj.GetComponent<Renderer> ();
		if (render != null) {
			Color fromColor = render.material.color;
			Color toColor = new Color (fromColor.r, fromColor.g, fromColor.b, 0);
			float pastTime = 0;
			while (pastTime < time) {
				pastTime += Time.deltaTime;
				render.material.color = Color.Lerp (fromColor, toColor, pastTime);
				yield return null;
			}
		}
	}
}
