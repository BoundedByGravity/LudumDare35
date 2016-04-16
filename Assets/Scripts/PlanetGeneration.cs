using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class PlanetGeneration : MonoBehaviour {

	public GameObject core;
	float radius, offset;
	GameObject obj, holder;
	
	// Use this for initialization
	void Start () {
		SphereCollider[] colliders = core.GetComponents<SphereCollider> ();
		SphereCollider collider = null;
		foreach(SphereCollider col in colliders) {
			if (!col.isTrigger)
				collider = col;
		}
		radius = collider.radius * core.transform.localScale.x;
		offset = 0.3f;
		obj = Resources.Load ("SpawnThis") as GameObject;
		//Destroy (obj);
		holder = new GameObject("Generated");
		holder.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		spawn (holder.transform);
	}

	public void spawn(Transform parent) {
		Vector3 spawnPos = Random.onUnitSphere * radius;
		obj = Instantiate (obj, spawnPos, Quaternion.FromToRotation(Vector3.up, spawnPos - transform.position)) as GameObject;

		Collider collider = obj.GetComponent<Collider> ();
		float offset = 0;
		if (collider.GetType () == typeof(BoxCollider)) {
			offset = ((BoxCollider)collider).size.y / 2f - this.offset;
		} else if (collider.GetType () == typeof(SphereCollider)) {
			offset = ((SphereCollider)collider).radius / 2f - this.offset;
		} else if(collider.GetType() == typeof(CapsuleCollider)) { 
			offset = ((CapsuleCollider)collider).height / 2f - this.offset;
		}
		obj.transform.position += obj.transform.up * offset;
		obj.transform.parent = parent;
	}
}
