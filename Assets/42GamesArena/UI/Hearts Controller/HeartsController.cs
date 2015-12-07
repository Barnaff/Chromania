using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsController : MonoBehaviour {

	#region Private Properties

	[SerializeField]
	private GameObject _heartIconPrefab;

	[SerializeField]
	private GameObject _heartsContainer;

	[SerializeField]
	private Text _heartsRefillTimerLabel;

	[SerializeField]
	private GameObject _heartsShopButton;

	private IHearts _heartsManager;

	private List<GameObject> _heartsIconsList;

	#endregion


	#region Lifecycle

	// Use this for initialization
	void Start () {

		_heartsManager = ComponentFactory.GetAComponent<IHearts>() as IHearts;
		if (_heartsManager != null)
		{
			if (_heartsManager.HeartsEnabled)
			{
				// set the gamelist top padding
				float height = RectTransformUtility.PixelAdjustRect(this.GetComponent<RectTransform>(), GameObject.Find("Lobby Canvas").GetComponent<Canvas>()).size.y;
				GameObject.Find("Games Grid").GetComponent<GridLayoutGroup>().padding.top = (int)height;

				_heartsManager.OnHeartAdded += HeartAddedhandler;
				_heartsManager.OnHeartRemoved += HeartRemovedHandler;
				_heartsManager.OnOutOfHearts += OutOfHeartsHandler;
				_heartIconPrefab.transform.SetParent(_heartIconPrefab.transform.parent.parent.transform);
				_heartIconPrefab.SetActive(false);
				_heartsIconsList = new List<GameObject>();
				UpdateHearts();
			}
			else
			{
				this.gameObject.SetActive(false);
			}

		}
		else
		{
			Debug.LogError("ERROR - Hearts Manager is not avalable!");
		}
	}

	void OnDisable()
	{
		_heartsManager.OnHeartAdded -= HeartAddedhandler;
		_heartsManager.OnHeartRemoved -= HeartRemovedHandler;
		_heartsManager.OnOutOfHearts -= OutOfHeartsHandler;
	}
	
	// Update is called once per frame
	void Update () {

		if (_heartsManager != null)
		{
			System.TimeSpan timeToNextHeart = _heartsManager.TimeToNextHeart;
			if (timeToNextHeart > System.TimeSpan.Zero)
			{
				_heartsRefillTimerLabel.text = timeToNextHeart.Minutes + ":" + timeToNextHeart.Seconds;
			}
			else
			{
				_heartsRefillTimerLabel.text = "";
			}

			if (_heartsManager.HeartsCount < _heartsManager.MaxHeartsCount)
			{
				_heartsShopButton.SetActive(true);
			}
			else
			{
				_heartsShopButton.SetActive(false);
			}
		}
	}

	#endregion


	#region Public

	public void HeartsShopButtonAction()
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		if (popupsManager != null)
		{
			popupsManager.DisplayPopup<HeartsShopPopupController>();
		}
	}

	#endregion


	#region private

	private void UpdateHearts()
	{
		for (int i=0; i< _heartsContainer.transform.childCount; i++)
		{
			Destroy(_heartsContainer.transform.GetChild(i).gameObject);
		}
		foreach (GameObject heartIocn in _heartsIconsList)
		{
			Destroy(heartIocn);
		}
		_heartsIconsList.Clear();

		for (int i = 0; i < _heartsManager.HeartsCount; i++)
		{
			AddHeartIcon();
		}
	}

	private void AddHeartIcon(bool animate = false)
	{
		GameObject heartIcon = Instantiate(_heartIconPrefab) as GameObject;
		heartIcon.transform.SetParent(_heartsContainer.transform);
		heartIcon.SetActive(true);
		_heartsIconsList.Add(heartIcon);
		if (animate)
		{
			heartIcon.transform.localScale = Vector3.zero;
			iTween.ScaleTo(heartIcon.gameObject, iTween.Hash("time", 0.5f, "x", 1.0f, "y", 1.0f, "easetype", iTween.EaseType.easeOutElastic));
		}
	}

	private void RemoveLastHeart(bool animate = false)
	{
		GameObject lastHeart = _heartsIconsList[_heartsIconsList.Count - 1];
		if (lastHeart != null)
		{
			_heartsIconsList.Remove(lastHeart);
			Destroy(lastHeart);
		}
	}

	#endregion


	#region Event Handlers

	private void HeartAddedhandler()
	{
		AddHeartIcon(true);
	}

	private void HeartRemovedHandler()
	{
		RemoveLastHeart(true);
	}

	private void OutOfHeartsHandler()
	{

	}

	#endregion
}
