using UnityEngine;
using System.Collections;

public class ChromiezSelectionController : BaseMenuController {

    #region Private Properties

    #endregion


    #region BaseMenuController Implementation

  

    #endregion


    #region User Interaction

    public void BackButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.ModeSelection);
    }

    public void LetsGoButtonAction()
    {
        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.StartGame();
        }
    }

    #endregion
}
