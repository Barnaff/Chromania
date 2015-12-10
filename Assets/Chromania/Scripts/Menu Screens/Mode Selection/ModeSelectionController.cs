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

    public override void OnEnterAnimationComplete()
    {
        base.OnEnterAnimationComplete();
    }

    public override void OnExitAnimationComplete()
    {
        base.OnExitAnimationComplete();
    }


    public void BackButtonAction()
    {
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.MainMenu);
    }

    public void RushButtonAction()
    {

    }

    public void ClassicButtonAction()
    {

    }
}
