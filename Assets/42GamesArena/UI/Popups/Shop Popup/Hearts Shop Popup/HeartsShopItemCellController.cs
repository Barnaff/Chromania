using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void PurchaseItemDelegate(ShopItemDataModel shopItem);

public class HeartsShopItemCellController : MonoBehaviour {

	#region Public

	public PurchaseItemDelegate OnPurchase;

	#endregion


	#region Private Properties

	[SerializeField]
	private Image _itemImage;

	[SerializeField]
	private Text _itemTitleLabel;

	[SerializeField]
	private Text _itemPriceLabel;

	[SerializeField]
	private GameObject _buyButton;

	private ShopItemDataModel _shopItem;

	[SerializeField]
	private Sprite[] _itemSprites;

	#endregion


	#region Public

	public void SetShopItem(ShopItemDataModel shopItem)
	{
		if (shopItem != null)
		{
			_shopItem = shopItem;
			if (!string.IsNullOrEmpty(_shopItem.ItemName))
			{
				_itemTitleLabel.text = _shopItem.ItemName;
			}
			else
			{
				_itemTitleLabel.text = _shopItem.Product.Amount.ToString();
			}
			if (_shopItem.Price != null)
			{
				if (_shopItem.Price.ActionType == ePriceAction.IAP)
				{
					_itemPriceLabel.text = _shopItem.Price.Value.ToString();
					_itemPriceLabel.text += "$";
				}

				if (_shopItem.Price.ActionType == ePriceAction.INCENT)
				{
					_itemPriceLabel.text = "Download";
				}
			} 
			else
			{
				_itemPriceLabel.text = "BUY";
			}


			// TODO - fix to show different icons for different products.
			for (int i = 0; i< _itemSprites.Length ; i++)
			{
				if (_itemSprites[i].name == "pile_" + shopItem.Product.Amount)
				{
					
					_itemImage.sprite = _itemSprites[i];
					break;
				}
			}

		
		}

	}

	public void BuyButtonAction()
	{
		if (OnPurchase != null)
		{
			OnPurchase(_shopItem);
		}
	}

	#endregion
}
