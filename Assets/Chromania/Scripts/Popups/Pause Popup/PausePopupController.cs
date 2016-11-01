using UnityEngine;
using System.Collections;

public class PausePopupController : PopupBaseController {


    #region Public Properties

    public void QuitGameButtonAction()
    {
        FlowManager.Instance.QuitGame();
    }

    public void ResumeButtonAction()
    {
        ClosePopup();
    }

    #endregion
}
