using UnityEngine;
using System.Collections;

public class eCitizenController : MonoBehaviour {

	public eHeroController player;				//reference the player's script
	public Vector3 mousePos;					//holder for mouse input from player's script
	private Rigidbody2D rgdb2D;					//the citizen's rigidbody2D component
	public float walkSpeed = 3f;				//how fast the citizen should move
	public Vector2 clickPos;					//the position of the mouse when the button was clicked
	public bool isFire1Pressed = false;			//prevent holding the button from doing anything

	// Use this for initialization
	void Awake () 
	{
		//initiate player script reference and variable from it
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<eHeroController>();
		mousePos = player.mousePos;
		//initiate rigidbody2d component
		rgdb2D = GetComponent<Rigidbody2D>();
		//initiate the citizen's target position to its current position
		clickPos = rgdb2D.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//keep the mouse input updated
		mousePos = player.mousePos;

		//when the button is pressed, set the target position for the citizen's movement
		//otherwise it should stay still
		if (Input.GetAxis ("Fire2") == 1)
		{
			if (isFire1Pressed == false)
			{
				clickPos = new Vector2(mousePos.x, rgdb2D.position.y);
				isFire1Pressed = true;
			}
		}
		else
		{
			isFire1Pressed = false;
			clickPos.y = rgdb2D.position.y;
		}
	}

	void FixedUpdate()
	{
		//move the citizen to the target position with the set walk speed
		if (rgdb2D.position != clickPos)
		{
			rgdb2D.position = Vector2.MoveTowards(rgdb2D.position, clickPos, walkSpeed * Time.deltaTime);
		}
	}
}
