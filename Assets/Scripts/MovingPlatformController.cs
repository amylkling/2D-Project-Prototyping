using UnityEngine;
using System.Collections;

public class MovingPlatformController : MonoBehaviour {

	public bool circlePath = false;
	public bool horizontalPath = false;
	public bool verticalPath = false;
	public float movementSpeed = 1f;
	private bool goBack = false;

	private bool setStart;


	[SerializeField]
	private Vector3 horizPathMarker1;
	[SerializeField]
	private Vector3 horizPathMarker2;

	[SerializeField]
	private Vector3 vertiPathMarker1;
	[SerializeField]
	private Vector3 vertiPathMarker2;

	void Start()
	{
		horizPathMarker1 = transform.GetChild(0).TransformPoint(transform.GetChild(0).gameObject.GetComponent<iTweenPath>().nodes[0]);
		horizPathMarker2 = transform.GetChild(0).TransformPoint(transform.GetChild(0).gameObject.GetComponent<iTweenPath>().nodes[1]);

		vertiPathMarker1 = transform.GetChild(1).TransformPoint(transform.GetChild(1).gameObject.GetComponent<iTweenPath>().nodes[0]);
		vertiPathMarker2 = transform.GetChild(1).TransformPoint(transform.GetChild(1).gameObject.GetComponent<iTweenPath>().nodes[1]);

		if (horizontalPath)
		{
			transform.position = horizPathMarker1;
		}
		else if (verticalPath)
		{
			transform.position = vertiPathMarker1;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(circlePath)
		{
			transform.parent.GetChild(0).gameObject.SetActive(true);
			transform.position = transform.parent.GetChild(0).GetChild(0).transform.position;
		}
		else if (horizontalPath)
		{
			transform.parent.GetChild(0).gameObject.SetActive(false);

			if (goBack)
			{
				transform.position = Vector3.Lerp(transform.position, horizPathMarker1, Time.deltaTime * movementSpeed);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, horizPathMarker2, Time.deltaTime * movementSpeed);

				if (transform.position.x == horizPathMarker2.x - .007f)
				{
					goBack = true;
				}
			}

		}
		else if (verticalPath)
		{
			transform.parent.GetChild(0).gameObject.SetActive(false);

			if (goBack)
			{
				transform.position = Vector3.Lerp(transform.position, vertiPathMarker1, Time.deltaTime * movementSpeed);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, vertiPathMarker2, Time.deltaTime * movementSpeed);
			}
		}
		else if (horizontalPath && verticalPath && circlePath)
		{
			Debug.Log("Please choose only one option");
		}
		else if (horizontalPath && verticalPath)
		{
			Debug.Log("Please choose only one option");
		}
		else if (horizontalPath && circlePath)
		{
			Debug.Log("Please choose only one option");
		}
		else if (verticalPath && circlePath)
		{
			Debug.Log("Please choose only one option");
		}
		else
		{
			transform.parent.GetChild(0).gameObject.SetActive(false);
		}


	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Civilian"))
		{
			col.transform.SetParent(transform);
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Civilian"))
		{
			col.transform.parent = null;
		}
	}
}
