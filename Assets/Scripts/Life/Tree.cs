using UnityEngine;
using System.Collections;

public class Tree : Life {

	// Use this for initialization
	void Start () {
		StartCoroutine (Popup(1f));
	}
	
	public override void interact() {
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			child.parent = null;
		}
	}
}
