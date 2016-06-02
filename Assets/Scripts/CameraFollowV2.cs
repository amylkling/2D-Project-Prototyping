using System;
using UnityEngine;



public class CameraFollowV2 : MonoBehaviour
{
    public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
    public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

    private Transform m_Player; // Reference to the player's transform.

	[Space]
	public float xFastMargin = 2f; //the distance in x axis player can move before the camera moves faster
	public float yFastMargin = 2f; //the distance in x axis player can move before the camera moves faster
	public float xSlowSmooth = 1f; //how slowly the camera will move as it catches up to the player in x axis
	public float ySlowSmooth = 1f; //how slowly the camera will move as it catches up to the player in x axis
	private bool xClose = false;
	private bool yClose = true;

	private eHeroController playerScript;
	Vector2 playerVelocity;
	float playerMSpeed = 0f;


    private void Awake()
    {
        // Setting up the reference.
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
		playerScript = m_Player.gameObject.GetComponent<eHeroController>();
		//playerVelocity = playerScript.playerVelocity;
		playerMSpeed= playerScript.mouseSpeed;

    }


    private bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - m_Player.position.x) > xMargin && Mathf.Abs(transform.position.x - m_Player.position.x) <= xFastMargin;
    }

	private bool CheckXFastMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x fast margin.
		return Mathf.Abs(transform.position.x - m_Player.position.x) > xFastMargin;
	}


    private bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin && Mathf.Abs(transform.position.y - m_Player.position.y) <= yFastMargin;
    }

	private bool CheckYFastMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y fast margin.
		return Mathf.Abs(transform.position.y - m_Player.position.y) > yFastMargin;
	}

	private void UpdatePlayerVelocity()
	{
		//playerVelocity = playerScript.playerVelocity;
		playerMSpeed = playerScript.mouseSpeed;
	}


    private void FixedUpdate()
    {
		//UpdatePlayerVelocity();
        TrackPlayer();
    }


    private void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

//		Debug.Log("X pos: " + Mathf.Abs(transform.position.x - m_Player.position.x).ToString());
//		Debug.Log("Y pos: " + Mathf.Abs(transform.position.y - m_Player.position.y).ToString());

        // If the player has moved beyond the x margin...
		if (CheckXMargin())
        {
			// ... the target x coordinate should be a slow Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp (transform.position.x, m_Player.position.x, xSlowSmooth * Time.fixedDeltaTime);

			Debug.Log ("X Slow Lerp");
        }

		if (CheckXFastMargin())
		{
			// ... the target x coordinate should be a faster Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth * Time.fixedDeltaTime);

			Debug.Log("X Lerp");

		}

        // If the player has moved beyond the y margin...
		if (CheckYMargin())
        {
            // ... the target y coordinate should be a slow Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySlowSmooth*Time.fixedDeltaTime);

			Debug.Log("Y Slow Lerp");
        }

		if (CheckYFastMargin())
		{
			// ... the target y coordinate should be a faster Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth * Time.fixedDeltaTime);

			Debug.Log("Y Lerp");
		}

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}