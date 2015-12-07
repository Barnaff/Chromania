using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseShopCellController : MonoBehaviour {

	#region Public Properties

	public delegate void ShopItemPurchaseActionDelegate(ShopItemDataModel shopItem);

	public ShopItemPurchaseActionDelegate OnShopItemPurchaseAction;

	#endregion

	#region Private Properties

	[SerializeField]
	protected ePriceAction _priceAction;

	[SerializeField]
	protected bool _populatePriceLabel;

	[SerializeField]
	protected Text _titleLabel;

	[SerializeField]
	protected Text _priceLabel;

	[SerializeField]
	protected Image _iconImage;

	[SerializeField]
	protected Button _actionButton;

	protected ShopItemDataModel _shopItem;

	#endregion


	#region Public

	public ePriceAction PriceAction
	{
		get
		{
			return _priceAction;
		}
	}

	public void SetShopItem(ShopItemDataModel shopItem)
	{
		_shopItem = shopItem;

		SetupCell();
	}

	#endregion


	#region UserActions

	public void PurchaseButtonAction()
	{
		if (OnShopItemPurchaseAction != null)
		{
			OnShopItemPurchaseAction(_shopItem);
		}
	}

	#endregion


	#region Subclassing

	protected virtual void SetupCell()
	{ 
		_titleLabel.text = _shopItem.Product.Amount.ToString();
		if (_populatePriceLabel)
		{
			if (!string.IsNullOrEmpty(_shopItem.Price.PriceString))
			{
				_priceLabel.text = _shopItem.Price.PriceString;
			}
			else
			{
				_priceLabel.text = _shopItem.Price.Value.ToString();
			}
		}

		if (_iconImage != null && !string.IsNullOrEmpty(_shopItem.IconURL))
		{
			LoadTileIconFromWeb(_shopItem.IconURL);
		}
	}

	#endregion

	#region Private

	private void LoadTileIconFromWeb(string imagePath)
	{
		IWebImageCache webImageCache = ComponentFactory.GetAComponent<IWebImageCache>();
		if (webImageCache != null)
		{
			Sprite sprite = null;
			bool isLocalImage = webImageCache.HasImageInCache(imagePath);
			
			if (!isLocalImage)
			{
				_iconImage.CrossFadeColor(Color.black, 0.0f, true, false);
			}
			
			webImageCache.LoadImage(imagePath, (texture)=>
			                        {
				if (texture != null)
				{
					texture.mipMapBias = 1;
					sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.zero, 100);
					if (sprite != null && _iconImage != null)
					{
						_iconImage.sprite = sprite;
						_iconImage.CrossFadeColor(Color.white, 1.0f, true, false);
						
					}
				}
			});
		}
	}

	#endregion

}
