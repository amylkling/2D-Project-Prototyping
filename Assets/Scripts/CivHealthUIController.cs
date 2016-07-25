using UnityEngine;
using System.Collections;

public class CivHealthUIController : MonoBehaviour {

	//script attached to an instance of Civilian
	public eCivilianController civScript;

	// Use this for initialization
	void Start () {

		//CivHealthUI script is setting civScript to the exact instance of Civilian that this instance of Civilian Health is assigned to within CivHealthUI script.


	}

	// Update is called once per frame
	void Update () {

		//destroy the health bar if the civilian dies
		if (civScript == null)
		{
			Debug.Log ("enemyBody doesn't exist!");
			Destroy (gameObject);
		}
		//or deactivates the health bar if the civilian is deactivated
		else if (civScript.isActiveAndEnabled == false)
		{
			gameObject.SetActive (false);
		}

	}
		
}
