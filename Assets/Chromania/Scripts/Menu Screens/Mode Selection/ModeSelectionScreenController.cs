using UnityEngine;
using System.Collections;

public class ModeSelectionScreenController : MenuScreenBaseController {

    #region User Interactions

    public void SelectClassicModeButtonAction()
    {
        FlowManager.Instance.SelectPlayMode(eGameplayMode.Classic);
    }

    public void SelectRushModeButtonAction()
    {
        FlowManager.Instance.SelectPlayMode(eGameplayMode.Rush);
    }

    public void BackButtonAction()
    {

    }

    #endregion
}
