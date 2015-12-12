using UnityEngine;
using System.Collections;

public class ModeSelectionController : BaseMenuController
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void BackButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.MainMenu);
    }

    public void RushButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.ChromiezSelection);
    }

    public void ClassicButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.ChromiezSelection);
    }
}
