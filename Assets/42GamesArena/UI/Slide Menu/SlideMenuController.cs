using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlideMenuController : MonoBehaviour {

	#region Private Properties

	[SerializeField]
	private bool _isOpen = false;

	[SerializeField]
	private RectTransform _heightReferanceTransform;

	[SerializeField]
	private float _animationDuration;

	[SerializeField]
	private GameObject _toggleButton;

	private bool _isAnimating = false;

	private float _closeYValue;

	private float _openYValue;

	[SerializeField]
	private GameObject _menuPanel;

	[SerializeField]
	private GameObject _medalsPanel;

	[SerializeField]
	private GameObject _debugPanel;

	[SerializeField]
	private Text _coinsLabel;

	#endregion


	#region Initialization

	// Use this for initialization
	void Start () {

		//  todo - get the real canvas, dont use parent-parent
		Rect headerRect = RectTransformUtility.PixelAdjustRect(_heightReferanceTransform, this.transform.parent.parent.GetComponent<Canvas>());
		_closeYValue =  0 + headerRect.size.y;

		Vector3 newPos = _heightReferanceTransform.transform.TransformPoint( new Vector3(0,_closeYValue,0 ));
		_closeYValue = newPos.y - Screen.height;


		_openYValue = Screen.height * 0.8f;
		TogglePanelAction(false);

		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>() as IInventory;
		if (inventoryManager != null)
		{
			inventoryManager.OnInventoryUpdate += HandleOnInventoryUpdate;
			UpdateCoinsLabel(inventoryManager);
		}


	}

	void OnDisable()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>() as IInventory;
		if (inventoryManager != null)
		{
			inventoryManager.OnInventoryUpdate -= HandleOnInventoryUpdate;
		}

		IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
		if (backButtonManager != null)
		{
			backButtonManager.RemoveResponderFromBackButton(BackButtonAction);
		}
	}

	private void BackButtonAction()
	{
		if (_isOpen)
		{
			TogglePanelAction(true);
		}
	}

	
	#endregion


	#region Public
	
	public void TogglePanelAction(bool animated)
	{
		if (animated && _isAnimating)
		{
			return;
		}

		if (_isOpen)
		{
			ClosePanel(animated);

			IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
			if (backButtonManager != null)
			{
				backButtonManager.RemoveResponderFromBackButton(BackButtonAction);
			}
		}
		else
		{
			OpenPanel(animated);

			IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
			if (backButtonManager != null)
			{
				backButtonManager.RegisterToBackButton(BackButtonAction);
			}
		}
		_isOpen = !_isOpen;
	}

	public void DisplayCoinsShop()
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		if (popupsManager != null)
		{
			popupsManager.DisplayPopup<HeartsShopPopupController>();
		}
	}

	public void ToggleMenuPanel()
	{
		if (_menuPanel != null)
		{
			_menuPanel.SetActive(true);
		}
		if (_medalsPanel != null)
		{
			_medalsPanel.SetActive(false);
		}
		if (_debugPanel != null)
		{
			_debugPanel.SetActive(false);
		}
	}

	public void ToggleMedalsPanel()
	{
		if (_menuPanel != null)
		{
			_menuPanel.SetActive(false);
		}
		if (_medalsPanel != null)
		{
			_medalsPanel.SetActive(true);
		}
		if (_debugPanel != null)
		{
			_debugPanel.SetActive(false);
		}
	}

	public void ToggleDebugPanel()
	{
		if (_menuPanel != null)
		{
			_menuPanel.SetActive(false);
		}
		if (_medalsPanel != null)
		{
			_medalsPanel.SetActive(false);
		}
		if (_debugPanel != null)
		{
			_debugPanel.SetActive(true);
		}
	}

	public void ShopButtonAction()
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		if (popupsManager != null)
		{
			popupsManager.DisplayPopup<CoinsShopPopupController>();
		}
	}

	#endregion


	#region Private

	private void ClosePanel(bool animated)
	{
		if (animated)
		{
			iTween.MoveTo(this.gameObject, iTween.Hash("time", _animationDuration, "y", _closeYValue, "easetype", iTween.EaseType.easeOutBounce, "islocal", false, "oncomplete", "FinishedCloseAnimation", "oncompletetarget", this.gameObject));
			iTween.RotateTo(_toggleButton, iTween.Hash("time", _animationDuration * 0.5f, "z", 0.0f));
			_isAnimating = true;
		}
		else
		{
			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, _closeYValue, this.gameObject.transform.position.z);
			_toggleButton.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	private void OpenPanel(bool animated)
	{

		if (animated)
		{
			iTween.MoveTo(this.gameObject, iTween.Hash("time", _animationDuration * 0.5f, "y", _openYValue, "easetype", iTween.EaseType.easeOutBack, "islocal", false,  "oncomplete", "FinishedCloseAnimation", "oncompletetarget", this.gameObject));
			iTween.RotateTo(_toggleButton, iTween.Hash("time", _animationDuration * 0.5f, "z", 180.0f));
			_isAnimating = true;
		}
		else
		{
			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, _openYValue, this.gameObject.transform.position.z);
			_toggleButton.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));
		}
	}

	private void FinishedOpenAnimation()
	{
		_isAnimating = false;
	}

	private void FinishedCloseAnimation()
	{
		_isAnimating = false;
	}

	private void UpdateCoinsLabel(IInventory inventoryManager)
	{
		int coinsAmount = 0;
		if (inventoryManager != null)
		{
			coinsAmount = inventoryManager.CoinsCount;
		}
		_coinsLabel.text = coinsAmount.ToString("###,###,###,###,###,###,###");

	}

	#endregion


	#region Handle Events

	
	void HandleOnInventoryUpdate (IInventory inventoryManager)
	{
		UpdateCoinsLabel(inventoryManager);
	}

	#endregion
}
	