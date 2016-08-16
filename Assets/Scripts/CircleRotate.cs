using UnityEngine;
using System.Collections;

public class CircleRotate : MonoBehaviour {

	public float rotationSpeed = 15f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
	
	}
}
