using UnityEngine;
using System.Collections;

public class SlotsTileController : MonoBehaviour {

	[SerializeField]
	public SlotsIconDefenition IconDefenition;


	public void DisplayTileWinningAnimation()
	{
		switch (IconDefenition.IconAnimation)
		{
		case SlotsIconDefenition.IconAnimationType.Wiggle:
		{
			iTween.PunchRotation(this.gameObject, iTween.Hash("time", 2.0f, "z", 30));
			break;
		}
		case SlotsIconDefenition.IconAnimationType.Punch:
		{
			iTween.PunchScale(this.gameObject, iTween.Hash("time", 1.0f, "x", 1.2f, "y", 1.2f));
			break;
		}
		default:
		{
			iTween.PunchScale(this.gameObject, iTween.Hash("time", 1.0f, "x", 1.2f, "y", 1.2f));
			break;
		}
		}
	}
}
