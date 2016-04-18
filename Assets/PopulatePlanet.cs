using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Planet))]
public class PopulatePlanet : MonoBehaviour {

	Planet planet;
	Mesh mesh;


	public GameObject spawnObject;
	public int spawnNumber = 10;

	// Use this for initialization
	void Start () {
		planet = this.GetComponent<Planet> ();
		mesh = planet.core.GetComponent<MeshFilter> ().mesh;

		if (spawnObject != null) {
			Spawn (spawnObject, spawnNumber, 0f);
		}

	}


	public void Spawn(Object obj, int number, float speed) {
		StartCoroutine (Spawner (obj as GameObject, number, speed));
	}

	IEnumerator Spawner(Object obj, int number, float speed) {
		while(number > 0) {
			yield return new WaitForSeconds(speed);
			Vector3 spawnPos = mesh.vertices [Random.Range(0, mesh.vertices.Length)] 
								* planet.core.transform.localScale.x 
								* transform.localScale.x + planet.core.transform.position;
			GameObject clone = Instantiate (obj, spawnPos, Quaternion.FromToRotation(Vector3.up, spawnPos - transform.position)) as GameObject;
			clone.transform.parent = planet.holder.transform;
			number--;
			yield return null;
		}
	}
}
