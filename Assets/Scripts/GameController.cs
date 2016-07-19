using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	[HideInInspector] public List<GameObject> activeCivs;	//all of the civilians currently 'active' in the level
	[HideInInspector] public List<GameObject> liveCivs;		//all of the active civilians that remain alive

	public int civCount = 0;								//number of civilians that made it out of the level alive
	private bool hasPickup = false;							//does the player have a pickup?

	// Use this for initialization
	void Start () 
	{
		//instantiate the lists of active and live civs
		activeCivs = new List<GameObject>();
		liveCivs = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		civCount = liveCivs.Count;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Civilian"))
		{
			if (!activeCivs.Contains(col.gameObject) && !liveCivs.Contains(col.gameObject))
			{
				activeCivs.Add(col.gameObject);
				liveCivs.Add(col.gameObject);
			}

			if (col.gameObject.GetComponent<eCivilianController>().Dying && hasPickup)
			{
				col.gameObject.GetComponent<eCivilianController>().StopDying();
				SetPickUp(false);
			}
		}
	}

	//function for getting hasPickup
	public bool PickUpStatus()
	{
		return hasPickup;
	}
		
	//function for setting hasPickup
	public void SetPickUp(bool status)
	{
		hasPickup = status;
	}
}
