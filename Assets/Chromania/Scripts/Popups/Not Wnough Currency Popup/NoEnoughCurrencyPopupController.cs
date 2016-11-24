using UnityEngine;
using System.Collections;

public class NoEnoughCurrencyPopupController : PopupBaseController {


    #region User Intarations

    public void OpenShopButtonAciton()
    {
        PopupsManager.Instance.DisplayPopup<CurrencyShopPopupController>();
        ClosePopup();
    }

    public void CloseButtonAction()
    {
        ClosePopup();
    }

    #endregion
}
