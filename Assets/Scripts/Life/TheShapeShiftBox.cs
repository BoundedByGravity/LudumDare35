using UnityEngine;
using System.Collections;

public class TheShapeShiftBox : Life {

	public Mesh[] meshes;

	// Use this for initialization
	void Start () {
		StartCoroutine (Popup (2f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	Renderer render;
	MeshFilter mf;
	Light l;

	public override void interact(GameObject player) {
		print ("Get ready to disco");
		//player.GetComponent<PlayerController> ().moveSpeed = 0;
		GameObject human = player.transform.FindChild("human").gameObject;
		if (human != null) {
			human.SetActive (false);
		}
		player.GetComponentInChildren<AudioSource> ().Stop ();

		GameObject box = Instantiate (Resources.Load ("TheShapeShiftBox"), player.transform.position, player.transform.rotation) as GameObject;
		box.transform.parent = player.transform;

		render = box.GetComponentInChildren<Renderer> ();
		mf = box.GetComponentInChildren<MeshFilter> ();
		l = box.GetComponentInChildren<Light> ();
		box.GetComponentInChildren<AudioSource> ().Play ();
		StartCoroutine (Disco (.1f));
	}

	IEnumerator Disco(float interval) {
		while (true) {
			yield return new WaitForSeconds (interval);
			render.material.color = Random.ColorHSV ();
			mf.mesh = meshes [Random.Range (0, meshes.Length)];
			l.intensity = Random.Range (0, 3) * 5;
			yield return null;
		}
	}
}
