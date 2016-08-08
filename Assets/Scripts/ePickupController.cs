using UnityEngine;
using System.Collections;

public class ePickupController : MonoBehaviour {

	public GameController gameMaster;		//reference general controller

	public float moveTimer = .5f;
	private float timer;
	private bool timerOn = true;

	private float timer2;
	public float dontMoveTimer = .5f;
	private bool timerOff = false;

	void Awake()
	{
		gameMaster = GameObject.Find("Overseer").GetComponent<GameController>();
		timer = moveTimer;
		timer2 = dontMoveTimer;
	}

	void Update()
	{
//		if (timerOn)
//		{
//			timer -= Time.deltaTime;
//			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5));
//			if (timer <= 0)
//			{
//				timer = moveTimer;
//				timerOn = false;
//				timerOff = true;
//			}
//		}
//
//		if (timerOff)
//		{
//			timer2 -= Time.deltaTime;
//			if (timer2 <= 0)
//			{
//				timer2 = dontMoveTimer;
//				timerOff = false;
//				timerOn = true;
//			}
//		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//give the player the pickup if they run into it and don't already have it
		if (col.gameObject.CompareTag("Player"))
		{
			if (!gameMaster.PickUpStatus())
			{
				gameMaster.SetPickUp(true);
				Destroy(gameObject);
			}
		
		}
	}
}
