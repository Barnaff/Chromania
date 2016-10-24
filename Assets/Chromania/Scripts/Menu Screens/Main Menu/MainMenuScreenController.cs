using UnityEngine;
using System.Collections;

public class MainMenuScreenController : MenuScreenBaseController {


    #region user Interactions

    public void PlayButtonAction()
    {
        FlowManager.Instance.MainMenuPlay();
    }

    public void SettingsButtonAction()
    {

    }

    #endregion
}
