using UnityEngine;
using System.Collections;

public class RockParticles : MonoBehaviour 
{
	private ParticleSystem particles;
	private float timer = 1f;
	// Use this for initialization
	void Start () 
	{
		particles = GetComponent<ParticleSystem> ();
		particles.Emit (15);
		//Destroy (this.gameObject);
	}
	void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			Destroy (this.gameObject);
		}
	}
}
