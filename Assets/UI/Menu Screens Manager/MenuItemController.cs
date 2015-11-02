using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public enum ItemDirection
{
	Top,
	Bottom,
	Left,
	Right,
}

public class MenuItemController : MonoBehaviour {

    [SerializeField]
	public ItemDirection Direction;

    [SerializeField]
    private bool _resetAnchorWhenAnimating;

	private Vector3 _origin = Vector3.zero;

	void Start()
	{
		if (_origin == Vector3.zero)
		{
			_origin = this.gameObject.transform.localPosition;

		}
	}

	public void MoveIn(float time = 0.5f, Action completionAction = null)
	{
        ItemDirection direction = Direction;
		MoveIn(direction, time, completionAction);

        if (_resetAnchorWhenAnimating)
        {
            ResetAnchors();
        }
	}

	public void MoveOut(float time = 0.5f, Action completionAction = null)
	{
		ItemDirection direction = Direction;
		MoveOut(direction, time, completionAction);

        if (_resetAnchorWhenAnimating)
        {
            ResetAnchors();
        }
    }

	public void MoveIn(ItemDirection direction, float time = 0.5f, Action completionAction = null)
	{

		if (_origin != Vector3.zero)
		{
			Vector3 targetPosition = Vector3.zero;
			switch (direction)
			{
			case ItemDirection.Top:
			{
				targetPosition = new Vector3(this.gameObject.transform.localPosition.x, Screen.height , this.gameObject.transform.localPosition.z);
				break;
			}
			case ItemDirection.Bottom:
			{
				targetPosition = new Vector3(this.gameObject.transform.localPosition.x, -Screen.height  , this.gameObject.transform.localPosition.z);
				break;
			}
			case ItemDirection.Left:
			{
				targetPosition = new Vector3(-Screen.width , this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
				break;
			}
			case ItemDirection.Right:
			{
				targetPosition = new Vector3(Screen.width  , this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
				break;
			}
			}
			
			this.gameObject.transform.localPosition = targetPosition;
			iTween.MoveTo(this.gameObject, iTween.Hash("time", time, "x", _origin.x, "y", _origin.y, "islocal", true));

		}


		if (completionAction != null)
		{
			GameUtils.StartDelayedCall(time, "moveAction"+ direction , completionAction);
		}
	}

	public void MoveOut(ItemDirection direction, float time = 0.5f, Action completionAction = null)
	{
		Vector3 targetPosition = Vector3.zero;
		switch (direction)
		{
		case ItemDirection.Top:
		{
			targetPosition = new Vector3(this.gameObject.transform.localPosition.x, Screen.height , this.gameObject.transform.localPosition.z);
			break;
		}
		case ItemDirection.Bottom:
		{
			targetPosition = new Vector3(this.gameObject.transform.localPosition.x, -Screen.height, this.gameObject.transform.localPosition.z);
			break;
		}
		case ItemDirection.Left:
		{
			targetPosition = new Vector3(-Screen.width, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
			break;
		}
		case ItemDirection.Right:
		{
			targetPosition = new Vector3(Screen.width , this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
			break;
		}
		}
		iTween.MoveTo(this.gameObject, iTween.Hash("time", time, "x", targetPosition.x, "y", targetPosition.y, "islocal", true));
		
		if (completionAction != null)
		{
			GameUtils.StartDelayedCall(time, "moveAction"+ direction , completionAction);
		}
	}
	    
    
    private void ResetAnchors()
    {
		//StartCoroutine(ResetAnchoresCorutine());
    }

	IEnumerator ResetAnchoresCorutine()
	{
		yield return new WaitForSeconds(0.2f);

		//this.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
		//this.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
	}
}
