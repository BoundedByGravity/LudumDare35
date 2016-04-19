using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Galaxy : MonoBehaviour {

	public float multiplier = 1.2f;
	GameObject player;
	LinkedList<GameObject> planets;

	// Use this for initialization
	void Start () {
		planets = new LinkedList<GameObject> ();
		addPlanets (planets, 5);

		Vector3 position = Vector3.up * 200;
		Quaternion rotation = Quaternion.identity;
		rotation.SetLookRotation (new Vector3 (1, 0, 0));
		player = Instantiate (Resources.Load ("Player"), position, rotation) as GameObject;
		float dist = 0;
		int i = 0;

		foreach(GameObject planet in planets) {
			planet.transform.localScale *= (planets.Count - i);
			planet.GetComponent<Rigidbody> ().mass = planet.transform.localScale.x * 20f * (planets.Count - i);
			Planet p = planet.GetComponent<Planet> ();
			planet.transform.position = Vector3.right * dist;

			if(planet != planets.First.Value) {
				Orbit o = p.GetComponent<Orbit> ();
				o.bodyToOrbit = planets.First.Value;
			}

			PopulatePlanet pp = p.GetComponent<PopulatePlanet> ();
			if (pp != null) {
				pp.Spawn (Resources.Load ("Tree"), Random.Range (100, 300), .5f);
				pp.Spawn (Resources.Load ("WaterWell"), Random.Range (30, 80), .4f);
				pp.Spawn (Resources.Load ("Stone"), Random.Range (100, 200), .4f);
				//pp.Spawn (Resources.Load ("Baker_house"), Random.Range (10, 20), .4f);
			}

			dist += 100 + p.transform.localScale.x * 2 * multiplier;
			i++;
		}

		planets.Last.Value.GetComponent<PopulatePlanet> ().Spawn (Resources.Load ("TheShapeShiftBox"), 1, 1f);
		//player.GetComponent<PlayerController> ().planet = planets.First.Value;
		player.GetComponent<PlayerController> ().setPlanet (planets.First.Value.GetComponent<Planet>());
	}

	void addPlanets(LinkedList<GameObject> planets, int number) {
		for (int i = 0; i < number; i++) {
			planets.AddLast(Instantiate (Resources.Load ("Planet"), Vector3.zero, Quaternion.identity) as GameObject);
		}
		planets.First.Value.name += "(Center)";
	}
}
