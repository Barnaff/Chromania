using UnityEngine;
using System.Collections;

public class NotEnoughHeartsPopupController : PopupBaseController {

	public System.Action BuyMoreAction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BuyMoreButtonAction()
	{
		//ClosePopup();
		if (BuyMoreAction != null)
		{
			BuyMoreAction();
		}
	}

}
