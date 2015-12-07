using UnityEngine;
using System.Collections;

public class FrozenPlayerController : MonoBehaviour {

	public float MoveSpeed = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	
	void FixedUpdate () {
		// Left key pressed?
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(new Vector2(-MoveSpeed, 0));
		}
		
		// Right key pressed?
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(new Vector2(MoveSpeed, 0));
		}

		if (Input.touchCount > 0) {
			// The screen has been touched so store the touch
			Touch touch = Input.GetTouch(0);
			
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
				// If the finger is on the screen, move the object smoothly to the touch position
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));     
				touchPosition.y = this.gameObject.transform.position.y;
				this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, touchPosition, Time.deltaTime * MoveSpeed);
			}
		}
		
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));    
			mousePosition.y = this.gameObject.transform.position.y;
			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, mousePosition, Time.deltaTime * MoveSpeed );
		}
	}
}
