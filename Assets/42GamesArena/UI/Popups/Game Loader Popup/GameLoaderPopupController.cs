using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLoaderPopupController : PopupBaseController {

	[SerializeField]
	private Image _loadingBarIndicator;

	[SerializeField]
	private Image _gameIcon;

	private GameLoaderUtil _gameLoader;

	public void SetToLoadGame(GameLoaderUtil gameLoader)
	{
		_gameLoader = gameLoader;
		AssetManager.instance.OnProgressUpdate += OnProgressUpdate;
		if (_gameLoader.Game != null)
		{
			string gameIconPath = "https://s3.amazonaws.com/forteetwogamesarena/icons/casino/" + gameLoader.Game.GameIconName + "_tilesize1.png";
			LoadTileImageFromWeb(gameIconPath);
		}
	}
	
	public override void PopupWillDismiss ()
	{
		base.PopupWillDismiss ();

		AssetManager.instance.OnProgressUpdate -= OnProgressUpdate;
	}

	private void OnProgressUpdate(float value)
	{
		_loadingBarIndicator.fillAmount = value;
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
					if (sprite != null && _gameIcon != null)
					{
						_gameIcon.sprite = sprite;
					}
				}
			});
		}
	}

	public override void BackButtonPressed ()
	{
		if (_gameLoader != null)
		{
			_gameLoader.CancelGameLoad();
		}
		base.BackButtonPressed ();
	}
}
