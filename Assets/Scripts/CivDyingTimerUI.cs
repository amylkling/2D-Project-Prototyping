using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CivDyingTimerUI : MonoBehaviour {

	//script variables
	private eCivilianController civScript;
	public CivDyingTimerUIController uiScript;

	//variables for creating GUI
	public Canvas canvas;
	public GameObject dyingPrefab;

	//variables for manipulating GUI
	public float dyingPanelOffsetY = 1f;
	public float dyingPanelOffsetX = 1f;
	private Renderer selfRenderer;
	private CanvasGroup canvasGroup;
	//	public float viewRange = 15f;
	public GameObject dyingPanel;
	public Slider dyingSlider;
	private float dyingTimer;


	// Use this for initialization
	void Awake () {
		//initialize and instantiate
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		civScript = gameObject.GetComponent<eCivilianController>();
		dyingPanel = Instantiate(dyingPrefab) as GameObject;
		dyingPanel.transform.SetParent(canvas.transform, false);

		dyingSlider = dyingPanel.GetComponent<Slider>();
		selfRenderer = gameObject.GetComponent<Renderer>();

		//let the Civilian script attached to the same civilian know which health bar belongs to it
		civScript.dyingTimerUI = dyingSlider;

		canvasGroup = dyingPanel.GetComponent<CanvasGroup>();

		//this is purely so that this script can tell CivDyingTimerUIController script which civilian it is associated with
		uiScript = dyingPanel.GetComponent<CivDyingTimerUIController>();
		uiScript.civScript = civScript;

		dyingPanel.SetActive(false);

	}

	// Update is called once per frame
	void Update () {

		//position the health bar above the civilian
		Vector3 worldPos = new Vector3(transform.position.x + dyingPanelOffsetX, 
				transform.position.y + dyingPanelOffsetY, transform.position.z);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		dyingPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

		//track camera distance and make the health bar invisible when the player is far enough away
		//		float distance = (worldPos - Camera.main.transform.position).magnitude;
		//		float alpha = viewRange - distance / 2.0f;
		//		SetAlpha(alpha);

		if (selfRenderer.isVisible && civScript.Dying)
		{
			dyingPanel.SetActive(true);
		}
		else
		{
			dyingPanel.SetActive(false);
		}

	}

	//make health bar invisible
	public void SetAlpha(float alpha)
	{
		canvasGroup.alpha = alpha;

		if (alpha <= 0)
		{
			dyingPanel.SetActive(false);
		}
		else
		{
			dyingPanel.SetActive(true);
		}
	}
}
