﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AppDebugPanelController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _currencyLabel;

    #endregion


    #region Initialization

    // Use this for initialization
    void Start()
    {

        this.gameObject.SetActive(false);
    }

    #endregion

    #region Update

    void Update()
    {
        if (_currencyLabel != null)
        {
            _currencyLabel.text = InventoryManager.Instance.Currency.ToString() + "C";
        }
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



    #endregion


    #region Crashalytics debug

    public void SendFabricCrashButtonAction()
    {
        Fabric.Crashlytics.Crashlytics.Crash();
    }

    public void SendFabricThrowNonFatalButtonAction()
    {
        Fabric.Crashlytics.Crashlytics.ThrowNonFatal();
    }

    #endregion

    public void PayCurrencyButtonAction(int amount)
    {
        ShopManager.Instance.Pay(amount, (sucsess) =>
        {
            Debug.Log("payment sucsess: " + sucsess);
        });
    }

    public void AddCurrencyButtonAction(int amount)
    {
        InventoryManager.Instance.Currency += amount;

        Debug.Log("updated amount: " + InventoryManager.Instance.Currency);
    }

    #region Shop Debug


    #endregion

}
