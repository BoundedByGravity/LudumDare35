using UnityEngine;
using System.Collections;

public class ParticleSystemField : MonoBehaviour {

	ParticleSystem ps;
	ParticleSystem.Particle[] particles;

	public float speed = 1f;

	// Use this for initialization
	void Start () {
	}

	void FixedUpdate() {
		InitializeIfNeeded ();
		int particlesAlive = ps.GetParticles(particles);
		for (int i = 0; i < particlesAlive; i++) {
			Vector3 pos = particles [i].position;
			//pos.y += 2;
			// Can also be done non-normalized for cewl effects
			Vector3 dir = VectorFields.TeleportField (pos).normalized;
			particles[i].velocity = speed*(dir);
			particles [i].lifetime = (20-particles[i].position.y);
		}
		ps.SetParticles(particles, particlesAlive);
	}

	void InitializeIfNeeded()
	{
		if (ps == null)
			ps = GetComponent<ParticleSystem>();

		if (particles == null || particles.Length < ps.maxParticles)
			particles = new ParticleSystem.Particle[ps.maxParticles]; 
	}
}
