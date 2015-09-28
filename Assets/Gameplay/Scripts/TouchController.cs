using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	private bool _isDragging;
	private Vector3 _lastPosition;
	private GameObject _draggedObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (_draggedObject != null)
		{
			_draggedObject.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(_draggedObject.GetComponent<Rigidbody2D>().position, this.gameObject.GetComponent<Rigidbody2D>().position, Time.deltaTime * 10.0f));
		}
	}

	void LateUpdate()
	{
		_lastPosition = this.gameObject.transform.position;
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (!_isDragging && other.gameObject.tag == "Chromie")
		{
			ChromieController chromieController = other.gameObject.GetComponent<ChromieController>() as ChromieController;
			if (chromieController != null && !chromieController.IsGrabbed)
			{
				_draggedObject = other.gameObject;
				_isDragging = true;
				chromieController.GrabChromie();
			}
		}
	}

	void OnDestroy()
	{
		if (_isDragging && _draggedObject != null)
		{
			float distance = Vector3.Distance(_lastPosition, this.gameObject.transform.position);
			float distance2 = Vector3.Distance(this.gameObject.transform.position, _draggedObject.transform.position);
			Vector2 forceVector = Vector2.zero;
			if (distance > distance2)
			{
				forceVector = this.gameObject.transform.position - _lastPosition;
			}
			else
			{
				forceVector = this.gameObject.transform.position - _draggedObject.transform.position;
			}

			this.gameObject.GetComponent<SpringJoint2D>().enabled = false;
			forceVector *= distance2 * 4.0f;
			_draggedObject.GetComponent<Rigidbody2D>().velocity = forceVector;

			ChromieController chromieController = _draggedObject.gameObject.GetComponent<ChromieController>() as ChromieController;
			if (chromieController != null)
			{
				chromieController.ReleaseChromie();
			}
		}
	}
}
