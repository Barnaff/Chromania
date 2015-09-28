using UnityEngine;
using System.Collections;

public class ColorZoneBehavior : MonoBehaviour {

	public Vector3 screenPosition = new Vector3(0,0,10);
	public Camera mainCamera;
	// Use this for initialization
	void Start () {
		transform.position = mainCamera.ScreenToWorldPoint(screenPosition);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
