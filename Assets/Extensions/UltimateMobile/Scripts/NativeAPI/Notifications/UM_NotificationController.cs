using UnityEngine;
using System;
using System.Collections;

public class UM_NotificationController : SA_Singleton<UM_NotificationController> {

	//Actions
	public static event Action<UM_PushRegistrationResult> OnPushIdLoadResult = delegate{};


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	private bool IsPushListnersRegistred = false;
	public void RetrieveDevicePushId() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(!IsPushListnersRegistred) {
				GoogleCloudMessageService.ActionCMDRegistrationResult += HandleActionCMDRegistrationResult;
			}

			GoogleCloudMessageService.Instance.RgisterDevice();

			break;
		case RuntimePlatform.IPhonePlayer:
			#if UNITY_IPHONE

			#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1	|| UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
			IOSNotificationController.instance.RegisterForRemoteNotifications(RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound);
			#else
			IOSNotificationController.Instance.RegisterForRemoteNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
			#endif



			if(!IsPushListnersRegistred) {
				IOSNotificationController.OnDeviceTokenReceived += IOSPushTokenReceived;
			}
			#endif
			break;
		}

		IsPushListnersRegistred = true;

	}




	public void ShowNotificationPoup(string title, string messgae) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidNotificationManager.Instance.ShowToastNotification(messgae);
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSNotificationController.Instance.ShowGmaeKitNotification(title, messgae);
			break;
		}
	}

	public int ScheduleLocalNotification(string title, string message, int seconds) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			return AndroidNotificationManager.Instance.ScheduleLocalNotification(title, message, seconds);
		case RuntimePlatform.IPhonePlayer:
			ISN_LocalNotification notification = new ISN_LocalNotification(DateTime.Now.AddSeconds(seconds), message, true);
			notification.Schedule();
			return notification.Id;
		}

		return 0;
	}

	public void CancelLocalNotification(int id) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidNotificationManager.Instance.CancelLocalNotification(id);
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSNotificationController.Instance.CancelLocalNotificationById(id);
			break;
		}
	}

	public void CancelAllLocalNotifications() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidNotificationManager.Instance.CancelAllLocalNotifications();
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSNotificationController.Instance.CancelAllLocalNotifications();
			break;
		}
	}

	void HandleActionCMDRegistrationResult (GP_GCM_RegistrationResult res) {
		if(res.isSuccess) {
			OnRegstred();
		} else {
			OnRegFailed();
		}
	}


	private void OnRegFailed() {
		UM_PushRegistrationResult result = new UM_PushRegistrationResult(string.Empty, false);
		OnPushIdLoadResult(result);
	}
	

	private void OnRegstred() {
		UM_PushRegistrationResult result = new UM_PushRegistrationResult(GoogleCloudMessageService.Instance.registrationId, true);
		OnPushIdLoadResult(result);
	}

	private void IOSPushTokenReceived (IOSNotificationDeviceToken res){
		UM_PushRegistrationResult result = new UM_PushRegistrationResult(res.tokenString, true);
		OnPushIdLoadResult(result);
	}
}
