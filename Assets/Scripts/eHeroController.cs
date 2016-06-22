using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class eHeroController : MonoBehaviour
{
	#region Variables
	private Rigidbody2D rgdBody2D;							//character's Rigidbody2D - necessary for physics
	private bool facingRight = true;						//keep track of which way the character is facing

	float horiz = 0f;										//holder for horizontal axis input

	public bool mouseInput = true;							//whether or not to use mouse input
	private Vector2 inpt = new Vector2(0,0);				//holder for 2D mouse input vector
	public Vector3 mousePos;								//holder for adjusted mouse input
	public float mouseSpeed = .1f;							//mouse speed multiplier

	CursorLockMode desiredState;							//for cursor control

	public bool doDash = false;								//whether or not dash attack has been initiated
	public float dashSpeed = 10f;							//how fast the dash attack moves
	public int dashThreshold = 10;							//distance between mouse and screen for a dash attack
	private float screenRight = 0f;							//calculated distance from screen edge for dash attack
	private float screenTop = 0f;							//calculated distance from screen edge for dash attack
	private float screenLB = 0f;							//calculated distance from screen edge for dash attack

	private float dashCountDown = 0f;						//holder for dash attack duration timer
	private float dashCoolTimer = 0f;						//holder for dash cooldown timer
	public float dashCoolDown = .25f;						//how long the dash attack cooldown is
	public float dashDuration = 3.25f;						//how long the dash attack lasts
	private bool dashStart = false;							//starts countdown timer when true
	private bool coolTimerOn = false;						//prevents continuous dash attack use

	public Vector3 mousePosRaw;								//holder for raw mouse input
	#endregion

	#region Awake Function
	private void Awake ()
	{
		//assign Rigidbody2D component
		rgdBody2D = GetComponent<Rigidbody2D> ();
		//set CursorLockMode to Confined, so the mouse will stay in the game window
		desiredState = CursorLockMode.Confined;
		//calculate the distance from the screen that the mouse needs to be to initiate a dash attack
		screenRight = Screen.width * 0.95f - dashThreshold;
		screenTop = Screen.height * 0.95f - dashThreshold;
		screenLB += dashThreshold;
		//set timers
		dashCountDown = dashDuration;
		dashCoolTimer = dashCoolDown;
	}
	#endregion

	#region FixedUpdate Function
	private void FixedUpdate ()
	{
		//move the character according to player input, unless doing a dash attack
		//doing a dash attack takes control away from the player
		if (!doDash)
		{
			#region Mouse Movement
			//move the character based on player's mouse input and a speed multiplier
			if (mouseInput)
			{
				rgdBody2D.MovePosition (Vector2.Lerp (transform.position, inpt, mouseSpeed));
			}
			#endregion
		}
		else
		{
			#region Initiate Dash Attack
			if (mouseInput)
			{
				Dash();
			}
			#endregion
		}

		#region Facing Direction
		// If the input is moving the player right and the player is facing left...
		if (horiz > 0 && !facingRight) {
			// ... flip the player.
			Flip ();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (horiz < 0 && facingRight) {
			// ... flip the player.
			Flip ();
		}
		#endregion

	}
	#endregion

	#region Update Function
	// Update is called once per frame
	void Update ()
	{
		#region Cursor Function Call
		//call the function to control the cursor
		SetCursorState ();
		#endregion

		#region Dash Duration Timer
		//timer for dash attack duration
		if (dashStart)
		{
			dashCountDown -= Time.deltaTime;
			if (dashCountDown < 0)
			{
				dashCountDown = dashDuration;
				dashStart = false;
				doDash = false;
				coolTimerOn = true;
			}
		}
		else
		{
			dashCountDown = dashDuration;
		}
		#endregion

		#region Dash Cooldown Timer
		//timer for dash attack cooldown
		if (coolTimerOn)
		{
			dashCoolTimer -= Time.deltaTime; 
			if (dashCoolTimer < 0)
			{
				dashCoolTimer = dashCoolDown;
				coolTimerOn = false;
			}
		}
		#endregion

		//get the input values for both axes
		horiz = Input.GetAxis ("Horizontal");


		#region Mouse Input Key
		//only allow player movement when "Fire1" key is held down
		if (Input.GetAxis("Fire1") != 0)
		{
			mouseInput = true;
		}
		else
		{
			mouseInput = false;
		}
		#endregion

		#region Mouse Input
		//get mouse position in screen space
		mousePosRaw = Input.mousePosition;
		//convert mouse position to world space
		mousePos = Camera.main.ScreenToWorldPoint(mousePosRaw);
		inpt = new Vector2 (mousePos.x, mousePos.y);
		#endregion

		#region Dash Calculations
		//calculate the distance between mouse and edge of screen to determine when to do a dash attack
		if ((mousePosRaw.x >= screenRight || mousePosRaw.y >= screenTop || mousePosRaw.x <= screenLB || mousePosRaw.y <= screenLB) && !coolTimerOn)
		{
			dashStart = true;
			doDash = true;
		}
		else
		{
			dashStart = false;
			doDash = false;
		}
		#endregion

		#region Quit Game
		//quit game when escape is pressed
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit ();
		}
		#endregion

	}
	#endregion

	#region Flip Function
	private void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	#endregion

	#region SetCursorState Function
	// Apply requested cursor state
	void SetCursorState ()
	{
		Cursor.lockState = desiredState;
		// Hide cursor when locking
		//Cursor.visible = (CursorLockMode.Confined != desiredState);
	}
	#endregion

	#region Dash Function
	//cause the player to move faster
	void Dash()
	{
		rgdBody2D.MovePosition(Vector2.Lerp(rgdBody2D.position, inpt, dashSpeed * Time.deltaTime));
		Debug.Log("CHARGE!!");
	}
	#endregion

	#region On Collision Enter 2D Function
	void OnCollisionEnter2D(Collision2D col)
	{
		#region Civilian Collision
		//stop colliding with civs
		if (col.gameObject.CompareTag("Civilian"))
		{
			Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), 
				col.gameObject.GetComponent<Collider2D>());
		}
		#endregion

		#region Breakable Object Collision
		//break breakable objects when dashing
		if (doDash)
		{
			if (col.gameObject.CompareTag("Breakable"))
			{
				Destroy(col.gameObject);
			}
		}
		#endregion
	}
	#endregion
}
