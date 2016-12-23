using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyDisplayController : MonoBehaviour {

    #region Privatre Properties

    [SerializeField]
    private Text _currencyLabel;

    #endregion


    #region Initialization

    void Start()
    {
        InventoryManager.Instance.OnInventoryUpdated += OnInventoryUpdatedHandler;
        UpdateCurrencyLabel();
       
    }

    void OnDestory()
    {
        InventoryManager.Instance.OnInventoryUpdated -= OnInventoryUpdatedHandler;
    }

    #endregion


    #region Private 

    private void UpdateCurrencyLabel()
    {
        if (_currencyLabel != null)
        {
            _currencyLabel.text = InventoryManager.Instance.Currency.ToString();
        }
    }

    #endregion

    #region Public

    public void OpenShopButtonAction()
    {
        PopupsManager.Instance.DisplayPopup<CurrencyShopPopupController>();
    }

    #endregion


    #region Events

    private void OnInventoryUpdatedHandler()
    {
        UpdateCurrencyLabel();
    }

    #endregion
}
