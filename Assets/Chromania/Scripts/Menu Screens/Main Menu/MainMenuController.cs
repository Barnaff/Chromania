using UnityEngine;
using System.Collections;
using System;

public class MainMenuController : BaseMenuController {


	

    #region BaseMenuController Implementation

    public override void OnEnterAnimationComplete ()
	{
		Debug.Log("Enter animation complete");
	}

    public override void OnExitAnimationComplete()
    {
        throw new NotImplementedException();
    }

    #endregion


    #region User Interactions

    public void PlayButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.ModeSelection);
    }

    #endregion
}
