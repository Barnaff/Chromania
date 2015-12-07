using UnityEngine;
using System.Collections;

[System.Flags]
public enum AnalyticsServiceType
{
	GoogleAnalytics,
	Appsflyer,
	GamesArenaService,
}

public interface IPortalAnalytics  {

	void SendScreenHit( AnalyticsServiceType serviceType ,string screenName);

	void SendEventHit(AnalyticsServiceType serviceType, string category, string action, string label = "", int value = -1);
}
