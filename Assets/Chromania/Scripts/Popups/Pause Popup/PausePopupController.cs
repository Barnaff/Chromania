﻿using UnityEngine;
using System.Collections;

public class PausePopupController : PopupBaseController {


    #region Public Properties

    public void QuitGameButtonAction()
    {
        PauseManager.Instance.ResumeGame();
        FlowManager.Instance.QuitGame();
    }

    public void ResumeButtonAction()
    {
        ClosePopup(); 
    }

    public void RestartButtonAction()
    {
        PauseManager.Instance.ResumeGame();
        FlowManager.Instance.RestartGame();
    }

    #endregion
}
