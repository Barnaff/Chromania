using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemAquiredPopupController : PopupBaseController {

	#region Private Properties

	[SerializeField]
	private GameObject _coinsPanel;

	[SerializeField]
	private GameObject _tileUnlockPanel;

	[SerializeField]
	private Text _coinsLabel;

	[SerializeField]
	private Image _tileIconImage;

	[SerializeField]
	private Image _lockTop;

	[SerializeField]
	private Image _lockBottom;

	[SerializeField]
	private GameObject _lockContainer;

	#endregion



	#region Public

	public void DisplayUnlockForItem(string itemType, int amount)
	{
		Debug.Log("display unlock for : " + itemType);

		if (itemType == "1")
		{
			StartCoroutine(DisplayCoinsEarned(amount));
		}
		else if (itemType.Contains("."))
		{
			StartCoroutine(DisplayTileUnlocked(itemType));
		}
		else
		{
			Debug.LogError("ERROR - Unsupported item type: " + itemType + " to display aquire popup!");
		}
	}

	#endregion


	#region Private

	IEnumerator DisplayCoinsEarned(int amount)
	{
		_coinsPanel.SetActive(true);
		_tileUnlockPanel.SetActive(false);

		_coinsLabel.text = "x " + amount.ToString();

		yield return null;

		yield return new WaitForSeconds(2.0f);

		ClosePopup();
	}

	IEnumerator DisplayTileUnlocked(string tileKey)
	{
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>();
		if (portalManager != null)
		{
			TileDataModelAbstract tileModel = portalManager.GetTileForKey(tileKey);

			if (tileModel != null && tileModel is GameTileDataModel)
			{

				GameTileDataModel gameTile = (GameTileDataModel)tileModel;

				string gameIconPath = "https://s3.amazonaws.com/forteetwogamesarena/icons/casino/" + gameTile.GameDefenition.GameIconName + "_tilesize1.png";
				LoadTileImageFromWeb(gameIconPath);
			}
		}

		yield return new WaitForSeconds(1.5f);

		iTween.RotateTo(_lockTop.gameObject, iTween.Hash("time", 1.0f, "z", -45, "easeType", iTween.EaseType.easeOutElastic));

		yield return new WaitForSeconds(1.5f);

		iTween.ScaleTo(_lockContainer, iTween.Hash("time", 1.0f, "scale", new Vector3(1.5f,1.5f,1)));

		_lockTop.CrossFadeAlpha(0, 1.0f, false);
		_lockBottom.CrossFadeAlpha(0, 1.0f, false);

		yield return new WaitForSeconds(1.5f);

		ClosePopup();
	}

	private void LoadTileImageFromWeb(string imagePath)
	{
		IWebImageCache webImageCache = ComponentFactory.GetAComponent<IWebImageCache>();
		Sprite sprite = null;
		if (webImageCache != null)
		{
			webImageCache.LoadImage(imagePath, (texture)=>
			                        {
				if (texture != null)
				{
					sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.zero);
					if (sprite != null && _tileIconImage != null)
					{
						_tileIconImage.sprite = sprite;
					}
				}
			});
		}
	}

	#endregion


}
