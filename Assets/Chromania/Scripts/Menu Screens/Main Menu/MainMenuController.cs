﻿using UnityEngine;
using System.Collections;
using System;

public class MainMenuController : BaseMenuController {


	

    #region BaseMenuController Implementation

  

    #endregion


    #region User Interactions

    public void PlayButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.ModeSelection);
    }

    #endregion
}
