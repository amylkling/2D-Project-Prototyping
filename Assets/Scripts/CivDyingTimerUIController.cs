using UnityEngine;
using System.Collections;

public class CivDyingTimerUIController : MonoBehaviour {

	//script attached to an instance of Civilian
	public eCivilianController civScript;

	// Use this for initialization
	void Start () {

		//CivDyingTimerUI script is setting civScript to the exact instance of Civilian that this instance of Civilian Dying Timer is assigned to within CivDyingTimerUI script.


	}

	// Update is called once per frame
	void Update () {

		//destroy the dying timer if the civilian dies
		if (civScript == null)
		{
			Debug.Log ("enemyBody doesn't exist!");
			Destroy (gameObject);
		}
		//or deactivates the dying timer if the civilian is deactivated
		else if (civScript.isActiveAndEnabled == false)
		{
			gameObject.SetActive (false);
		}

	}

}
