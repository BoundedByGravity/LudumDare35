using UnityEngine;
using System.Collections;

public class HideDistantDetails : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera camera = GetComponent<Camera>();
		float[] distances = new float[32];
		float detailDist = 10;
		distances[1] = detailDist;
		distances[2] = detailDist;
		distances[3] = detailDist;
		distances[4] = detailDist;
		distances[5] = detailDist;
		distances[6] = detailDist;
		distances[7] = detailDist;
		distances[8] = detailDist;
		distances[9] = detailDist;
		distances[10] = detailDist;
		distances[11] = detailDist;
		camera.layerCullDistances = distances;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
