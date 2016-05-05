using UnityEngine;
using System.Collections;

public class Tree : Life {

	// Use this for initialization
	void Start () {
		StartCoroutine (Popup(1f));
		//Debug.Log (gameObject.layer);
	}
	
	public override void interact(GameObject player) {
		StartCoroutine (Popdown (player, 2f));
	}
}
