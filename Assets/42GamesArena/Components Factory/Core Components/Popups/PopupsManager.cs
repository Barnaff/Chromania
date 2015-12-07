using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupsManager : MonoBehaviour , IPopups
{

	[SerializeField]
	private PopupBaseController[] _popupsPrefabs;

	[SerializeField]
	private GameObject _overlayPrefab;

	private List<PopupBaseController> _activePopups = new List<PopupBaseController>();

	private GameObject _activeOverlay;


	#region IPopups Implementation

	public T DisplayPopup<T>(System.Action closeAction = null) where T: class 
	{
		try
		{ 
			PopupBaseController popupPrefab = GetPopupPrefab(typeof(T));

			if (popupPrefab != null)
			{
				GameObject popupGameobject = Instantiate(popupPrefab.gameObject) as GameObject;
				PopupBaseController popupController = popupGameobject.GetComponent<PopupBaseController>() as PopupBaseController;
				popupController.DisplayPopup(()=>
				                             {
					
				});
				popupController.PopupClosedAction += closeAction;
				_activePopups.Add(popupController);
				popupController.OnPopupClosed += RemovePopupHandler;
				popupController.OnPopupWillClosed += PopupWillRemoveHandler;
				DisplayOverlayForPopup(popupController);
				return popupController as T;
			}

		}
		catch (System.Exception e)
		{
			Debug.LogError("ERROR - Could not load popup for type: " + typeof(T).ToString() + " stack: " +  e);
		}
		
		return null;
	}

	public void CloseAllPopups()
	{
		List<PopupBaseController> _tmpPopupList = new List<PopupBaseController>();
		foreach (PopupBaseController popupController in _tmpPopupList)
		{
			_tmpPopupList.Add(popupController);
		}
		for (int i= _tmpPopupList.Count - 1; i >= 0; i--)
		{
			PopupBaseController popupController = _tmpPopupList[i];
			if (popupController != null)
			{
				popupController.ClosePopup();
			}
		}
	}

	public bool IsDisplayingPopup()
	{
		return (_activePopups != null && _activePopups.Count > 0);
	}
	
	public PopupBaseController CurrentActivePopup()
	{
		if (_activePopups.Count > 0)
		{
			return _activePopups[_activePopups.Count - 1];
		}
		return null;
	}


	#endregion


	#region Private

	private PopupBaseController GetPopupPrefab(System.Type popupType)
	{
		for (int i = 0; i< _popupsPrefabs.Length; i++)
		{
			if (_popupsPrefabs[i].GetType().Equals(popupType))
			{
				return _popupsPrefabs[i];
			}
		}
		Debug.LogError("Could not find popup for type: " + popupType);
		return null;
	}

	private GameObject GetPopupPrefab<T>() where T: class 
	{
		for (int i=0; i< _popupsPrefabs.Length; i++)
		{
			if (_popupsPrefabs[i].gameObject.GetComponent(typeof(T)) != null)
			{



				return _popupsPrefabs[i].gameObject;
			}
		}
		return null;
	}

	private void DisplayOverlayForPopup(PopupBaseController popupController)
	{
		if (popupController.EnableOverlay)
		{
			if (_activeOverlay == null)
			{
				_activeOverlay = Instantiate(_overlayPrefab) as GameObject;

				DontDestroyOnLoad(_activeOverlay);

				if (popupController.EnterAnimationType != PopupEnterAnimationType.None)
				{
					iTween.ValueTo(_activeOverlay, iTween.Hash("time", popupController.EnterAnimationDuration, 
					                                           "from", 0, 
					                                           "to" , 1, 
					                                           "onupdate", "OnOverlayValueUpdate", 
					                                           "onupdatetarget", this.gameObject, 
					                                           "easetype", iTween.EaseType.easeOutBack, 
					                                           "ignoretimescale", true));
				}

			}

		}
	}

	private void OnOverlayValueUpdate(float value)
	{
		if (_activeOverlay != null)
		{
			for (int i=0; i< _activeOverlay.transform.childCount; i++)
			{
				_activeOverlay.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(0.0f,0.0f,0.0f,value * 0.7f);
			}
		}
	}

	#endregion


	#region Update



	#endregion



	#region Events

	private void PopupWillRemoveHandler(PopupBaseController popupController)
	{
		if (_activePopups.Contains(popupController))
		{
			_activePopups.Remove(popupController);
		}
		
		bool needToRemoveOverlay = true;
		foreach (PopupBaseController popup in _activePopups)
		{
			if (popup != null && popup.EnableOverlay)
			{
				needToRemoveOverlay = false;
				break;
			}
		}
		
		if (needToRemoveOverlay)
		{
			if (_activeOverlay != null)
			{
				System.Action removeOverlayAction = ()=>
				{
					Destroy(_activeOverlay);
					_activeOverlay = null;
				};
				
				if (popupController.ExitAnimationType != PopupExitAnimationType.None)
				{
					
					iTween.ValueTo(_activeOverlay, iTween.Hash("time", popupController.ExitAnimationDuration, 
					                                           "from", 1, 
					                                           "to" , 0, 
					                                           "onupdate", "OnOverlayValueUpdate", 
					                                           "onupdatetarget", this.gameObject, 
					                                           "oncompleteaction", removeOverlayAction, 
					                                           "easetype", iTween.EaseType.easeOutBack, 
					                                           "ignoretimescale", true));
				}
				else
				{
					removeOverlayAction();
				}
			}
		}
	}

	private void RemovePopupHandler(PopupBaseController popupController)
	{

	}

	#endregion
}
