using UnityEngine;
using System.Collections;

public class Stalactite : MonoBehaviour 
{
	private bool activated = false;
	// Update is called once per frame
	void FixedUpdate () 
	{
		RaycastHit2D hit = Physics2D.Raycast (this.gameObject.transform.GetChild(0).position, Vector2.down);

		//Debug.DrawRay (this.transform.position, hit);
		if (hit.collider.tag == "Civilian" && activated == false)
		{
			activated = true;
			iTweenEvent.GetEvent (this.gameObject, "Shake").Play();
		}
	}
	void FallDown()
	{
		Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D> ();
		rb.gravityScale = 2f;
	}
}
