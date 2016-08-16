using UnityEngine;
using System.Collections;

public class FallingRock : MonoBehaviour 
{
	public GameObject RockParticles;


	void Update()
	{
		//send a raycast from the bottom of the rock downward
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, 
			gameObject.transform.position.y - gameObject.GetComponent<CircleCollider2D>().radius), Vector2.down);

//		Debug.DrawRay (new Vector3(gameObject.transform.position.x, 
//			gameObject.transform.position.y - gameObject.GetComponent<CircleCollider2D>().radius, 0), Vector3.down);

		if (hit.collider.gameObject.CompareTag("Civilian"))
		{
			//if the raycast finds a civ that is invincible or dying, don't hit it
			Debug.Log("look out below!");
			if (hit.collider.gameObject.GetComponent<eCivilianController>().invincibleTimerOn ||
				hit.collider.gameObject.GetComponent<eCivilianController>().Dying)
			{
				Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), hit.collider);
			}
		}
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (this.gameObject.GetComponent<Renderer>().isVisible)
		{
			Instantiate (RockParticles, this.transform.position, this.transform.rotation);
			GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera") as GameObject;
			iTweenEvent.GetEvent (cam, "ScreenPunch").Play ();
		}
		if (col.gameObject.CompareTag("Civilian"))
		{
			col.gameObject.GetComponent<eCivilianController> ().TakeDmg (34);
		}
		Destroy(gameObject);
	}
}
