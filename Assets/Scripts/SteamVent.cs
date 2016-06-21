using UnityEngine;
using System.Collections;

public class SteamVent : MonoBehaviour 
{

	public float sprayTimer = 0.5f;
	public bool[] sprayTimes = new bool[8];


	private float actualTimer;
	private int sprayIndex;
	private ParticleSystem steam;

	void Start()
	{
		actualTimer = sprayTimer;
		sprayIndex = 0;
		steam = GetComponent<ParticleSystem> ();
		//steam.emission.rate = 0;
	}

	void Update()
	{
		actualTimer -= Time.deltaTime;

		if (actualTimer <= 0)
		{
			if (sprayTimes [sprayIndex] == true)
			{
				Debug.Log (actualTimer);
				steam.Emit(10);
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
