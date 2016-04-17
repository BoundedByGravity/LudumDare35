using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Galaxy : MonoBehaviour {

	public float multiplier = 1.2f;
	List<GameObject> planets;

	// Use this for initialization
	void Start () {
		planets = new List<GameObject> ();
		addPlanets (planets, 5);
		float dist = 0;
		int i = 0;
		foreach(GameObject planet in planets) {
			planet.transform.localScale *= Random.Range(1f, multiplier) * (planets.Count - i++);
			Planet p = planet.GetComponent<Planet> ();
			planet.transform.position = Vector3.right * dist;
			p.SpawnTrees (150, .5f);
			dist += p.transform.localScale.x * p.core.transform.localScale.x * 2 * multiplier;
		}
	}

	// Update is called once per frame
	void Update () {
		foreach (GameObject obj in planets) {
			Planet planet = obj.GetComponent<Planet> ();
			planet.orbit ();
		}
	}

	void addPlanets(List<GameObject> planets, int number) {
		for (int i = 0; i < number; i++) {
			planets.Add(Instantiate (Resources.Load ("Planet"), Vector3.zero, Quaternion.identity) as GameObject);
		}
	}
}
