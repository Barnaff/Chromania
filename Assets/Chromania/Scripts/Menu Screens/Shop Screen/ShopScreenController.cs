using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopScreenController : MonoBehaviour {

    #region Private proeprties

    [SerializeField]
    private GameObject _shopitemCellPrefab;

    [SerializeField]
    private Transform _shopitemListContent;

    #endregion

    #region Initialization

    void Start () {
	
        if (_shopitemCellPrefab != null)
        {
            _shopitemCellPrefab.SetActive(false);
        }

        PopulateShopitems();
    }

    #endregion


    #region Public

    public void BackButtonAction()
    {
        FlowManager.Instance.DisplayMainMenu();
    }

    #endregion


    #region Private

    private void PopulateShopitems()
    {
        for (int i=0; i < _shopitemListContent.childCount; i++)
        {
            if (_shopitemListContent.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(_shopitemListContent.GetChild(i).gameObject);
            }
        }

        List<ShopItem> shopitems = ShopManager.Instance.GetShopitems(ShopItem.eShopItemCategoty.Chromiez);
        foreach (ShopItem shopItem in shopitems)
        {
            GameObject shopItemCell = Instantiate(_shopitemCellPrefab);
            shopItemCell.SetActive(true);
            shopItemCell.transform.SetParent(_shopitemListContent);
            shopItemCell.transform.localScale = Vector3.one;
        }
    }

    #endregion
}
