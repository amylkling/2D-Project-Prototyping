using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CivHealthUI : MonoBehaviour {

	//script variables
	private eCivilianController civScript;
	public CivHealthUIController uiScript;

	//variables for creating GUI
	public Canvas canvas;
	public GameObject healthPrefab;

	//variables for manipulating GUI
	public float healthPanelOffset = 1f;
	public GameObject healthPanel;
	public Slider healthSlider;
	private Renderer selfRenderer;
	private CanvasGroup canvasGroup;
//	public float viewRange = 15f;
	public Color fullHealth;
	public Color twoThirdsHealth;
	public Color almostDead;
	public Image barFill;


	// Use this for initialization
	void Awake () {
		//initialize and instantiate
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		civScript = gameObject.GetComponent<eCivilianController>();
		healthPanel = Instantiate(healthPrefab) as GameObject;
		healthPanel.transform.SetParent(canvas.transform, false);

		healthSlider = healthPanel.GetComponent<Slider>();
		selfRenderer = gameObject.GetComponent<Renderer>();

		//let the Civilian script attached to the same civilian know which health bar belongs to it
		civScript.healthBar = healthSlider;

		barFill = healthPanel.GetComponentInChildren<Image>();

		canvasGroup = healthPanel.GetComponent<CanvasGroup>();

		//this is purely so that this script can tell CivHealthUIController script which civilian it is associated with
		uiScript = healthPanel.GetComponent<CivHealthUIController>();
		uiScript.civScript = civScript;

	}

	// Update is called once per frame
	void Update () {

		//position the health bar above the civilian
		Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

		//track camera distance and make the health bar invisible when the player is far enough away
//		float distance = (worldPos - Camera.main.transform.position).magnitude;
//		float alpha = viewRange - distance / 2.0f;
//		SetAlpha(alpha);

		if (selfRenderer.isVisible)
		{
			healthPanel.SetActive(true);
		}
		else
		{
			healthPanel.SetActive(false);
		}
			


		if (healthSlider.value <= Mathf.CeilToInt(healthSlider.maxValue * (2f/3f)) && 
			healthSlider.value > Mathf.CeilToInt(healthSlider.maxValue * (1f/3f)))
		{
			barFill.color = twoThirdsHealth;
		}
		else if (healthSlider.value <= Mathf.CeilToInt(healthSlider.maxValue * (1f/3f)))
		{
			barFill.color = almostDead;
		}
		else if (healthSlider.value > Mathf.CeilToInt(healthSlider.maxValue * (2f/3f)))
		{
			barFill.color = fullHealth;
		}


	}

	//make health bar invisible
	public void SetAlpha(float alpha)
	{
		canvasGroup.alpha = alpha;

		if (alpha <= 0)
		{
			healthPanel.SetActive(false);
		}
		else
		{
			healthPanel.SetActive(true);
		}
	}
}
