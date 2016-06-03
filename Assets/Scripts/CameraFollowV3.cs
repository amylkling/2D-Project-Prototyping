using UnityEngine;
using System.Collections;

public class CameraFollowV3 : MonoBehaviour {

	private Rigidbody2D playerRB2D;
	private eHeroController playerCntrl;
	private bool doCharge;
	private bool chargeDone = false;

	private float timer = 0f;
	private float chargeTime = 5f;

	public float smooth = 8f;
	private float targetX = 0f;
	private float targetY = 0f;

	// Use this for initialization
	void Awake () 
	{
		playerRB2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		playerCntrl = GameObject.FindGameObjectWithTag("Player").GetComponent<eHeroController>();
		doCharge = playerCntrl.doCharge;
		timer = chargeTime;
	}

	void Update()
	{
		doCharge = playerCntrl.doCharge;

		//while the player charges, countdown until camera can move again
		if (doCharge && !chargeDone)
		{
			timer -=  Time.deltaTime;
			if (timer < 0)
			{
				timer = chargeTime;
				chargeDone = true;
				doCharge = false;
				playerCntrl.doCharge = doCharge;
			}
		}
			
	}
	
	void FixedUpdate()
	{
		//calculate how the camera should move after a charge
		targetX = Mathf.Lerp(transform.position.x,playerRB2D.position.x,smooth*Time.fixedDeltaTime);
		targetY = Mathf.Lerp(transform.position.y,playerRB2D.position.y,smooth*Time.fixedDeltaTime);

		//camera should follow player 1:1 until they charge
		//when the player charges it should stay still until the charge is finished
		if (!doCharge && !chargeDone)
		{
			transform.position = new Vector3(playerRB2D.position.x, playerRB2D.position.y, transform.position.z);
		}
		else if (chargeDone)
		{
			transform.position = new Vector3(targetX, targetY, transform.position.z);
			chargeDone = false;
		}

	}
}
