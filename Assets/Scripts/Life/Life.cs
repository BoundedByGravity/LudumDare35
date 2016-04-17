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
}
