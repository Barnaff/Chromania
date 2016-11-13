using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

    #region Private Properties

    private bool _isDragging;
	private Vector3 _lastPosition;
	private GameObject _draggedObject;

    #endregion


    #region Initialize

    void Start()
    {
        GameplayEventsDispatcher.Instance.OnChromieSpawned += OnChromieDespwanedHandler;
    }

    public void Init()
    {
        _isDragging = false;
        _draggedObject = null;
        _lastPosition = this.gameObject.transform.position;
    }

    #endregion

    #region Update

    // Update is called once per frame
    void Update ()
    {
		if (_draggedObject != null)
		{
			_draggedObject.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(_draggedObject.GetComponent<Rigidbody2D>().position, this.gameObject.GetComponent<Rigidbody2D>().position, Time.deltaTime * 20.0f));
		}
	}

	void LateUpdate()
	{
		_lastPosition = this.gameObject.transform.position;
	}

    #endregion


    #region Events

    void OnTriggerEnter2D(Collider2D other)
    {
		if (!_isDragging)
		{
            IDraggable draggable = other.gameObject.GetComponent<IDraggable>();
			if (draggable != null && !draggable.IsGrabbed)
			{
				_draggedObject = other.gameObject;
				_isDragging = true;
                draggable.BeginDrag();
			}
		}
	}

    public void OnDespwan()
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
			forceVector *= distance2 * 10.0f;
			_draggedObject.GetComponent<Rigidbody2D>().velocity = forceVector;

            IDraggable draggable = _draggedObject.gameObject.GetComponent<IDraggable>();
			if (draggable != null)
			{
                draggable.EndDrag();
			}
		}
	}


    private void OnChromieDespwanedHandler(ChromieController chromieController)
    {
        if (chromieController.gameObject == _draggedObject)
        {
            IDraggable draggable = _draggedObject.gameObject.GetComponent<IDraggable>();
            if (draggable != null)
            {
                draggable.EndDrag();
            }
            _isDragging = false;
            _draggedObject = null;
        }
    }

    #endregion
}
