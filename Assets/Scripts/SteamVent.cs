using UnityEngine;
using System.Collections;

public class SteamVent : MonoBehaviour 
{

	public float sprayTimer = 0.5f;
	//public float sprayTimeLimit = 0.25f;
	public bool[] sprayTimes = new bool[8];

	public GameObject DamageTrigger;


	private float actualTimer;
	private int sprayIndex;
	private ParticleSystem steam;

	void Start()
	{
		actualTimer = sprayTimer;
		sprayIndex = 0;
		steam = GetComponent<ParticleSystem> ();
	}

	void Update()
	{
		actualTimer -= Time.deltaTime;

		if (actualTimer <= 0)
		{
			if (sprayTimes [sprayIndex] == true)
			{
				steam.Play ();
				DamageTrigger.SetActive (true);
			} 
			else
			{
				steam.Stop ();
				DamageTrigger.SetActive (false);
			}
			sprayIndex++;
			if(sprayIndex > 7) 
			{
				sprayIndex = 0;
			}
			actualTimer = sprayTimer;
		}
	}

}
