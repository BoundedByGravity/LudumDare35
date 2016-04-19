using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	const string menuSceneName = "Main Menu";
	const string gameSceneName = "Galaxy";

	Text[] texts;

	bool paused = true;

	// Use this for initialization
	void Start () {
		texts = GetComponentsInChildren<Text> ();
	}

	void pause() {
		paused = true;
		texts[1].text = "Paused";
		SceneManager.LoadScene(menuSceneName);
	}

	void unpause() {
		paused = false;
		SceneManager.LoadScene (gameSceneName);
	}
		
	public void StartGame() {
		Debug.Log ("Game starting, loading scene");
		SceneManager.LoadScene(gameSceneName);
	}

	public void Quit() {
		Debug.Log ("Application closing");
		Application.Quit ();
	}

	void OnGUI() {
		
	}

	public void resume(Event e) {
		Debug.Log ("Event: " + e);
	}

	// Update is called once per frame
	void Update () {

	}
}
