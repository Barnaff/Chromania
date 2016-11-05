using UnityEngine;
using System.Collections;

public class AppDebugPanelController : MonoBehaviour {

    #region Private Properties

    // Use this for initialization
    void Start () {
       
        this.gameObject.SetActive(false);
	}

    #endregion


    #region Public - Buttons Actions

    public void Show()
    {
        Debug.Log("show debug");
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SendFabricCrashButtonAction()
    {
        Fabric.Crashlytics.Crashlytics.Crash();
    }

    public void SendFabricThrowNonFatalButtonAction()
    {
        Fabric.Crashlytics.Crashlytics.ThrowNonFatal();
    }

    #endregion

}
