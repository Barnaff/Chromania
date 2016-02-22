using UnityEngine;
using System.Collections;
using System;

public class MainMenuController : BaseMenuController {


	

    #region BaseMenuController Implementation

  

    #endregion


    #region User Interactions

    public void PlayButtonAction()
    {
        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.DisplayMenuScreen(eMenuScreenType.ModeSelection);
        }
        
    }

    #endregion
}
