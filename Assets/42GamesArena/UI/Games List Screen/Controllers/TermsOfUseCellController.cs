using UnityEngine;
using System.Collections;

public class TermsOfUseCellController : MonoBehaviour {


	public void OpenTermsOfUse()
	{
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>();
		if (portalManager != null)
		{
			string termsOfUseURL = portalManager.TermsOfUseURL;
			Application.OpenURL(termsOfUseURL);
			AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics, "URL", "terms of use");
		}
	}
}
