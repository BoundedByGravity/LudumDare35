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
	
	// Use this for initialization
	void Start () {
		holder = new GameObject ("Generated");
		holder.transform.parent = this.transform;
		holder.transform.localScale = Vector3.one;
		holder.transform.localPosition = Vector3.zero;

		mesh = core.GetComponent<MeshFilter> ().mesh;
	}
}
