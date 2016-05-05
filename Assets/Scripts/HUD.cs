using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	GameObject interact_panel;


	// Use this for initialization
	void Start () {
		interact_panel = GameObject.Find("Interact Popup");
		Debug.Log (interact_panel);
	}
	
	// Update is called once per frame
	void Update () {
		PlayerController player = Object.FindObjectOfType<PlayerController>();
		if (player) {
			if (player.canInteract) {
				showInteractPanel ();
			} else {
				hideInteractPanel ();
			}
		}
	}

	void showInteractPanel() {
		CanvasGroup cg = interact_panel.GetComponent<CanvasGroup> ();
		cg.alpha = 1;
	}

	void hideInteractPanel() {
		CanvasGroup cg = interact_panel.GetComponent<CanvasGroup> ();
		cg.alpha = 0;
	}
}
