using UnityEngine;
using System.Collections;

public class eMarkerController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Civilian"))
		{
			Destroy(gameObject);
		}
	}
}
