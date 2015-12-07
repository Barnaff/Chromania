using UnityEngine;
using System.Collections;

public delegate void BubbleDepthSwapButtonPressedDelegate();

public class BubbleDepthSwapButtonController : MonoBehaviour {

	#region Public Properties

	public BubbleDepthSwapButtonPressedDelegate OnSwapButtonPressed;

	#endregion


	#region Public

	public void DisplayClickAnimation()
	{
		iTween.PunchScale(this.gameObject, iTween.Hash("time",  0.4f, "x", 0.9f, "y", 0.9f));
		//iTween.RotateBy(this.gameObject, iTween.Hash("time", 0.5f, "z", Mathf.Deg2Rad * 180.0f ));

	}

	#endregion


	#region User Interaction

	void OnMouseDown()
	{
		DisplayClickAnimation();
		if (OnSwapButtonPressed != null)
		{
			OnSwapButtonPressed();
		}
	}

	#endregion
}
