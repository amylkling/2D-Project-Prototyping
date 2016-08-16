using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class LevelEndTrigger : MonoBehaviour {

	public GameController gameMaster; 		//the miscellaneous control script on Overseer
	public int rescuedCivs = 0; 			//the number of civilians that have safely reached this point

	// Use this for initialization
	void Start ()
	{
		gameMaster = GameObject.Find("Overseer").GetComponent<GameController>();
	}

	void Update()
	{
		//when all active civs are brought to this spot that marks the end of the section,
		//a "level cleared" screen needs to pop up and tell the player how many civs they rescued
		//out of how many there were total.

		//then all the rescued civs need to "leave" the level - preferably through a cutscene,
		//but more practically via a fade-out/fade-in.
		//^maybe that should happen before the results screen?
		//the civs are deleted at that time

		//after the player clicks "continue" or whatever, maybe after a certain amount of time,
		//the next section opens up, again via a cutscene or fade-out/fade-in, and the player moves on

		if (gameMaster.civCount != 0)
		{
			if (rescuedCivs == gameMaster.civCount)
			{
				Debug.Log("YOU WIN!!!!");
			}
			else if (rescuedCivs < gameMaster.civCount && rescuedCivs != 0)
			{
				Debug.Log("YOU LOSE!");
			}
			else if (rescuedCivs == 0)
			{
				Debug.Log("Waiting for Civilians...");
			}
		}

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Civilian"))
		{
			if (gameMaster.activeCivs.Contains(col.gameObject))
			{
				//when an active civilian enters the trigger, count it as rescued 
				//and prevent the player from further interaction with it
				rescuedCivs++;
				gameMaster.activeCivs.Remove(col.gameObject);
				col.gameObject.GetComponent<eCivilianController>().isSelected = false;
				gameMaster.gameObject.GetComponent<CivRTSUnitHandling>().selectedCivs.Remove(col.gameObject);
			}
		}
	}
}
