using UnityEngine;
using System.Collections;

public class eCivilianController : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () 
	{
		//keep the mouse input updated
		mousePos = player.mousePos;

		//keep track of how many markers are in the scene
		mars = GameObject.FindGameObjectsWithTag("Marker");

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

		//controls for selecting civilians 
		if (Input.GetAxis("Select") != 0)
		{
			if (isSelectPressed == false)
			{
				//when double tapped, all civilians are selected
				if (Time.time - pressTime <= pressTimeLimit)
				{
					Debug.Log("Time: " + Time.time);
					Debug.Log("Time Difference: " + (Time.time - pressTime));
					isSelected = true;
					pressTime = Time.time;
				}
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
				//capture the mouse position in screen space when the button was first pressed
				initMousePos = Camera.main.WorldToScreenPoint(mousePos);

				isSelectPressed = true;
			}
			else
			{
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
			}
		}
		else
		{
			isSelectPressed = false;
		}
	}

	void FixedUpdate()
	{
		//move the civilian to the target position with the set walk speed or keep it still
		if (rgdb2D.position != clickPos && !stop)
		{
			rgdb2D.position = Vector2.MoveTowards(rgdb2D.position, clickPos, walkSpeed * Time.deltaTime);
		}
		else
		{
			rgdb2D.position = rgdb2D.position;
		}
	}
}
