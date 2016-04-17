using UnityEngine;
using System.Collections;

public abstract class Life : MonoBehaviour {

	public abstract void interact (GameObject player);

	protected IEnumerator Popup(float time) {
		Vector3 originalScale = transform.localScale;
		transform.localScale = Vector3.zero;
		float pastTime = 0;
		while (pastTime < time) {
			pastTime += Time.deltaTime;
			transform.localScale = Vector3.Lerp (transform.localScale, originalScale, pastTime);
			yield return null;
		}
	}

	protected IEnumerator fadeOutAndDestroy(GameObject obj, float time) {
		Destroy (obj, time);
		//Fade childs if possible
		Renderer[] renders = obj.GetComponentsInChildren<Renderer> ();
		Color[] fromColors = new Color[renders.Length];
		Color[] toColors = new Color[renders.Length];
		for (int i = 0; i < renders.Length; i++) {
			fromColors [i] = renders [i].material.color;
			toColors [i] = new Color (fromColors[i].r, fromColors[i].g, fromColors[i].b, 0);
		}
		float pastTime = 0;
		while (pastTime < time) {
			pastTime += Time.deltaTime;
			for(int i = 0; i < renders.Length; i++)
				renders[i].material.color = Color.Lerp (fromColors[i], toColors[i], pastTime);
			yield return null;
		}
	}
}
