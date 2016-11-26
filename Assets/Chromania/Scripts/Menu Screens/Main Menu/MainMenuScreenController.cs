using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScreenController : MenuScreenBaseController {

    #region Private Properties

    [SerializeField]
    private Text _versionLabel;

    #endregion


    #region Initialization

    void Start()
    {
        _versionLabel.text = GeneratedConstants.PlayerSettings.BundleVersion;
    }

    #endregion


    #region user Interactions

    public void PlayButtonAction()
    {
        FlowManager.Instance.MainMenuPlay();
    }

    public void SettingsButtonAction()
    {

    }

    public void ShopButtonAction()
    {
        FlowManager.Instance.Shop();
    }

    #endregion
}
