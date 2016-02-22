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
        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.DisplayMenuScreen(eMenuScreenType.MainMenu);
        }
    }

    public void RushButtonAction()
    {
        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.DisplayMenuScreen(eMenuScreenType.ChromiezSelection);
        }
    }

    public void ClassicButtonAction()
    {
        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.DisplayMenuScreen(eMenuScreenType.ChromiezSelection);
        }
    }
}
