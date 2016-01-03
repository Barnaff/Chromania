using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {


	public delegate void OnTouchCreatedDelegate(GameObject touchController);
	
	#region Public
	
	/// <summary>
	/// The touch prefab.
	/// </summary>
	public GameObject TouchPrefab;
	
	/// <summary>
	/// Calls when a touch is created.
	/// </summary>
	public OnTouchCreatedDelegate OnTouchCreated;
	
	#endregion
	
	
	#region Private
	
	/// <summary>
	/// Table of all active touches.
	/// </summary>
	private Hashtable _touches;
	
	#endregion
	
	
	#region Initialize
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		_touches = new Hashtable();
	}
	
	#endregion
	
	#region Update
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		
		foreach (Touch touch in Input.touches)
		{
			switch (touch.phase)
			{
			case TouchPhase.Began:
			{
				CreateTouch(touch);
				break;
			}
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
			{
				MoveTouch(touch);
				break;
			}
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
			{
				RemoveTouch(touch);
				break;
			}
				
			}
		}
		
		#if UNITY_EDITOR
		/*
		 * For running on Editor, 
		 * this simulates the mouse as touches.
		 */
		
		if (_touches.ContainsKey(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;
			GameObject touchController = _touches[0] as GameObject;
			if (touchController != null)
			{
				touchController.transform.position = mousePosition;
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!_touches.Contains(0))
				{
					Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					mousePosition.z = 0;
					GameObject touchController = Lean.LeanPool.Spawn(TouchPrefab, mousePosition, Quaternion.identity) as GameObject;
                    touchController.GetComponent<TouchController>().Init();
                    _touches.Add(0, touchController);
					if (OnTouchCreated != null)
					{
						OnTouchCreated(touchController);
					}
				}
			}
		}
		
		if (Input.GetMouseButtonUp(0))
		{
			if (_touches.ContainsKey(0))
			{
				GameObject touchController = _touches[0] as GameObject;
				if (touchController != null)
				{
                    touchController.GetComponent<TouchController>().OnDespwan();
                    Lean.LeanPool.Despawn(touchController);
				}
				_touches.Remove(0);
			}
		}
		#endif
	}
	
	#endregion
	
	
	#region Private
	
	private void CreateTouch(Touch touch)
	{
		Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.0f));
		position.z = 0.0f;
		GameObject touchController = Lean.LeanPool.Spawn(TouchPrefab, position, Quaternion.identity) as GameObject;
        touchController.GetComponent<TouchController>().Init();
        if (_touches.Contains (touch.fingerId)) 
		{
			RemoveTouch(touch);
		}
		_touches.Add(touch.fingerId, touchController);
		
		if (OnTouchCreated != null)
		{
			OnTouchCreated(touchController);
		}
	}
	
	private void MoveTouch(Touch touch)
	{
		if (!_touches.ContainsKey(touch.fingerId))
		{
			CreateTouch(touch);
		}
		GameObject touchController = _touches[touch.fingerId] as GameObject;
		if (touchController != null)
		{
			Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0.0f));
			position.z = 0.0f;
			touchController.transform.position = position;
		}
		else
		{
			CreateTouch(touch);
		}
	}
	
	private void RemoveTouch(Touch touch)
	{
		if (_touches.ContainsKey(touch.fingerId))
		{
			GameObject touchController = _touches[touch.fingerId] as GameObject;
			_touches.Remove(touch.fingerId);
            touchController.GetComponent<TouchController>().OnDespwan();
            Lean.LeanPool.Despawn(touchController);
        }
	}
	
	#endregion

}
