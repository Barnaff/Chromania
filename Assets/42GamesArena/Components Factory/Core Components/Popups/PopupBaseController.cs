using UnityEngine;
using System.Collections;

public delegate void PopupClosedDelegate(PopupBaseController popupController);
public delegate void PopupWillClosedDelegate(PopupBaseController popupController);

public enum PopupEnterAnimationType
{
	None,
	Punch,
	Bouncy,
	FromTop,
	FropBottom,
	FromLeft,
	FromRight,
	FadeIn,
}

public enum PopupExitAnimationType
{
	None,
	Bouncy,
	ToTop,
	ToBottom,
	ToLeft,
	ToRight,
	FadeOut,
}

public class PopupBaseController : MonoBehaviour
{
	#region Private Properties

	[SerializeField]
	private GameObject _popupPanel;

	[SerializeField]
	private float _enterAnimationDuration = 0.5f;

	[SerializeField]
	private float _exitAnimationDuration = 0.5f;

	[SerializeField]
	private PopupEnterAnimationType _enterAnimationType;

	[SerializeField]
	private PopupExitAnimationType _exitAnimationType;

	[SerializeField]
	public bool EnableOverlay = true;

	[SerializeField]
	private bool _dontDestroyOnLoad = true;

	#endregion


	#region Public Properties

	public PopupClosedDelegate OnPopupClosed;
	public PopupWillClosedDelegate OnPopupWillClosed;

	public System.Action PopupClosedAction;

	#endregion


	#region Initialization


	#endregion


	#region Public

	public float EnterAnimationDuration {
		get {
			return _enterAnimationDuration;
		}
	}

	public float ExitAnimationDuration {
		get {
			return _exitAnimationDuration;
		}
	}

	public PopupEnterAnimationType EnterAnimationType {
		get {
			return _enterAnimationType;
		}
	}

	public PopupExitAnimationType ExitAnimationType {
		get {
			return _exitAnimationType;
		}
	}

	public void DisplayPopup(System.Action completionAction)
	{
		PopupWillShow();

		DisplayEnterAnimation(()=>
		                      {
			PopupDidShow();
			if (completionAction != null)
			{
				completionAction();
			}
		});

		if (_dontDestroyOnLoad)
		{
			DontDestroyOnLoad(this.gameObject);
		}
		
		string popupName = this.gameObject.name;
		popupName = popupName.Replace("(Clone)", "");
		AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics, 
		                           AnalyticsEvents.ANALYTICS_CATEGORY_POPUPS,
		                           AnalyticsEvents.ANALYTICS_EVENT_DISPLAY_POPUP + popupName);

		AnalyticsUtil.SendScreenHit(AnalyticsServiceType.GoogleAnalytics,  popupName);
	}

	public void ClosePopup()
	{
		ClosePopup(null);
	}

	public void ClosePopup(System.Action completionAction)
	{
		PopupWillDismiss();
		if (OnPopupWillClosed != null)
		{
			OnPopupWillClosed(this);
		}
		DisplayExitAnimation(()=>
		                     {

			PopupDidDismiss();
			if (OnPopupClosed != null)
			{
				OnPopupClosed(this);
			}
			if (PopupClosedAction != null)
			{
				PopupClosedAction();
			}
			if (completionAction != null)
			{
				completionAction();
			}
			if (this.gameObject != null)
			{

				string popupName = this.gameObject.name;
				popupName = popupName.Replace("(Clone)", "");
				AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics, 
				                           AnalyticsEvents.ANALYTICS_CATEGORY_POPUPS,
				                           AnalyticsEvents.ANALYTICS_EVENT_CLOSED_POPUP + popupName);

				Destroy(this.gameObject);
			}
		});

	}
	
	public void DisplayEnterAnimation(System.Action completionAction)
	{
		switch (_enterAnimationType)
		{
		case PopupEnterAnimationType.Punch:
		{
			iTween.PunchScale(_popupPanel, iTween.Hash("time", _enterAnimationDuration, 
			                                           "x", 1.2f, 
			                                           "y", 1.2f, 
			                                           "oncompleteaction", completionAction, 
			                                           "ignoretimescale", true));
			break;
		}
		case PopupEnterAnimationType.Bouncy:
		{
			iTween.ScaleFrom(_popupPanel, iTween.Hash("time", _enterAnimationDuration, 
			                                          "x", 0, 
			                                          "y", 0,  
			                                          "easetype", iTween.EaseType.easeOutBack, 
			                                          "oncompleteaction", completionAction, 
			                                          "ignoretimescale", true));
			break;
		}
		case PopupEnterAnimationType.FromTop:
		{
			iTween.MoveFrom(_popupPanel, iTween.Hash("time", _enterAnimationDuration, 
			                                         "y", Screen.height, 
			                                         "easetype", iTween.EaseType.easeOutBack, 
			                                         "oncompleteaction", completionAction, 
			                                         "islocal", true, 
			                                         "ignoretimescale", true));
			break;
		}
		case PopupEnterAnimationType.FropBottom:
		{
			iTween.MoveFrom(_popupPanel, iTween.Hash("time", _enterAnimationDuration, 
			                                         "y", -Screen.height, 
			                                         "easetype", iTween.EaseType.easeOutBack, 
			                                         "oncompleteaction", completionAction, 
			                                         "islocal", true, 
			                                         "ignoretimescale", true));
			break;
		}
		case PopupEnterAnimationType.FadeIn:
		{
			CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				Debug.LogWarning("WARNING - Popup does not contain CanvasGroup, adding new CanvasGroup");
				canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
			}
			System.Action <float> updateAction = (float value)=>
			{
				canvasGroup.alpha = value;
			};
			iTween.ValueTo(_popupPanel, iTween.Hash("time", _enterAnimationDuration, 
			                                        "from", 0,
			                                        "to", 1,
			                                        "easetype", iTween.EaseType.easeOutBack, 
			                                        "onupdateaction", updateAction, 
			                                        "oncompleteaction", completionAction, 
			                                        "ignoretimescale", true));
			break;
		}
		case PopupEnterAnimationType.None:
		default:
		{
			if (completionAction != null)
			{
				completionAction();
			}
			break;
		}
		}
	}

	public void DisplayExitAnimation(System.Action completionAction)
	{
		switch (_exitAnimationType)
		{
		case PopupExitAnimationType.Bouncy:
		{
			iTween.ScaleTo(_popupPanel, iTween.Hash("time", _exitAnimationDuration, 
			                                        "x", 0, 
			                                        "y", 0, 
			                                        "easetype", iTween.EaseType.easeInBack, 
			                                        "oncompleteaction", completionAction, 
			                                        "ignoretimescale", true));
			break;
		}
		case PopupExitAnimationType.ToBottom:
		{
			iTween.MoveTo(_popupPanel, iTween.Hash("time", _exitAnimationDuration, 
			                                       "y", -Screen.height, 
			                                       "easetype",  iTween.EaseType.easeInBack, 
			                                       "oncompleteaction", completionAction, 
			                                       "islocal", true, 
			                                       "ignoretimescale", true));
			break;
		}
		case PopupExitAnimationType.ToTop:
		{
			iTween.MoveTo(_popupPanel, iTween.Hash("time", _exitAnimationDuration, 
			                                       "y", Screen.height,
			                                       "easetype",  iTween.EaseType.easeInBack,
			                                       "oncompleteaction", completionAction,
			                                       "islocal", true, 
			                                       "ignoretimescale", true));
			break;
		}
		case PopupExitAnimationType.FadeOut:
		{
			CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				Debug.LogWarning("WARNING - Popup does not contain CanvasGroup, adding new CanvasGroup");
				canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
			}
			System.Action <float> updateAction = (float value)=>
			{
				canvasGroup.alpha = 1.0f - value;
			};
			iTween.ValueTo(_popupPanel, iTween.Hash("time", _exitAnimationDuration, 
			                                        "from", 0,
			                                        "to", 1,
			                                        "easetype", iTween.EaseType.easeOutBack, 
			                                        "onupdateaction", updateAction, 
			                                        "oncompleteaction", completionAction,
			                                        "ignoretimescale", true));
			break;
		}
		case PopupExitAnimationType.None:
		default:
		{
		if (completionAction != null)
			{
				completionAction();
			}
			break;
		}
		}
	}

	#endregion


	#region Subclassing

	public virtual void PopupWillShow()
	{
		
	}
	
	public virtual void PopupDidShow()
	{
		
	}
	
	public virtual void PopupWillDismiss()
	{
		
	}
	
	public virtual void PopupDidDismiss()
	{

	}

	public virtual void BackButtonPressed()
	{
		ClosePopup();
	}


	#endregion
}

