using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class eHeroController : MonoBehaviour
{
	#region Variables
	[SerializeField] private float maxHorzSpeed = 10f;		//maximum player speed - horizontal axis
	[SerializeField] private float maxVertSpeed = 5f;		//maximum player speed - vertical axis
	[SerializeField] private float maxDiagSpeed = 8f;		//maximum player speed - diagonal
	//Vector2 deltaSpeed;
	private Rigidbody2D rgdBody2D;							//character's Rigidbody2D - necessary for physics
	private bool facingRight = true;						//keep track of which way the character is facing
	float horiz = 0f;										//holder for horizontal axis input
	float horizRaw = 0f;									//holder for raw horizontal axis input
	//float mHoriz = 0f;
	float verti = 0f;										//holder for vertical axis input
	float vertiRaw = 0f;									//holder for raw vertical axis input
	//float mVerti = 0f;
	public bool mouseInput = true;							//whether or not to use mouse input
	private Vector2 inpt = new Vector2(0,0);				//holder for 2D mouse input vector
	public Vector3 mousePos;								//holder for adjusted mouse input
	public float mouseSpeed = .1f;							//mouse speed multiplier

	CursorLockMode desiredState;							//for cursor control

	public bool doDash = false;							//whether or not dash attack has been initiated
	public float dashSpeed = 10f;							//how fast the dash attack moves
	public int dashThreshold = 10;						//distance between mouse and screen for a dash attack
	private float screenRight = 0f;							//calculated distance from screen edge for dash attack
	private float screenTop = 0f;							//calculated distance from screen edge for dash attack
	private float screenLB = 0f;							//calculated distance from screen edge for dash attack
//	private float distance = 0f;							//holder for calculated distance between mouse and player

	private float dashCountDown = 0f;						//holder for dash attack duration timer
	private float dashCoolTimer = 0f;						//holder for dash cooldown timer
	public float dashCoolDown = .25f;						//how long the dash attack cooldown is
	public float dashDuration = 3.25f;						//how long the dash attack lasts
	private bool dashStart = false;						//starts countdown timer when true
	private bool coolTimerOn = false;						//prevents continuous dash attack use

//	private bool isFire1Pressed = false;					//check that any "Fire1" key isn't held down
//	private bool isFire2Pressed = false;					//check that any "Fire2" key isn't held down

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
		#region Movement Code Attempts
//		rgdBody2D.velocity = new Vector2(horiz*maxHorzSpeed, rgdBody2D.velocity.y);
//		rgdBody2D.velocity = new Vector2(rgdBody2D.velocity.x, verti*maxVertSpeed);
//		rgdBody2D.velocity = new Vector2(horiz*maxHorzSpeed, verti*maxVertSpeed);
		#endregion

		//move the character according to player input, unless doing a dash attack
		//doing a dash attack takes control away from the player
		if (!doDash)
		{
			#region Keyboard Movement
			//move the character based on player's keyboard input and a speed multiplier
//			if (!mouseInput)
//			{
//				rgdBody2D.velocity = new Vector2 (horiz * maxHorzSpeed, verti * maxVertSpeed);
//				if (horizRaw != 0 && vertiRaw != 0)
//				{
//					//diagonal movement with a different speed multiplier
//					rgdBody2D.velocity = new Vector2 (horiz * maxDiagSpeed, verti * maxDiagSpeed);
//				}
//			}
			#endregion

			#region Mouse Movement
			//move the character based on player's mouse input and a speed multiplier
			if (mouseInput)
			{
				#region Fail Mouse Movement
				//rgdBody2D.MovePosition(new Vector2(Screen.width - (inpt.x + (deltaSpeed.x*Time.fixedDeltaTime)), Screen.height - (inpt.y + (deltaSpeed.y*Time.fixedDeltaTime))));
				#endregion
				rgdBody2D.MovePosition (Vector2.Lerp (transform.position, inpt, mouseSpeed));
			}
			#endregion
		}
		else
		{
			if (mouseInput)
			{
				Dash();
			}
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
		//call the function to control the cursor
		SetCursorState ();

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

		//get the input values for both axes
		horiz = Input.GetAxis ("Horizontal");
		verti = Input.GetAxis ("Vertical");

		//get the "raw" input value - whether it was pushed and in which direction
		horizRaw = Input.GetAxisRaw ("Horizontal");
		vertiRaw = Input.GetAxisRaw ("Vertical");

		#region Input Code Attempts
//		mHoriz = Input.GetAxis("Mouse X");
//		mVerti = Input.GetAxis("Mouse Y");

		//inpt = new Vector2 (horiz,verti).normalized;
		#endregion

		//only allow player movement when "Fire1" key is held down
		if (Input.GetAxis("Fire1") != 0)
		{
			mouseInput = true;
		}
		else
		{
			mouseInput = false;
		}

		#region Dash Button
		//do a dash attack with any key in the "Fire 2" axis
		//but only do it once - ignore held down key
//		if (Input.GetAxis("Fire2") != 0)
//		{
//			if (isFire2Pressed == false && doDash == false)
//			{
//				doDash = true;
//				isFire2Pressed = true;
//			}
//		}
//		else
//		{
//			isFire2Pressed = false;
//		}
		#endregion

		//get mouse position in screen space
		mousePosRaw = Input.mousePosition;
		//convert mouse position to world space
		mousePos = Camera.main.ScreenToWorldPoint(mousePosRaw);
		inpt = new Vector2 (mousePos.x, mousePos.y);

		//calculate the distance between mouse and edge of screen to determine when to do a dash attack
//		distance = screenRight;
		if ((mousePosRaw.x >= screenRight || mousePosRaw.y >= screenTop || mousePosRaw.x <= screenLB || mousePosRaw.y <= screenLB) && !coolTimerOn)
		{
			dashStart = true;
			doDash = true;
		}

		#region Velocity DebugLog
		//send velocity values to console
//		Debug.Log ("Velocity: " + rgdBody2D.velocity.x.ToString () + ", " + rgdBody2D.velocity.y.ToString ());
		#endregion

		//send distance value to console
//		Debug.Log ("Distance: " + distance.ToString());

		//quit game when escape is pressed
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit ();
		}

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
	//stop colliding with civs
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Civilian"))
		{
			Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), 
									col.gameObject.GetComponent<Collider2D>());
		}
	}
	#endregion
}
