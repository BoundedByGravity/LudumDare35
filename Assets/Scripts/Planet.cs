using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Planet : MonoBehaviour {
	// FIXME: Clean up, remove behavior overlapping with PopulatePlanet

	public GameObject core;

	public GameObject holder {
		get;
		private set;
	}

	Mesh mesh;
	float damping;

	public GameObject spawnObject;
	public int spawnNumber = 10;
	
	// Use this for initialization
	void Start () {
		damping = Random.Range (200f, 500f);
		Collider[] colliders = core.GetComponents<Collider> ();
		Collider collider = null;
		foreach (SphereCollider col in colliders) {
			if (!col.isTrigger)
				collider = col;
		}
		holder = new GameObject ("Generated");
		holder.transform.parent = this.transform;
		holder.transform.localScale = Vector3.one;
		holder.transform.localPosition = Vector3.zero;

		mesh = core.GetComponent<MeshFilter> ().mesh;

		if (spawnObject != null) {
			Spawn (spawnObject, spawnNumber, 0f);
		}
	}

	[System.Obsolete("Use PopulatePlanet instead")]
	public void Spawn(UnityEngine.Object obj, int number, float speed) {
		StartCoroutine (Spawner (obj as GameObject, number, speed));
	}

	[System.Obsolete("Use PopulatePlanet instead")]
	IEnumerator Spawner(Object obj, int number, float speed) {
		while(number > 0) {
			yield return new WaitForSeconds(speed);
			Vector3 spawnPos = mesh.vertices [Random.Range(0, mesh.vertices.Length)] * core.transform.localScale.x * transform.localScale.x + core.transform.position;
			GameObject clone = Instantiate (obj, spawnPos, Quaternion.FromToRotation(Vector3.up, spawnPos - transform.position)) as GameObject;
			clone.transform.parent = holder.transform;
			number--;
			yield return null;
		}
	}

	[System.Obsolete("Use Orbit instead")]
	public void orbit() {
		float modifier = Vector3.Distance(Vector3.zero, transform.position) / (damping * core.transform.localScale.x + transform.localScale.x) + 1;
		transform.RotateAround (Vector3.zero, Vector3.up, Time.deltaTime * modifier);
	}
}
