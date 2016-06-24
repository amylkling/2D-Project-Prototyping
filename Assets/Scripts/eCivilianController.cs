using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class eCivilianController : MonoBehaviour {

	#region Variables
	public eHeroController player;				//reference the player's script
	public Vector3 mousePos;					//holder for mouse input from player's script
	private Rigidbody2D rgdb2D;					//the civilian's rigidbody2D component
	public float walkSpeed = 3f;				//how fast the civilian should move
	public Vector2 clickPos;					//the position of the mouse when the button was clicked
	public bool isDeployPressed = false;		//prevent holding the button from doing anything
	public bool isStopPressed = false;			//prevent holding the button from doing anything
	public bool stop = false;					//whether or not the civilian moves
	public GameObject marker;					//reference the marker prefab
	public Vector3 markerPos;					//holder for the marker's spawn position
	private GameObject[] mars;					//holder for an array of markers in the scene
	public bool isSelected = false;				//whether or not a civ can be considered "selected"
	public bool isSelectPressed = false;		//prevent holding the button from doing anything
	private Vector2 mousePos2D;					//holder for the conversion of the mouse position to 2D

	public bool selectAll = false;				//whether or not to select all civs, not just this one
	private float pressTime;					//the time of the previous button press
	public float pressTimeLimit = .10f;			//the amount of time between button presses for a double tap
	public Rect boxSelect;						//holder for the invisible selection box
	public Vector2 initMousePos;				//the position of the mouse when the selection box was created

	public int maxHealth = 100;					//the maximum health of the civilian
	private int health = 100;					//the current health of the civilian
	private bool Dead = false;					//whether or not the civilian is dead
	private bool Dying = false;					//whether or not the civilian is in a dying state
	private float dyingTimer = 0f;				//the countdown for when dying turns into dead
	public float dyingTimeLimit = 30f;			//the amount of time it takes for dying to turn into dead
	public Slider healthBar;					//the UI bar that shows the civilian's health
	public CivHealthUI uiScript;				//reference the script that handles the UI functionality
	public CivHealthUIController uiControl;		//reference the script that controls the UI's existence
	#endregion

	#region Awake Function
	// Use this for initialization
	void Awake () 
	{
		//initiate player script reference and variable from it
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<eHeroController>();
		mousePos = player.mousePos;
		//initiate rigidbody2d component
		rgdb2D = GetComponent<Rigidbody2D>();
		//initiate the civilian's target position to its current position
		clickPos = rgdb2D.position;
		pressTime = Time.time;
		//initiate health
		health = maxHealth;
		dyingTimer = dyingTimeLimit;
		uiScript = gameObject.GetComponent<CivHealthUI>();
		healthBar.value = health;
		healthBar.maxValue = maxHealth;
		uiControl = uiScript.uiScript;
	}
	#endregion

	#region Update Function
	// Update is called once per frame
	void Update () 
	{
		//keep the mouse input updated
		mousePos = player.mousePos;

		//keep track of how many markers are in the scene
		mars = GameObject.FindGameObjectsWithTag("Marker");

		#region Marker Controls
		//controls for civilian movement
		if (Input.GetAxis ("Deploy") != 0)
		{
			if (isDeployPressed == false && isSelected)
			{
				//when the button is pressed, set the civ's target position
				clickPos = new Vector2(mousePos.x, rgdb2D.position.y);
				markerPos = new Vector3(clickPos.x, clickPos.y - 0.5f, 0);

				//place a marker if there isn't one already or replace the current one
				if (mars.Length == 0)
				{
					Instantiate(marker, markerPos, Quaternion.identity);
				}
				else if (mars.Length == 1)
				{
					Destroy(GameObject.FindGameObjectWithTag("Marker"));
					Instantiate(marker, markerPos, Quaternion.identity);
				}
				isDeployPressed = true;
			}
		}
		else
		{
			//when not pressed it should stay still
			isDeployPressed = false;
			clickPos.y = rgdb2D.position.y;
		}
		#endregion

		#region Stopping Controls
		//controls for stopping the civilian
		if (Input.GetAxis("Stop") != 0)
		{
			//when the button is pressed, the civilian stops or resumes movement
			if (isStopPressed == false && isSelected)
			{
				isStopPressed = true;
				stop = !stop;
			}
		}
		else
		{
			isStopPressed = false;
		}
		#endregion

		#region Selection Controls
		//controls for selecting civilians 
		if (Input.GetAxis("Select") != 0)
		{
			if (isSelectPressed == false)
			{
				#region Select All
				//when double tapped, all civilians are selected
				if (Time.time - pressTime <= pressTimeLimit)
				{
					Debug.Log("Time: " + Time.time);
					Debug.Log("Time Difference: " + (Time.time - pressTime));
					isSelected = true;
					pressTime = Time.time;
				}
				#endregion
				#region Select One
				else
				{
					//when the button is pressed while mouse is hovering over the civilian, it is selected
					//when pressed while mouse is not hovering over any civilian, the civilian is deselected
					pressTime = Time.time; 
					Debug.Log("Press Time: " + pressTime); 

					mousePos2D = new Vector2 (mousePos.x, mousePos.y);

					RaycastHit2D hit = Physics2D.Raycast (mousePos2D, Vector2.zero, 0f);

					if (hit)
					{
						if (hit.transform.gameObject.Equals (gameObject))
						{
							isSelected = !isSelected;
						}
						else if (!hit.transform.gameObject.CompareTag ("Civilian"))
						{
							isSelected = false;
						}
					}
					else
					{
						isSelected = false;
					}
				}
				#endregion
				//capture the mouse position in screen space when the button was first pressed
				initMousePos = Camera.main.WorldToScreenPoint(mousePos);

				isSelectPressed = true;
			}
			else
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
				}
				else
				{
					isSelected = false;
				}
				#endregion
			}
		}
		else
		{
			isSelectPressed = false;
		}
		#endregion

		#region Dying
		if (Dying)
		{
			Debug.Log("I'm dying!!");
			dyingTimer -= Time.deltaTime;
			if (dyingTimer <= 0)
			{
				Death();
				dyingTimer = dyingTimeLimit;
				Dying = false;
			}
		}
		#endregion

		#region Dead
		if (Dead)
		{
			Destroy(gameObject);
		}
		#endregion

		#region Temp Damaging Controls
		if (Input.GetKeyDown(KeyCode.D))
		{
			TakeDmg(34);
		}
		#endregion
	}
	#endregion

	#region Fixed Update Function
	void FixedUpdate()
	{
		//move the civilian to the target position with the set walk speed or keep it still
		if (rgdb2D.position != clickPos && !stop && !Dead && !Dying)
		{
			rgdb2D.position = Vector2.MoveTowards(rgdb2D.position, clickPos, walkSpeed * Time.deltaTime);
		}
		else
		{
			rgdb2D.position = rgdb2D.position;
		}
	}
	#endregion

	#region Take Damage Function
	public void TakeDmg(int amt)
	{
		health -= amt;
		healthBar.value = health;
		if (health <= 0 && !Dying && !Dead)
		{
			StartDying();
		}
	}
	#endregion

	#region Death Function
	void Death()
	{
		Dead = true;
	}
	#endregion

	#region Dying Function
	void StartDying()
	{
		Dying = true;
	}
	#endregion
}
