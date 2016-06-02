using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Platformer2DUserControlV2 : MonoBehaviour
{
	private PlatformerCharacterControllerV2 m_Character;
	private bool m_Jump;


	private void Awake ()
	{
		m_Character = GetComponent<PlatformerCharacterControllerV2> ();
	}


	private void Update ()
	{
		if (!m_Jump) {
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
		}
	}


	private void FixedUpdate ()
	{
		// Read the inputs.
		bool crouch = Input.GetKey (KeyCode.LeftControl);
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		// Pass all parameters to the character control script.
		m_Character.Move (h, v, crouch, m_Jump);
		m_Jump = false;
	}
}