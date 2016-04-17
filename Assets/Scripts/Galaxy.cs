using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Galaxy : MonoBehaviour {

	public float multiplier = 1.2f;
	GameObject player;
	LinkedList<GameObject> planets;

	// Use this for initialization
	void Awake () {
		player = Instantiate (Resources.Load ("Player"), Vector3.up * 61, Quaternion.identity) as GameObject;
		planets = new LinkedList<GameObject> ();
		addPlanets (planets, 5);
		float dist = 0;
		int i = 0;
		foreach(GameObject planet in planets) {
			planet.transform.localScale *= 10f * Random.Range(1f, multiplier) * (planets.Count - i++);
			Planet p = planet.GetComponent<Planet> ();
			planet.transform.position = Vector3.right * dist;
			p.Spawn ("Tree", Random.Range(10,150), .5f);
			p.Spawn ("WaterWell", Random.Range (10, 20), .4f);
			p.Spawn ("Stone", Random.Range (10, 20), .4f);
			dist += p.transform.localScale.x * p.core.transform.localScale.x * 2 * multiplier;
		}
		//player.GetComponent<PlayerController> ().planet = planets.First.Value;
		player.GetComponent<PlayerController> ().setPlanet (planets.First.Value.GetComponent<Planet>());
	}

	// Update is called once per frame
	void Update () {
		foreach (GameObject obj in planets) {
			Planet planet = obj.GetComponent<Planet> ();
			planet.orbit ();
		}
	}

	void addPlanets(LinkedList<GameObject> planets, int number) {
		for (int i = 0; i < number; i++) {
			planets.AddLast(Instantiate (Resources.Load ("Planet"), Vector3.zero, Quaternion.identity) as GameObject);
		}
	}
}
