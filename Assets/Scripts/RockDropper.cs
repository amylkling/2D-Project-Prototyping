using UnityEngine;
using System.Collections;

public class RockDropper : MonoBehaviour 
{
	public float dropTimer = 0.5f;
	public bool[] dropTimes = new bool[8];

	public GameObject fallingRock;


	private float actualTimer;
	private int dropIndex;

	void Awake()
	{
		actualTimer = dropTimer;
		dropIndex = 0;
	}

	void Update()
	{
		actualTimer -= Time.deltaTime;

		if (actualTimer <= 0)
		{
			if (dropTimes [dropIndex] == true)
			{
				GameObject Rock = (GameObject)Instantiate (fallingRock, transform.position, transform.rotation);
			}
			dropIndex++;
			if(dropIndex > 7) 
			{
				dropIndex = 0;
			}
			actualTimer = dropTimer;
		}
	}

}
