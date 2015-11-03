using System;
using UnityEngine;
using System.Collections;

public class UM_Camera : SA_Singleton<UM_Camera> {

	//Actions
	public event Action<UM_ImagePickResult> OnImagePicked = delegate{};
	public event Action<UM_ImageSaveResult> OnImageSaved = delegate{};
	


	void Awake() {
		DontDestroyOnLoad(gameObject);

		AndroidCamera.Instance.OnImagePicked += OnAndroidImagePicked;
		IOSCamera.OnImagePicked += OnIOSImagePicked;

		AndroidCamera.Instance.OnImageSaved += OnAndroidImageSaved;
		IOSCamera.OnImageSaved += OnIOSImageSaved;
	}

	public void SaveImageToGalalry(Texture2D image) {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.Instance.SaveImageToGallery(image);
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.Instance.SaveTextureToCameraRoll(image);
				break;
		}
		
	}
	
	
	public void SaveScreenshotToGallery() {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.Instance.SaveScreenshotToGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.Instance.SaveScreenshotToCameraRoll();
				break;
		}
	}
	
	
	public void GetImageFromGallery() {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.Instance.GetImageFromGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.Instance.PickImage(ISN_ImageSource.Library);
				break;
		}
	}
	
	
	
	public void GetImageFromCamera() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidCamera.Instance.GetImageFromCamera();
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSCamera.Instance.PickImage(ISN_ImageSource.Camera);
			break;
		}
	}
	
	

	void OnAndroidImagePicked (AndroidImagePickResult obj) {
		UM_ImagePickResult result = new UM_ImagePickResult(obj.Image);
		OnImagePicked(result);
	}

	void OnIOSImagePicked (IOSImagePickResult obj) {
		UM_ImagePickResult result = new UM_ImagePickResult(obj.Image);
		OnImagePicked(result);
	}

	void OnAndroidImageSaved (GallerySaveResult res) {
		UM_ImageSaveResult result = new UM_ImageSaveResult(res.imagePath, res.IsSucceeded);
		OnImageSaved(result);
	}
	

	void OnIOSImageSaved (ISN_Result res) {
		UM_ImageSaveResult result = new UM_ImageSaveResult(string.Empty, res.IsSucceeded);
		OnImageSaved(result);
	}
}
