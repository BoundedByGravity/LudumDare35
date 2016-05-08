using UnityEngine;
using System.Collections;

public static class VectorFields {

	const float upSpeed_default = 0.1f;

	public static Vector3 CircleField(Vector3 pos) {
		return (-pos.x * Vector3.forward + pos.z * Vector3.right);
	}

	public static Vector3 CylinderField(Vector3 pos, float upSpeed=upSpeed_default) {
		return CircleField(pos) + upSpeed*Vector3.up;
	}

	public static Vector3 ConeField(Vector3 pos, float rotation=10f, float upSpeed=upSpeed_default) {
		return Quaternion.AngleAxis(rotation, Vector3.up) * CylinderField (pos, upSpeed: upSpeed);
	}

	public static Vector3 TeleportField(Vector3 pos) {
		// At which height the particles start to gather near the center
		float falloffHeight = 5f;

		// At which proportion of the falloff height the particles begin to accelerate upwards
		float dissipateFalloffCoeff = 0.9f;

		// Weird name
		float coreHeight = 1/falloffHeight;

		// Basic rotation constant
		float rotation = 10f;
		//float rotation = 50f * pos.y/10;

		float upSpeed = Random.Range(0.5f, 1f)*Mathf.Max(0.1f, (pos.y-falloffHeight*dissipateFalloffCoeff));
		Vector3 velocity = ConeField(pos, (rotation*(Mathf.Max(coreHeight, Mathf.Abs(pos.y/falloffHeight)))), upSpeed: upSpeed);
		return velocity;
	}


	public static Vector3 SpecialConeField(Vector3 pos) {
		return ConeField (pos, 20f/(/*(1+2/pos.y)*/(1f)*Vector2.Distance(new Vector2(pos.x, pos.z), Vector2.zero)));
	}

	public static Vector3 FocusBeamField(Vector3 pos) {
		float height = 5;
		Vector3 velocity = Mathf.Min((1 - pos.y)/5, 0) * CylinderField (pos) + Mathf.Max((pos.y - 1)/height) * ConeField (pos);
		velocity.y += (3);
		return velocity;
	}

	public static Vector3 SpikeField(Vector3 pos) {
		Vector3 velocity = ConeField(pos, 15);
		velocity /= Vector3.Distance (pos, pos.y * Vector3.up);
		return velocity;
	}
}
