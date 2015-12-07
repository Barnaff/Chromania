using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void UnlockItemActionDelegate(ShopItemDataModel shopItem);

public class TileUnlockCellController : MonoBehaviour {

	[SerializeField]
	public ePriceAction CellActionType;

	[SerializeField]
	private Text _itemTitle;

	[SerializeField]
	private Image _itemIcon;

	private ShopItemDataModel _shopItem;

	public UnlockItemActionDelegate OnUnlockAction;

	#region Public

	public void SetShopItem(ShopItemDataModel shopItem)
	{
		_shopItem = shopItem;

		switch (_shopItem.Price.ActionType)
		{
		case ePriceAction.INCENT:
		{
			SetIncentShopItem();
			break;
		}
		case ePriceAction.COINS:
		{
			SetCoinsItem();
			break;
		}
		default:
		{
			break;
		}
		}
	}


	public void ActionButtonPressed()
	{
		if (OnUnlockAction != null)
		{
			OnUnlockAction(_shopItem);
		}
	}
	

	#endregion


	#region Private

	private void SetIncentShopItem()
	{
		this.gameObject.SetActive(false);
		_itemTitle.text = "";
		_itemIcon.gameObject.SetActive(false);
		IAds adsManager = ComponentFactory.GetAComponent<IAds>();
		// display loader indicator
		if (adsManager != null)
		{
			adsManager.GetAdForShopitem(_shopItem, (ad)=>
			                            {
				// hide loader indicator
				// display ad;
				if (ad != null)
				{
					_shopItem.Price.Link = ad.AdTargetURL;
					this.gameObject.SetActive(true);
					if (!string.IsNullOrEmpty(ad.AdDisplayName))
					{
						_itemTitle.text = "Download " + ad.AdDisplayName;
					}
					else
					{
						_itemTitle.text = "Download App";
					}

					if (!string.IsNullOrEmpty(ad.AdImageURL))
					{
						StartCoroutine(LoadTileImageFromWeb(ad.AdImageURL));
					}
				}
			});
		}
	}

	private void SetCoinsItem()
	{
		_itemTitle.text = _shopItem.Price.Value.ToString() + " Coins";
	}

	private void SetScoreItem()
	{

	}

	private void SetShareItem()
	{

	}


	IEnumerator LoadTileImageFromWeb(string imagePath)
	{
		_itemIcon.gameObject.SetActive(true);
		_itemIcon.CrossFadeColor(Color.black, 0.0f, true, false);
		
		WWW www = new WWW(imagePath);
		
		yield return www;
		
		Texture2D texture = www.texture;
		
		Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.zero);
		_itemIcon.sprite = sprite;
		
		_itemIcon.CrossFadeColor(Color.white, 1.0f, true, false);
		
	}

	#endregion

}


