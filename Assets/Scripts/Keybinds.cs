using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Keybinds : MonoBehaviour {

	//strike this, it's all totally unnecessary b/c unity handles it during launch
	//and who cares if the player can change the keys
	//it was just going to be a neat little extra feature anyway

	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

	public Text selectText, stopText, destText;

	private GameObject currentKey;

	// Use this for initialization
	void Start () 
	{
		keys.Add("SelectButton", KeyCode.Z);
		keys.Add("StopButton", KeyCode.C);
		keys.Add("DestButton", KeyCode.X);

		selectText.text = keys["SelectButton"].ToString();
		stopText.text = keys["StopButton"].ToString();
		destText.text = keys["DestButton"].ToString();


	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
		if (currentKey != null)
		{
			Event e = Event.current;
			if (e.isKey)
			{
				keys[currentKey.name] = e.keyCode;
				currentKey.GetComponentInChildren<Text>().text = e.keyCode.ToString();
				currentKey = null;
			}
		}
	}

	public void ChangeKey(GameObject clicked)
	{
		currentKey = clicked;
	}
}
