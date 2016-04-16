using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Planet : MonoBehaviour {

	public GameObject core;
	float size = 1;
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
		obj = Resources.Load ("Tree") as GameObject;
		//Destroy (obj);
		holder = new GameObject("Generated");
		holder.transform.parent = this.transform;
	}

	public void setSize(float size) {
		transform.localScale *= size;
		radius *= size;
	}

	public void spawnTrees() {
		Vector3 spawnPos = Random.onUnitSphere * radius + transform.position;
		GameObject clone = Instantiate (obj, spawnPos, Quaternion.FromToRotation(Vector3.up, spawnPos - transform.position)) as GameObject;

		Collider collider = clone.GetComponent<Collider> ();
		float offset = 0;
		if (collider.GetType () == typeof(BoxCollider)) {
			offset = ((BoxCollider)collider).size.y / 2f - this.offset;
		} else if (collider.GetType () == typeof(SphereCollider)) {
			offset = ((SphereCollider)collider).radius / 2f - this.offset;
		} else if(collider.GetType() == typeof(CapsuleCollider)) { 
			offset = ((CapsuleCollider)collider).height / 2f - this.offset;
		}
		clone.transform.position += clone.transform.up * offset;
		clone.transform.parent = holder.transform;
	}
}
