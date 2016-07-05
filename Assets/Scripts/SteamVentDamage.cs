using UnityEngine;
using System.Collections;

public class SteamVentDamage : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Civilian")
		{
			col.gameObject.GetComponent<eCivilianController>().TakeDmg(34);
		}
	}
}
