using UnityEngine;
using System.Collections;

public class CameraFollowV3 : MonoBehaviour {

	private Rigidbody2D playerRB2D;
	public eHeroController playerCntrl;

	// Use this for initialization
	void Awake () 
	{
		playerRB2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate()
	{
		transform.position = new Vector3(playerRB2D.position.x, playerRB2D.position.y, transform.position.z);
	}
}
