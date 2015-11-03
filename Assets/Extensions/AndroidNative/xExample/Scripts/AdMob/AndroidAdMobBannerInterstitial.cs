using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//Attach the script to the empty gameobject on your sceneS
public class AndroidAdMobBannerInterstitial : MonoBehaviour {


	public string InterstitialUnityId;


	// --------------------------------------
	// Unity Events
	// --------------------------------------
	
	void Awake() {

		if(AndroidAdMobController.Instance.IsInited) {
			if(!AndroidAdMobController.Instance.InterstisialUnitId.Equals(InterstitialUnityId)) {
				AndroidAdMobController.Instance.SetInterstisialsUnitID(InterstitialUnityId);
			} 
		} else {
			AndroidAdMobController.Instance.Init(InterstitialUnityId);
		}


	}

	void Start() {
		ShowBanner();
	}




	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	public void ShowBanner() {
		AndroidAdMobController.Instance.StartInterstitialAd();
	}



	// --------------------------------------
	// GET / SET
	// --------------------------------------



	public string sceneBannerId {
		get {
			return Application.loadedLevelName + "_" + this.gameObject.name;
		}
	}

	
}
