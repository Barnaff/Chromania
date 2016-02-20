using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackButtonManager : MonoBehaviour , IBackButton {

	private	IPopups _popupsManager;

	private List<OnBackButtonMethod> _responders = new List<OnBackButtonMethod>();

	void Start()
	{
		_popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
	}

	void Update()
	{ 
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Back();
		}
	}

	public void RegisterToBackButton(OnBackButtonMethod callBack)
	{
		if (_responders.Contains(callBack))
		{
			_responders.Remove(callBack);
		}
		_responders.Add(callBack);

	}

	public void RemoveResponderFromBackButton(OnBackButtonMethod callBack)
	{
		if (_responders.Contains(callBack))
		{
			_responders.Remove(callBack);
		}
	}

	private void Back()
	{
		if (_popupsManager != null && _popupsManager.IsDisplayingPopup())
		{
			PopupBaseController popupBaseController =  _popupsManager.CurrentActivePopup();
			if (popupBaseController != null)
			{
				popupBaseController.BackButtonPressed();
			}
			return;
		}

		if (_responders.Count > 0)
		{
			OnBackButtonMethod callback = _responders[_responders.Count - 1];
			if (callback != null)
			{
				callback();
			}
		}
	}

}
