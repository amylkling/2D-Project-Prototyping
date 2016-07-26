using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	public GameObject RockParticles;
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (col.gameObject.GetComponent<eHeroController>().doDash == true)
			{
				Instantiate (RockParticles, this.transform.position, this.transform.rotation);
				GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera") as GameObject;
				iTweenEvent.GetEvent (cam, "ScreenPunch").Play ();
				Destroy (gameObject);
			}
		}
	}
}
