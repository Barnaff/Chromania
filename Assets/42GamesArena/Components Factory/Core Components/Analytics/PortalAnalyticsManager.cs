using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PortalAnalyticsManager : MonoBehaviour, IPortalAnalytics {


	[SerializeField]
	private bool _enableGoogleAnalytics = true;

	[SerializeField]
	private bool _enableAppsFlyerAnalytics = true;

	[SerializeField]
	private bool _enableDebug = true;

	[SerializeField]
	private string _appsflyerAppleAppID;

	[SerializeField]
	private string _appsflyerAndroidAppID;

	[SerializeField]
	private string _appsflyerKey;

	[SerializeField]
	private bool _disableAnalyticsInEditor;


	void Start()
	{
		InitializeAnalyticsServices();
	}

	private void InitializeAnalyticsServices()
	{
		#if UNITY_EDITOR
		if (_disableAnalyticsInEditor)
		{
			return;
		}
		#endif

		if (_enableGoogleAnalytics)
		{

		}

		if (_enableAppsFlyerAnalytics)
		{
#if UNITY_IOS
			AppsFlyer.setAppID(_appsflyerAppleAppID);
#endif

#if UNITY_ANDROID
			AppsFlyer.setAppID(_appsflyerAndroidAppID);
#endif

			AppsFlyer.setAppsFlyerKey(_appsflyerKey);
			AppsFlyer.trackAppLaunch();
			
			#if UNITY_EDITOR 
			AppsFlyer.setIsDebug(true);
			#endif

			if (_enableDebug)
			{
				Debug.Log("<color=brown><b>Initializeed Appflyer</b></color>");
			}
		}
	}

	public void SendScreenHit(AnalyticsServiceType serviceType, string screenName)
	{
		#if UNITY_EDITOR
		if (_disableAnalyticsInEditor)
		{
			return;
		}
		#endif

		if (serviceType == AnalyticsServiceType.GoogleAnalytics && _enableGoogleAnalytics)
		{
			GoogleAnalytics.Client.SendScreenHit(screenName);
		}

		if (serviceType == AnalyticsServiceType.Appsflyer && _enableAppsFlyerAnalytics)
		{
			AppsFlyer.trackEvent("screenHit", screenName);
		}

		if (serviceType == AnalyticsServiceType.GamesArenaService)
		{

		}

		if (_enableDebug)
		{
			Debug.Log(string.Format("<color=brown><b>Screen Hit</b>: serviceType: {0}, screenName: {1}</color>", serviceType, screenName));
		}
	}

	public void SendEventHit(AnalyticsServiceType serviceType, string category, string action, string label = "", int value = -1)
	{
		#if UNITY_EDITOR
		if (_disableAnalyticsInEditor)
		{
			return;
		}
		#endif

		if (serviceType == AnalyticsServiceType.GoogleAnalytics && _enableGoogleAnalytics)
		{
			GoogleAnalytics.Client.SendEventHit(category, action, label, value);
		}
		
		if (serviceType == AnalyticsServiceType.Appsflyer && _enableAppsFlyerAnalytics)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data.Add("category", category);
			data.Add("action", action);
			data.Add("label", label);
			data.Add("value", value.ToString());
			AppsFlyer.trackRichEvent(action, data);
		}
		
		if (serviceType == AnalyticsServiceType.GamesArenaService)
		{
			
		}

		if (_enableDebug)
		{
			Debug.Log(string.Format("<color=brown><b>Event Hit</b>: serviceType: {0} ,category: {1}, action: {2}, label: {3}, value: {4}</color>", serviceType, category, action, label, value));
		}
	}
}

public class  AnalyticsUtil
{
	private static IPortalAnalytics _instance;

	private static IPortalAnalytics Instance()
	{
		if (AnalyticsUtil._instance == null)
		{
			AnalyticsUtil._instance = ComponentFactory.GetAComponent<IPortalAnalytics>();
		}
		return AnalyticsUtil._instance;
	}

	public static void SendScreenHit(string screenName)
	{
		AnalyticsUtil.SendScreenHit(AnalyticsServiceType.GoogleAnalytics, screenName);
	}

	public static void SendScreenHit(AnalyticsServiceType serviceType ,string screenName)
	{
		AnalyticsUtil.Instance().SendScreenHit(serviceType, screenName);
	}

	public static void SendEventHit(string category, string action, string label = "", int value = -1)
	{
		AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics, category, action, label, value);
	}

	public static void SendEventHit(AnalyticsServiceType serviceType, string category, string action, string label = "", int value = -1)
	{
		AnalyticsUtil.Instance().SendEventHit(serviceType, category, action, label, value);
	}

}
