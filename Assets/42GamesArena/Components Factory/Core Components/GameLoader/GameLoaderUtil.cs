using UnityEngine;
using System.Collections;

public class GameLoaderUtil : MonoBehaviour  {

	private static GameDefenitionDataModel _currentGame;

	public static void ReplayCurrentGame()
	{
		if (GameLoaderUtil._currentGame != null)
		{
			LoadGame(GameLoaderUtil._currentGame, true);
		}
	}

	public static void LoadGame(GameDefenitionDataModel game, bool isInGame = false)
	{
		GameLoaderUtil._currentGame = game;
		IHearts heartsManager = ComponentFactory.GetAComponent<IHearts>() as IHearts;
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>() as IPortal;
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		GameObject gameLoaderContainer = new GameObject();
		gameLoaderContainer.name = game.GameName + " Game Launcher";
		GameLoaderUtil gameLoader = gameLoaderContainer.AddComponent<GameLoaderUtil>() as GameLoaderUtil;
		DontDestroyOnLoad(gameLoaderContainer);

		if (heartsManager.HeartsEnabled)
		{
			if (heartsManager.HeartsCount > 0)
			{
				if (portalManager.IsTurnementEnabled)
				{
					PreGameTurnementPopupController preGameTurnrmentPopup = popupsManager.DisplayPopup<PreGameTurnementPopupController>();

					preGameTurnrmentPopup.SetPlayButtonAction(()=>
					                                          {
						heartsManager.ReduceHeart();
						gameLoader.LaunchGame(game, !isInGame);
					});
				}
				else
				{
					heartsManager.ReduceHeart();
					gameLoader.LaunchGame(game, !isInGame);
				}

			}
			else
			{
				// not enough hearts
				NotEnoughHeartsPopupController notEnoughHeartsPopupController = popupsManager.DisplayPopup<NotEnoughHeartsPopupController>(()=>
				                                                                                                                           {
					if (isInGame)
					{
						if (popupsManager != null)
						{
							popupsManager.CloseAllPopups();
						}
						gameLoader.CancelGameLoad();
					}
				});
				notEnoughHeartsPopupController.BuyMoreAction = ()=>
				{
					popupsManager.DisplayPopup<HeartsShopPopupController>(()=>
					{
						notEnoughHeartsPopupController.ClosePopup();
					});
				};

			}
		}
		else
		{
			if (portalManager.IsTurnementEnabled)
			{
				PreGameTurnementPopupController preGameTurnrmentPopup = popupsManager.DisplayPopup<PreGameTurnementPopupController>();
				preGameTurnrmentPopup.SetPlayButtonAction(()=>
				                                          {
					gameLoader.LaunchGame(game, !isInGame);
				});
			}
			else
			{
				gameLoader.LaunchGame(game, !isInGame);
			}
		}
	}

	private void LaunchGame(GameDefenitionDataModel game, bool displayLoader)
	{
		StartCoroutine(LoadGameAsync(game, displayLoader));
	}

	IEnumerator LoadGameAsync (GameDefenitionDataModel game, bool displayLoader) 
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
		GameLoaderPopupController gameLoaderPopup = null;
		if (displayLoader)
		{
			if (popupsManager != null)
			{
				gameLoaderPopup = popupsManager.DisplayPopup<GameLoaderPopupController>();
				gameLoaderPopup.SetToLoadGame(this);
				yield return new WaitForSeconds(0.5f);
			}
		}

		Time.timeScale = 1;
		while (!AssetManager.instance.isReady)
		{
			Debug.LogError("assets manager is not ready!");
			yield return null;
		}
		AssetManager.instance.LoadBundle (game.GameBundleName);
		
		while (!AssetManager.instance.IsBundleLoaded(game.GameBundleName))
		{
			yield return null;
		}

		bool gameLoaded = false;
		GameplayBase gamePlayController = null;
		SceneLoaderUtil.LoadScene(game.GameSceneName, ()=>
		                          {
			gameLoaded = true;
			gamePlayController = GameObject.FindObjectOfType<GameplayBase>() as GameplayBase;
		});

		while (!gameLoaded)
		{
			yield return null;
		}

		if (gamePlayController != null)
		{
			gamePlayController.Initialize();
		}

		yield return new WaitForSeconds(1.5f);

		if (gameLoaderPopup != null)
		{
			gameLoaderPopup.ClosePopup();
			yield return new WaitForSeconds(0.5f);
		}

		if (gamePlayController != null)
		{
			gamePlayController.StartGame(game);
		}

		Destroy(this.gameObject);
	}

	public void CancelGameLoad()
	{
		Debug.Log("cancel game load");
		StopCoroutine("LoadGameAsync");
		Application.LoadLevel("Lobby Scene");
		Destroy(this.gameObject);
	}

	public GameDefenitionDataModel Game
	{
		get
		{
			return _currentGame;
		}
	}
	
}
