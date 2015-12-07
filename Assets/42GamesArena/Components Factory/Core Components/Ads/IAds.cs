using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void AdPresentedDelegate(AdDataModel ad);

public delegate void AdClickedDelegate(AdDataModel ad);

public interface IAds 
{
	void Initialize(System.Action completionAction);

	bool AdsReady { get; }

	void GetAdsListFromServer(System.Action <List<AdDataModel>> completionAction);

	void GetAdForShopitem(ShopItemDataModel shopItem, System.Action <AdDataModel> completionAction);

	void GetAdForTile(AdTileDataModel tile, System.Action <AdDataModel> completionAction);

	AdDataModel GetAdForPresentation (AdPresentationType adPresentationType);

	void ClickAdFromTile(AdDataModel ad, AdTileDataModel tile);

	void DisplayVideoAd(System.Action <bool> completionAction);

	event AdPresentedDelegate OnAdPresented;

	event AdClickedDelegate OnAdClicked;


}
