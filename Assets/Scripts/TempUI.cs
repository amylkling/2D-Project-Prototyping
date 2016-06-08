using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TempUI : MonoBehaviour {

	public eHeroController player;
	public Text normalSpeed;
	public Text dashSpeed;
	public float mouseSpeed;
	public float chaSpeed;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<eHeroController>();
		mouseSpeed = player.mouseSpeed;
		chaSpeed = player.dashSpeed;
		normalSpeed.text = "Normal Speed " + mouseSpeed.ToString("F");
		dashSpeed.text = "Dash Speed " + chaSpeed.ToString("F");
	}
	
	// Update is called once per frame
	void Update () {
		mouseSpeed = player.mouseSpeed;
		chaSpeed = player.dashSpeed;

		if (Input.GetKeyDown(KeyCode.I))
		{
			mouseSpeed += .01f;
			player.mouseSpeed = mouseSpeed;
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			mouseSpeed -= .01f;
			player.mouseSpeed = mouseSpeed;
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			chaSpeed += .1f;
			player.dashSpeed = chaSpeed;
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			chaSpeed -= .1f;
			player.dashSpeed = chaSpeed;
		}
			
		normalSpeed.text = "Normal Speed " + mouseSpeed.ToString("F");
		dashSpeed.text = "Dash Speed " + chaSpeed.ToString("F");
	}
}
