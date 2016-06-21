using UnityEngine;
using System.Collections;

public class GUISelectionBox : MonoBehaviour
{
	// Draggable inspector reference to the Image GameObject's RectTransform.
	public RectTransform selectionBox;

	// This variable will store the location of wherever we first click before dragging.
	private Vector2 initialClickPosition = Vector2.zero;

	public eCivilianController civvie;
	public Vector2 mousePos;
	bool selectPressed;

	void Awake()
	{
		civvie = gameObject.GetComponent<eCivilianController>();
		selectPressed = civvie.isSelectPressed;
	}


	void Update()
	{
		selectPressed = civvie.isSelectPressed;

		// Click somewhere in the Game View.
		if (Input.GetAxis("Select") != 0)
		{
			if (selectPressed == false)
			{
				// Get the initial click position of the mouse. No need to convert to GUI space
				// since we are using the lower left as anchor and pivot.
				initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

				// The anchor is set to the same place.
				selectionBox.anchoredPosition = initialClickPosition;
			}
			// While we are dragging.
			else
			{
				// Store the current mouse position in screen space.
				mousePos = Camera.main.WorldToScreenPoint (civvie.mousePos);

				// How far have we moved the mouse?
				Vector2 difference = mousePos - initialClickPosition;

				// Copy the initial click position to a new variable. Using the original variable will cause
				// the anchor to move around to wherever the current mouse position is,
				// which isn't desirable.
				Vector2 startPoint = initialClickPosition;

				// The following code accounts for dragging in various directions.
				if (difference.x < 0)
				{
					startPoint.x = mousePos.x;
					difference.x = -difference.x;
				}
				if (difference.y < 0)
				{
					startPoint.y = mousePos.y;
					difference.y = -difference.y;
				}

				// Set the anchor, width and height every frame.
				selectionBox.anchoredPosition = startPoint;
				selectionBox.sizeDelta = difference;
			}
		}
		// After we release the mouse button.
		else
		{
			// Reset
			initialClickPosition = Vector2.zero;
			selectionBox.anchoredPosition = Vector2.zero;
			selectionBox.sizeDelta = Vector2.zero;
		}
		
	}
}
