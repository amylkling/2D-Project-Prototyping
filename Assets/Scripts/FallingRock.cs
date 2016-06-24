using UnityEngine;
using System.Collections;

public class FallingRock : MonoBehaviour 
{
	public GameObject RockParticles;
	void OnCollisionEnter2D(Collision2D col)
	{
		if (this.gameObject.GetComponent<Renderer>().isVisible)
		{
			Instantiate (RockParticles, this.transform.position, this.transform.rotation);
			GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera") as GameObject;
			iTweenEvent.GetEvent (cam, "ScreenPunch").Play ();
		}
//		if (col.gameObject.tag == "Civilian")
//		{
//			col.gameObject.GetComponent<eCivilianController> ().TakeDmg (34f);
//		}
		Destroy(gameObject);
	}
}
