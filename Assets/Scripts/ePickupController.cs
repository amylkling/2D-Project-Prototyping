using UnityEngine;
using System.Collections;

public class ePickupController : MonoBehaviour {

	public GameController gameMaster;		//reference general controller

	void Awake()
	{
		gameMaster = GameObject.Find("Overseer").GetComponent<GameController>();
	}

	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//give the player the pickup if they run into it and don't already have it
		if (col.gameObject.CompareTag("Player"))
		{
			if (!gameMaster.PickUpStatus())
			{
				gameMaster.SetPickUp(true);
				Destroy(gameObject);
			}
		
		}
	}
}
