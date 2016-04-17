using System;
using UnityEngine;

public class Spawn : Life {

	void Start() {
		StartCoroutine (Popup(2f));
	}

	public override void interact(GameObject player) {
		// Nothing happens
	}
}