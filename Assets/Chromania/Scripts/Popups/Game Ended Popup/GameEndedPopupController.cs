﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEndedPopupController : PopupBaseController {

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    #endregion


    #region User Interaction

    public void PlayAgainButtonAction()
    {
        ClosePopup(() =>
        {
            IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
            if (flowManager != null)
            {
                flowManager.StartGame();
            }

        });
        
    }

    public void MenuButtonAction()
    {
        ClosePopup(() =>
        {
            IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
            if (flowManager != null)
            {
                flowManager.MainMenu();
            }
        });
    }

    #endregion
}
