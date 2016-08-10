using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class eCivilianController : MonoBehaviour {

	#region Variables
	public eHeroController player;								//reference the player's script
	[HideInInspector] public Vector3 mousePos;					//holder for mouse input from player's script
	private Rigidbody2D rgdb2D;									//the civilian's rigidbody2D component
	public float walkSpeed = 3f;								//how fast the civilian should move
//	public Vector2 clickPos;									//the position of the mouse when the button was clicked
	[HideInInspector] public Vector2 destPos;					//the destination as determined by CivRTSUnitHandling
//	public bool isDeployPressed = false;						//prevent holding the button from doing anything
//	public bool isDeployed = false;								//whether or not a marker exists
	public bool stop = false;									//whether or not the civilian moves
//	public GameObject marker;									//reference the marker prefab
//	public Vector3 markerPos;									//holder for the marker's spawn position
//	private GameObject[] mars;									//holder for an array of markers in the scene
	public bool isSelected = false;								//whether or not a civ can be considered "selected"
	[HideInInspector] public bool isSelectPressed = false;		//prevent holding the button from doing anything
	private Vector2 mousePos2D;									//holder for the conversion of the mouse position to 2D

	public bool selectAll = false;								//whether or not to select all civs, not just this one
	private float pressTime;									//the time of the previous button press
	public float pressTimeLimit = .10f;							//the amount of time between button presses for a double tap
	[HideInInspector] public Rect boxSelect;					//holder for the invisible selection box
	[HideInInspector] public Vector2 initMousePos;				//the position of the mouse when the selection box was created
	public bool noMarquee = false;								//determine whether or not to use marquee/box selection

	public int maxHealth = 100;									//the maximum health of the civilian
	private int health = 100;									//the current health of the civilian
	private bool Dead = false;									//whether or not the civilian is dead
	public bool Dying = false;									//whether or not the civilian is in a dying state
	private float dyingTimer = 0f;								//the countdown for when dying turns into dead
	public float dyingTimeLimit = 30f;							//the amount of time it takes for dying to turn into dead
	public Slider healthBar;									//the UI bar that shows the civilian's health
	public bool damaged = false;
	public Slider dyingTimerUI;									//the UI that displays the time left until the civilian dies

	[HideInInspector] public bool invincibleTimerOn = false;	//start count down on how long invincibility lasts
	private float invincibleTimer = 0f;							//countdown for how long invincibility lasts
	public float invincibleTimeLimit = 5f;						//how long invincibility should last

	private bool facingRight = true;							//keep track of which way the character is facing
	public CivRTSUnitHandling unitHandling;						//reference the civilian RTS unit-style handling script
	[HideInInspector] public List<Collision2D> otherCivs;		//store the collision data of any civs this civ collides with
	[HideInInspector] public Pause pauseMenu;					//reference the pausing script on the menu UI
	public GameController gameMaster;							//reference the general control script 

	#endregion

	#region Awake Function
	// Use this for initialization
	void Awake () 
	{
		//initialize player script reference and variable from it
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<eHeroController>();
		mousePos = player.mousePos;
		//initialize rigidbody2d component
		rgdb2D = GetComponent<Rigidbody2D>();
		//initialize the civilian's target position to its current position
		//clickPos = rgdb2D.position;
		destPos = rgdb2D.position;
		pressTime = Time.time;
		//initialize health
		health = maxHealth;
		healthBar.value = health;
		healthBar.maxValue = maxHealth;
		dyingTimer = dyingTimeLimit;
		invincibleTimer = invincibleTimeLimit;
		dyingTimerUI.maxValue = dyingTimeLimit;
		dyingTimerUI.value = dyingTimeLimit;
		//instantiate the list for collisions with other civs
		otherCivs = new List<Collision2D>();
		//initialize pause script if running from main menu
		if (GameObject.Find("UI") != null)
		{
			pauseMenu = GameObject.Find("UI").GetComponent<Pause>();
		}
		//initialize general control script
		gameMaster = GameObject.Find("Overseer").GetComponent<GameController>();
	}
	#endregion

	#region Update Function
	// Update is called once per frame
	void Update () 
	{
		//keep the mouse input updated
		mousePos = player.mousePos;

		//keep track of how many markers are in the scene
//		mars = GameObject.FindGameObjectsWithTag("Marker");

		#region Marker Controls - Now Controlled By CivRTSUnitHandling
		//controls for civilian movement
//		if (Input.GetButtonDown("Deploy"))
//		{
//			if (isDeployPressed == false && isSelected)
//			{
//				//when the button is pressed, set the civ's target position
//				clickPos = new Vector2(mousePos.x, rgdb2D.position.y);
//				markerPos = new Vector3(clickPos.x, clickPos.y - 0.5f, 0);
//
//				//place a marker if there isn't one already or replace the current one
//				if (mars.Length == 0)
//				{
//					Instantiate(marker, markerPos, Quaternion.identity);
//				}
//				else if (mars.Length == 1)
//				{
//					Destroy(GameObject.FindGameObjectWithTag("Marker"));
//					Instantiate(marker, markerPos, Quaternion.identity);
//				}
//				isDeployed = true;
//				isDeployPressed = true;
//			}
//		}
//		else
//		{
//			//when not pressed it should stay still
//			isDeployPressed = false;
//			clickPos.y = rgdb2D.position.y;
//		}
		#endregion

		#region Stopping Controls
		//controls for stopping the civilian
		if (Input.GetButtonDown("Stop"))
		{
			if (pauseMenu == null || !pauseMenu.Paused())
			{
				if (gameMaster.activeCivs.Contains(gameObject))
				{
					//when the button is pressed, the civilian stops or resumes movement
					if (isSelected)
					{
						stop = !stop;
					}
				}
			}
		}
		#endregion

		#region BAD Selection Controls
//		//controls for selecting civilians 
//		if (Input.GetAxis("Select") != 0)
//		{
//			if (isSelectPressed == false)
//			{
//				#region Select All
//				//when double tapped, all civilians are selected
//				if (Time.time - pressTime <= pressTimeLimit)
//				{
//					Debug.Log("Time: " + Time.time);
//					Debug.Log("Time Difference: " + (Time.time - pressTime));
//					isSelected = true;
//					Debug.Log("isSelected: " + isSelected.ToString());
//					pressTime = Time.time;
//				}
//				#endregion
//				#region Select One
//				else
//				{
//					//when the button is pressed while mouse is hovering over the civilian, it is selected
//					//when pressed while mouse is not hovering over any civilian, the civilian is deselected
//					pressTime = Time.time; 
//					Debug.Log("Press Time: " + pressTime); 
//
//					mousePos2D = new Vector2 (mousePos.x, mousePos.y);
//
//					RaycastHit2D hit = Physics2D.Raycast (mousePos2D, Vector2.zero, 0f);
//
//					if (hit)
//					{
//						if (hit.transform.gameObject.Equals (gameObject))
//						{
//							isSelected = !isSelected;
//						}
//						else if (!hit.transform.gameObject.CompareTag ("Civilian"))
//						{
//							isSelected = false;
//						}
//					}
//					else
//					{
//						isSelected = false;
//					}
//				}
//				#endregion
//				//capture the mouse position in screen space when the button was first pressed
//				initMousePos = Camera.main.WorldToScreenPoint(mousePos);
//
//				isSelectPressed = true;
//			}
//			else
//			{
//				#region Selection Box
//				//when held down and the mouse is dragged, creates a selection box that selects all civilians within
//				boxSelect = new Rect(Mathf.Min(initMousePos.x, Camera.main.WorldToScreenPoint(mousePos).x), 
//									Mathf.Min(initMousePos.y, Camera.main.WorldToScreenPoint(mousePos).y), 
//									Mathf.Abs(initMousePos.x - Camera.main.WorldToScreenPoint(mousePos).x), 
//									Mathf.Abs(initMousePos.y - Camera.main.WorldToScreenPoint(mousePos).y));
//				
//				if (boxSelect.Contains(Camera.main.WorldToScreenPoint(transform.position)))
//				{
//					isSelected = true;
//				}
//				else
//				{
//					isSelected = false;
//				}
//				#endregion
//			}
//		}
//		else
//		{
//			isSelectPressed = false;
//		}
		#endregion

		#region Selection Controls
		if(Input.GetButtonDown("Select"))
		{
			if (pauseMenu == null || !pauseMenu.Paused())
			{
				if (gameMaster.activeCivs.Contains(gameObject))
				{
					#region Select All
					//when double tapped, all civilians are selected
					if (Time.time - pressTime <= pressTimeLimit)
					{
		//				Debug.Log("Time: " + Time.time);
		//				Debug.Log("Time Difference: " + (Time.time - pressTime));
						isSelected = true;
						//when a civilian is selected, check if it is in the selectedCivs list before adding it
						if (!unitHandling.selectedCivs.Contains(gameObject))
						{
							unitHandling.selectedCivs.Add(gameObject);
						}
		//				Debug.Log("isSelected: " + isSelected.ToString());
						pressTime = Time.time;

						noMarquee = true;
		//				Debug.Log("all");
					}
					#endregion
					#region Select One
					else
					{
						//when the button is pressed while mouse is hovering over the civilian, it is selected
						//when pressed while mouse is not hovering over any civilian, the civilian is deselected
						pressTime = Time.time; 
		//				Debug.Log("Press Time: " + pressTime); 

						mousePos2D = new Vector2 (mousePos.x, mousePos.y);

						RaycastHit2D hit = Physics2D.Raycast (mousePos2D, Vector2.zero, 0f);

						if (hit)
						{
							if (hit.transform.gameObject.Equals (gameObject))
							{
		//						Debug.Log("HIT");
								isSelected = !isSelected;
								if (isSelected)
								{
									//when a civilian is selected, check if it is in the selectedCivs list before adding it
									if (!unitHandling.selectedCivs.Contains(gameObject))
									{
										unitHandling.selectedCivs.Add(gameObject);
									}

								}
								else
								{
									//when a civilian is deselected, check if it is in the selectedCivs list before removing it
									if (unitHandling.selectedCivs.Contains(gameObject))
									{
										unitHandling.selectedCivs.Remove(gameObject);
									}
								}
							}
							else if (!hit.transform.gameObject.CompareTag ("Civilian"))
							{
								isSelected = false;
								//when a civilian is deselected, check if it is in the selectedCivs list before removing it
								if (unitHandling.selectedCivs.Contains(gameObject))
								{
									unitHandling.selectedCivs.Remove(gameObject);
								}
							}

							//prevent selection box from being used if single select happened
							noMarquee = true;
						}
						else
						{
							isSelected = false;
							//when a civilian is deselected, check if it is in the selectedCivs list before removing it
							if (unitHandling.selectedCivs.Contains(gameObject))
							{
								unitHandling.selectedCivs.Remove(gameObject);
							}
							noMarquee = false;
						}

		//				Debug.Log("one");
					}
					#endregion
					//capture the mouse position in screen space when the button was first pressed
					initMousePos = Camera.main.WorldToScreenPoint(mousePos);
				}
			}
		}
			
		if(Input.GetButton("Select"))
		{
			if (pauseMenu == null || !pauseMenu.Paused())
			{
				if (gameMaster.activeCivs.Contains(gameObject))
				{
					if (noMarquee == false)
					{
						#region Selection Box
						//when held down and the mouse is dragged, creates a selection box that selects all civilians within
						boxSelect = new Rect(Mathf.Min(initMousePos.x, Camera.main.WorldToScreenPoint(mousePos).x), 
							Mathf.Min(initMousePos.y, Camera.main.WorldToScreenPoint(mousePos).y), 
							Mathf.Abs(initMousePos.x - Camera.main.WorldToScreenPoint(mousePos).x), 
							Mathf.Abs(initMousePos.y - Camera.main.WorldToScreenPoint(mousePos).y));

						if (boxSelect.Contains(Camera.main.WorldToScreenPoint(transform.position)))
						{
							isSelected = true;
							//when a civilian is selected, check if it is in the selectedCivs list before adding it
							if (!unitHandling.selectedCivs.Contains(gameObject))
							{
								unitHandling.selectedCivs.Add(gameObject);
							}
						}
						else
						{
		//					Debug.Log("OUTSIDE");
							isSelected = false;
							//when a civilian is deselected, check if it is in the selectedCivs list before removing it
							if (unitHandling.selectedCivs.Contains(gameObject))
							{
								unitHandling.selectedCivs.Remove(gameObject);
							}
						}
						#endregion

		//				Debug.Log("box");
						isSelectPressed = true;
					}
				}
			}
		}
		else
		{
			isSelectPressed = false;
		}
		#endregion

		#region Is Selected Visual Feedback
		//enable the Halo component if the civ is selected
		if (isSelected)
		{
			(gameObject.GetComponent("Halo") as Behaviour).enabled = true;
		}
		else
		{
			(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
		}
		#endregion

		#region Dying
		if (Dying)
		{
			//start a timer for how long to remain in the Dying state before switching to Death state
			Debug.Log(gameObject.name + ": I'm dying!!");
			dyingTimer -= Time.deltaTime;
			dyingTimerUI.value = dyingTimer;
			if (dyingTimer <= 0)
			{
				Death();
				dyingTimer = dyingTimeLimit;
				dyingTimerUI.value = dyingTimeLimit;
				Dying = false;
			}
		}
		else
		{
			dyingTimer = dyingTimeLimit;
			dyingTimerUI.value = dyingTimeLimit;
		}
		#endregion

		#region Dead
		if (Dead)
		{
			//remove the civ, if previously selected, from the selected civs list
			if (unitHandling.selectedCivs.Contains(gameObject))
			{
				unitHandling.selectedCivs.Remove(gameObject);
			}

			if (gameMaster.activeCivs.Contains(gameObject) && gameMaster.liveCivs.Contains(gameObject))
			{
				gameMaster.activeCivs.Remove(gameObject);
				gameMaster.liveCivs.Remove(gameObject);
			}

			//Dead is dead
			Destroy(gameObject);
		}
		#endregion

		#region Invincibility Timer
		if (invincibleTimerOn)
		{
			//start a timer for how long invincibility lasts
			//when it runs out, allow civ to move and get damaged again
			healthBar.gameObject.GetComponentInChildren<Image>().color = Color.white;
			invincibleTimer -= Time.deltaTime;
			if (invincibleTimer <= 0)
			{
				invincibleTimer = invincibleTimeLimit;
				invincibleTimerOn = false;
				damaged = false;
				stop = false;
			}
		}
		#endregion

		//prevent civ from constantly bouncing
		destPos.y = rgdb2D.position.y;

	}
	#endregion

	#region Fixed Update Function
	void FixedUpdate()
	{
		//move the civilian to the target position with the set walk speed or keep it still
		if (rgdb2D.position != destPos && !stop && !Dead && !Dying && !damaged)
		{
			Debug.Log(gameObject.name + " is marching on!");
			rgdb2D.position = Vector2.MoveTowards(rgdb2D.position, destPos, walkSpeed * Time.deltaTime);
		}
		else
		{
			stop = true;
		}

		//after stopping/dying/being dead, turn collision back on for any civs previously collided with
		if (stop || Dead || Dying)
		{
			if (otherCivs.Count != 0)
			{
				int count = otherCivs.Count;

				for(int i = count; i > 0; i--)
				{
					Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), 
						otherCivs[i-1].gameObject.GetComponent<Collider2D>(), ignore: false);

					otherCivs.Remove(otherCivs[i-1]);
				}	
			}
		}


		#region Facing Direction
		// If the destination is to the right and the civ is facing left...
		if (destPos.x > rgdb2D.position.x && !facingRight && !stop) {
			// ... flip the player.
			Flip ();
		}
		// Otherwise if the destination is to the left and the civ is facing right...
		else if (destPos.x < rgdb2D.position.x && facingRight && !stop) {
			// ... flip the player.
			Flip ();
		}
		#endregion
	}
	#endregion

	#region Take Damage Function
	public void TakeDmg(int amt)
	{
		//take damage if...
		if (!Dying && !Dead)
		{
			//...not in the state of dying or dead
			if (pauseMenu == null || !pauseMenu.Paused())
			{
				//...the pause menu doesn't exist or the game is not paused
				if (gameMaster.activeCivs.Contains(gameObject))
				{
					//...it is active
					if (invincibleTimerOn == false)
					{
						//...and not invincible
						health -= amt;
						healthBar.value = health;

						//enter Dying state if no more health, otherwise enter damaged state and become invincible
						if (health <= 0)
						{
							StartDying ();
						}
						else
						{
							invincibleTimerOn = true;
							damaged = true;
						}
					}
				}
			}
		}

		Debug.Log(gameObject.name + "'s Health: " + health.ToString());

	}
	#endregion

	#region Death Function
	void Death()
	{
		Dead = true;
	}
	#endregion

	#region Start Dying Function
	void StartDying()
	{
		Dying = true;
	}
	#endregion

	#region Stop Dying Function
	public void StopDying()
	{
		Dying = false;
		health = maxHealth;
		healthBar.value = health;
		invincibleTimerOn = true;
	}
	#endregion

	#region Flip Function
	private void Flip ()
	{
		// Switch the way the civ is labelled as facing.
		facingRight = !facingRight;

		// Multiply the civ's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	#endregion

	#region On Collision Enter Function
	void OnCollisionEnter2D(Collision2D col)
	{
		//if collided with another civ while moving, turn off collision with that civ
		if (col.gameObject.CompareTag("Civilian") && !stop && !Dead && !Dying)
		{
			otherCivs.Add(col);

			Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), 
				col.gameObject.GetComponent<Collider2D>());
		}
	}
	#endregion

	#region On Collision Stay Function
	void OnCollisionStay2D(Collision2D col)
	{
		//after turning on collision again, if the colliders are still touching, 
		//separate them by a minuscule amount according to relative positioning
		//this allows On Collision Enter to be called again without any extra input from the player
		if (col.gameObject.CompareTag("Civilian"))
		{
			if(col.gameObject.GetComponent<Rigidbody2D>().position.x > rgdb2D.position.x)
			{
				rgdb2D.MovePosition(new Vector2(rgdb2D.position.x - .01f, rgdb2D.position.y));
			}
			else if (col.gameObject.GetComponent<Rigidbody2D>().position.x < rgdb2D.position.x)
			{
				rgdb2D.MovePosition(new Vector2(rgdb2D.position.x + .01f, rgdb2D.position.y));
			}
		}
	}
	#endregion

	#region On Became Visible Function
	void OnBecameVisible()
	{
		//when this civilian becomes visible to any camera, mark it as active and alive
		//if it isn't already marked
		if (!gameMaster.activeCivs.Contains(gameObject))
		{
			gameMaster.activeCivs.Add(gameObject);
		}

		if (!Dead && !gameMaster.liveCivs.Contains(gameObject))
		{
			gameMaster.liveCivs.Add(gameObject);
		}
	}
	#endregion
}
