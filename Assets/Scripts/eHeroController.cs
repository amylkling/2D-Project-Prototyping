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
	public Vector3 mousePos;								//holder for mouse position
	public float mouseSpeed = .1f;							//mouse speed multiplier

	CursorLockMode desiredState;							//for cursor control

	public Vector2 playerVelocity;
	public bool doCharge = false;							//whether or not charge attack has been initiated
	public float chargeSpeed = 10f;
	public float chargeThreshold = 15f;						//distance between mouse and player for a charge attack
	private float distance = 0f;							//holder for calculated distance between mouse and player

	private bool isFire1Pressed = false;					//check that any "Fire1" key isn't held down
//	private bool isFire2Pressed = false;					//check that any "Fire2" key isn't held down
	#endregion

	#region Awake Function
	private void Awake ()
	{
		//assign Rigidbody2D component
		rgdBody2D = GetComponent<Rigidbody2D> ();
		//set CursorLockMode to Confined, so the mouse will stay in the game window
		desiredState = CursorLockMode.Confined;
		playerVelocity = rgdBody2D.velocity;
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

		//move the character according to player input, unless doing a charge attack
		//doing a charge attack takes control away from the player
		if (!doCharge)
		{
			#region Keyboard Movement
			//move the character based on player's keyboard input and a speed multiplier
			if (!mouseInput)
			{
				rgdBody2D.velocity = new Vector2 (horiz * maxHorzSpeed, verti * maxVertSpeed);
				if (horizRaw != 0 && vertiRaw != 0)
				{
					//diagonal movement with a different speed multiplier
					rgdBody2D.velocity = new Vector2 (horiz * maxDiagSpeed, verti * maxDiagSpeed);
				}
			}
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
			Charge();
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

		//switch between keyboard and mouse input with any key in the "Fire 1" axis
		//but only do it once - ignore held down key
		if (Input.GetAxis("Fire1") != 0)
		{
			if (isFire1Pressed == false)
			{
				mouseInput = !mouseInput;
				isFire1Pressed = true;
			}
		}
		else
		{
			isFire1Pressed = false;
		}

		#region Charge Button
		//do a charge attack with any key in the "Fire 2" axis
		//but only do it once - ignore held down key
//		if (Input.GetAxis("Fire2") != 0)
//		{
//			if (isFire2Pressed == false && doCharge == false)
//			{
//				doCharge = true;
//				isFire2Pressed = true;
//			}
//		}
//		else
//		{
//			isFire2Pressed = false;
//		}
		#endregion

		//get mouse position and keep it within the screen
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		inpt = new Vector2 (mousePos.x, mousePos.y);

		//calculate the distance between player and mouse to determine when to do a charge attack
		distance = Vector3.Distance(rgdBody2D.position,mousePos);
		if (distance >= chargeThreshold)
		{
			doCharge = true;
		}
		else
		{
			doCharge = false;
		}

		#region Velocity DebugLog
		//send velocity values to console
//		Debug.Log ("Velocity: " + rgdBody2D.velocity.x.ToString () + ", " + rgdBody2D.velocity.y.ToString ());
		#endregion

		//send distance value to console
		Debug.Log ("Distance: " + distance.ToString());

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

	#region Charge Function
	void Charge()
	{
		rgdBody2D.MovePosition(Vector2.Lerp(rgdBody2D.position, inpt, chargeSpeed * Time.deltaTime));
		Debug.Log("CHARGE!!");
	}
	#endregion
}
