using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupsManager : Kobapps.Singleton<PopupsManager> {

    #region private properties

    [SerializeField]
    private GameObject _overlayController;

    [SerializeField]
    private List<PopupBaseController> _activePopups;

    #endregion


    #region Public

    public T DisplayPopup<T>(System.Action closePopupAction = null) where T : PopupBaseController
    {
        PopupBaseController popupControllerPrefab = GetPopupPrefab<T>();
        if (popupControllerPrefab != null)
        {
            PopupBaseController popupController = Instantiate(popupControllerPrefab) as PopupBaseController;
            popupController.SetCloseAction(closePopupAction);
            if (_activePopups == null)
            {
                _activePopups = new List<PopupBaseController>();
            }
            _activePopups.Add(popupController);

            if (popupController.DontDestroyOnLoad)
            {
                DontDestroyOnLoad(popupController.gameObject);
            }

            return popupController as T;
        }
        return null;
    }

    public void ClosePopup<T>() where T : PopupBaseController
    {
        foreach (PausePopupController popupController in _activePopups)
        {
            if (popupController.GetType() == typeof(T))
            {
                ClosePopup(popupController);
            }
        }
    }

    public void ClosePopup(PopupBaseController popupController)
    {
        popupController.ClosePopup();
    }

    public void CloseAllPopups()
    {
        foreach (PopupBaseController popupController in _activePopups)
        {
            ClosePopup(popupController);
        }
    }

    #endregion


    #region Private

    public PopupBaseController GetPopupPrefab<T>() where T : PopupBaseController
    {
        T[] popupsPrefabs = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
        if (popupsPrefabs != null && popupsPrefabs.Length > 0)
        {
            return popupsPrefabs[0] as PopupBaseController;
        }

        T popupPrefab = Resources.Load<T>(typeof(T).ToString());
        if (popupPrefab != null)
        {
            return popupPrefab;
        }

        Debug.LogError("Could not load popup: " + typeof(T).ToString());
        return null;
    }

    #endregion
}


public class PopupBaseController : MonoBehaviour
{
    #region Private Properties

    [SerializeField]
    private bool _displyOverlay;

    [SerializeField]
    private bool _isSingle;

    [SerializeField]
    private bool _dontDestoryOnLoad;

    [SerializeField]
    private bool _closeOnBackButton;

    private System.Action _closePopupAction;

    #endregion


    #region Public

    public bool DontDestroyOnLoad
    {
        get
        {
            return _dontDestoryOnLoad;
        }
    }

    public void ClosePopup()
    {
        Debug.Log("close popup");
        if (_closePopupAction != null)
        {
            _closePopupAction();
        }
        Destroy(this.gameObject);
    }

    public void SetCloseAction(System.Action closePopupAction)
    {
        Debug.Log("set popup close action: " + closePopupAction);
        _closePopupAction = closePopupAction;
    }

    #endregion
}
