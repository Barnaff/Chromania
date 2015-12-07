using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void TileSelectDelegate(LobbyTileController tileController);

public class LobbyTileController : MonoBehaviour {

	#region Private Properties

	[SerializeField]
	public Text _titleLabel;

	[SerializeField]
	private Image _tileImage;

	private TileSelectDelegate _tileSelectDelegate;

	[SerializeField]
	private TileDataModelAbstract _tileDataModel;

	[SerializeField]
	private GameObject _lockOverlay;

	[SerializeField]
	private GameObject _playButton;
	

	#endregion


	#region Initialization

	void Start()
	{

	}

	#endregion
	

	#region Public

	public void SetTile(TileDataModelAbstract tile, bool isUnlocked, TileSelectDelegate tileSelectCallBack = null)
	{
		_tileDataModel = tile;

		if (tileSelectCallBack != null)
		{
			_tileSelectDelegate = tileSelectCallBack;
		}

		if (tile is GameTileDataModel)
		{
			_titleLabel.text = ((GameTileDataModel)_tileDataModel).GameDefenition.GameName;
			SetGameTile();
		}
		else if (tile is AdTileDataModel)
		{
			SetAdTile();
		}

		SetLockState(isUnlocked);

	}

	public void TileSelectAction()
	{
		if (_tileSelectDelegate != null)
		{
			_tileSelectDelegate(this);
		}
	}

	public TileDataModelAbstract TileData
	{
		get
		{
			return _tileDataModel;
		}
	}

	public AdDataModel Ad
	{
		get
		{
			AdTileDataModel adTileModel = (AdTileDataModel)_tileDataModel;
			return adTileModel.Ad;
		}
	}

	#endregion


	#region Private

	private void SetGameTile()
	{
		GameDefenitionDataModel gameDefenition = ((GameTileDataModel)_tileDataModel).GameDefenition;
		if (gameDefenition != null)
		{

			string tileImagePath = "https://s3.amazonaws.com/forteetwogamesarena/icons/casino/" + gameDefenition.GameIconName + "_tilesize1.png";
			LoadTileImageFromWeb(tileImagePath);

			if (_playButton != null)
			{
				_playButton.SetActive(true);
			}

		}
	}

	private void SetAdTile()
	{
		AdTileDataModel adTileModel = (AdTileDataModel)_tileDataModel;
		string imageURL = adTileModel.ImageForTileSize(_tileDataModel.TileSize);
		if (adTileModel.Ad != null && !string.IsNullOrEmpty(imageURL))
		{
			LoadTileImageFromWeb(imageURL);
		}


		if (_playButton != null)
		{
			_playButton.SetActive(false);
		}
	}  

	private void LoadTileImageFromWeb(string imagePath)
	{

		IWebImageCache webImageCache = ComponentFactory.GetAComponent<IWebImageCache>();
		Sprite sprite = null;
		if (webImageCache != null)
		{

			bool isLocalImage = webImageCache.HasImageInCache(imagePath);

			if (!isLocalImage)
			{
				_tileImage.CrossFadeColor(Color.black, 0.0f, true, false);
			}

			webImageCache.LoadImage(imagePath, (texture)=>
			                        {
				if (texture != null)
				{
					texture.mipMapBias = 1;
					sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.zero, 100);
					if (sprite != null && _tileImage != null)
					{
						_tileImage.sprite = sprite;


						_tileImage.CrossFadeColor(Color.white, 1.0f, true, false);

					}

				}
			});
		}
	}

	private void SetLockState(bool isUnlocked)
	{
		_lockOverlay.SetActive(!isUnlocked);

		if (!isUnlocked)
		{
			_tileImage.color = Color.gray;
		}
	}

	#endregion
}
