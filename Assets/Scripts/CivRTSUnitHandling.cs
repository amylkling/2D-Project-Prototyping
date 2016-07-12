using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CivRTSUnitHandling : MonoBehaviour {

	public List<GameObject> selectedCivs;		//stores all selected civs as determined by eCivilianController
	public List<GameObject> sortedCivs;			//stores selected civs after sorting by distance to destination
	public Vector2 destination;					//the destination of the civs, based on the player's input
	public float distToGround = .5f;			//how far off the ground to place the marker
	public float civPosOffset = 3f;				//how far from each other the civs should be when they stop

	public eHeroController player;				//reference the player's script
	public Vector3 mousePos;					//holder for mouse input from player's script
	private Vector2 mousePos2D;					//holder for the conversion of the mouse position to 2D
	private GameObject[] mars;					//holder for an array of markers in the scene
	public GameObject marker;					//reference the marker prefab
	public Vector3 markerPos;					//holder for the marker's spawn position
	public bool isDeployed = false;				//whether or not a marker exists

	void Start()
	{
		//initiate player script reference and variable from it
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<eHeroController>();
		mousePos = player.mousePos;
		mousePos2D = new Vector2(mousePos.x, mousePos.y);

		//instantiate the list of selected civs - not necessary, but probably a good idea
		selectedCivs = new List<GameObject>();

	}


	// Update is called once per frame
	void Update () 
	{

		//keep the mouse input updated
		mousePos = player.mousePos;
		mousePos2D = new Vector2(mousePos.x, mousePos.y);

		//keep track of how many markers are in the scene
		mars = GameObject.FindGameObjectsWithTag("Marker");

		#region Marker Controls
		//controls for civilian movement
		if (Input.GetButtonDown("Deploy"))
		{
			//create a marker at the mouse position
			//sort the selected civs from closest to farthest
			//send the closest civ to the marker
			//send the rest to a point marker + (number of civs in front of them) radius away

			//ensure that this is only done if there are civs selected
			if(selectedCivs.Count() != 0)
			{
				//don't do anything else unless a marker can be placed
				if(CreateMarker())
				{
					SortCivs();
					SetDestinations();
				}
			}
				
		}
		#endregion

	}

	bool CreateMarker()
	{
		//>>create a marker at mouse position
		//>only make a marker if a valid position for it can be found
		//1. find the appropriate positioning
		//   ->the x value is the mouse position's
		//   ->the y value needs to be a set height above the nearest ground
		//       ->use raycast to determine where the ground is below the mouse position
		//2. create the marker at that position
		//   ->if one already exists, replace it

		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.down);

		if (hit)
		{
			if (hit.transform.gameObject.CompareTag("Ground"))
			{
				markerPos = new Vector3(mousePos2D.x, hit.transform.position.y + distToGround, 0); 
				destination = new Vector2(markerPos.x, markerPos.y);

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

				return true;
			}
			else
			{
				Debug.Log("can't place marker here");
				return false;
			}
		}
		else
		{
			return false;
		}

	}

	void SortCivs()
	{
		//>>sort the selected civs from closest to farthest
		//1. access each civ
		//   ->use a foreach loop on the selectedCivs list
		//2. determine how far each civ is from the destination
		//   ->compare civ's x position to destination x
		//   ->store that in a variable for that civ
		//3. compare the civs to each other
		//   ->remember when you learned how to do this in cs?
		//   ->yeah go look that up again because of course you forgot how to do it
		//4. add them to the sortedCivs list in order
		//   ->closest to destination goes in first slot
		//
		//^^or, you know, just let the list sort itself you idiot

		//example sorting function code
		//hits = hits.OrderBy(x => Vector2.Distance(this.transform.position,x.transform.position)).ToList();

		sortedCivs = selectedCivs.OrderBy(x => Vector2.Distance(
			new Vector2(x.transform.position.x, x.transform.position.y), destination)).ToList();

		Debug.Log("civs sorted!");

	}


	void SetDestinations()
	{
		//>>send the closest civ to the marker
		//1. access the closest civ
		//   ->should be in the first slot of the sortedCivs list
		//2. set its destination variable to the marker's position
		//   ->need to access that civ's controller script
		//   ->only set the x though, leave y as current y


		//>>send the rest to a point marker + (number of civs in front of them) radius away
		//1. access the rest of the civs in order
		//   ->simple foreach in sortedCivs should do it
		//2. determine how many are in front of that civ
		//   ->can just use the index of the civ in the list
		//3. set the civ's destination
		//   ->access that civ's controller script
		//   ->multiply the number of civs to an individual civ's radius (arbitrary offset number)
		//   ->add that to the marker's x position
		//   ->only set x destination, leave y as the civ's current y
		//>problem: only works in negative direction
		//   ->can't add when going in positive direction, need to subtract
		//   ->RESOLVED
		//>problem: only works if destination is outside of the group of civs
		//   ->when a destination is placed between civs, they act strangely


		sortedCivs[0].GetComponent<eCivilianController>().destPos.x = destination.x;
		sortedCivs[0].GetComponent<eCivilianController>().stop = false;


		foreach(GameObject civ in sortedCivs)
		{
			if (destination.x < civ.GetComponent<Rigidbody2D>().position.x)
			{
				civ.GetComponent<eCivilianController>().destPos.x = destination.x + 
					((float)sortedCivs.IndexOf(civ) * civPosOffset);
			}
			else if (destination.x > civ.GetComponent<Rigidbody2D>().position.x)
			{
				civ.GetComponent<eCivilianController>().destPos.x = destination.x - 
					((float)sortedCivs.IndexOf(civ) * civPosOffset);
			}

			civ.GetComponent<eCivilianController>().stop = false;

		}

	}


}
