using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FlowManager : FactoryComponent, IFlow
{

    #region FactoryComponent Implementation

    public override void InitComponentAtStart()
    {

    }

    public override void InitComponentAtAwake()
    {

    }

    #endregion


    public void StartGame()
    {

        SceneManager.LoadScene("Gameplay Scene");
    }

    public void FinishGame()
    {
        IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
        if (popupsManager != null)
        {
            popupsManager.DisplayPopup<GameEndedPopupController>();
        }
    }
   
}
