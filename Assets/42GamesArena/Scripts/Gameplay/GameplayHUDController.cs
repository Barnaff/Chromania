using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void HUDPauseDelegate();
public delegate void HUDKeepPlayingDelegate();
public delegate void HUDFinishPlayingDelegate();
public delegate void HUDRestartGameDelegate();
public delegate void HUDQuitGameDelegate();
public delegate void HUDResumeGameDelegate();

public class GameplayHUDController : MonoBehaviour {

	#region Public Properties

	public HUDPauseDelegate OnHUDPause;

	public HUDKeepPlayingDelegate OnHUDKeepPlaying;

	public HUDFinishPlayingDelegate OnHUDFinishPlaying;

	public HUDResumeGameDelegate OnHUDResume;

	public HUDRestartGameDelegate OnHUDRestartGame;

	public HUDQuitGameDelegate OnHUDQuitGame;

	#endregion


	#region Private

	[SerializeField]
	private Text _scoreLabel; 

	[SerializeField]
	private GameObject _keepPlayingPanel;

	[SerializeField]
	private GameObject _pausePanel;

	[SerializeField]
	private GameObject _pauseButton;

	[SerializeField]
	private GameObject _coinsPanel;

	private bool _enableCoinsPanel = false;

	[SerializeField]
	private Text _coinsAmountLabel;

	[SerializeField]
	private GameObject _coinsEffectPrefab;

	private GameObject _coinsEffect;

	#endregion


	// Use this for initialization
	void Start () 
	{
		if (_scoreLabel != null)
		{
			_scoreLabel.text = "0";
		}

		if (_pauseButton != null)
		{
			_pauseButton.SetActive(true);
		}

		_coinsPanel.SetActive(_enableCoinsPanel);
		if (_enableCoinsPanel)
		{
			_coinsAmountLabel.text = GameplayWrapper.GetCoinsCount().ToString("###,###,###,###,###,###,###");

			if (_pauseButton != null)
			{
				_pauseButton.SetActive(false);
			}

			if (_scoreLabel != null)
			{
				_scoreLabel.gameObject.SetActive(false);
			}

			IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>() as IInventory;
			if (inventoryManager != null)
			{
				inventoryManager.OnInventoryUpdate += HandleOnInventoryUpdate;
			}

			if (_coinsEffectPrefab != null)
			{
				_coinsEffect = Instantiate(_coinsEffectPrefab) as GameObject;
				_coinsEffect.transform.position = new Vector3(-1000,1000,0);
				_coinsEffect.SetActive(false);
			}
		}
	}

	void OnDisable()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>() as IInventory;
		if (inventoryManager != null)
		{
			inventoryManager.OnInventoryUpdate -= HandleOnInventoryUpdate;
		}
	}

	#region User Interaction

	public void PauseButtonAction()
	{
		if (OnHUDPause != null)
		{
			OnHUDPause();
		}

		if (_pauseButton != null)
		{
			_pauseButton.SetActive(false);
		}
	}

	public void ResumeGameAction()
	{
		if (OnHUDResume != null)
		{
			OnHUDResume();
		}

		if (_pauseButton != null)
		{
			_pauseButton.SetActive(true);
		}
	}

	public void QuitButtonAction()
	{
		if (OnHUDQuitGame != null)
		{
			OnHUDQuitGame();
		}
	}

	public void RestartGameButtonAction()
	{
		if (OnHUDRestartGame != null)
		{
			OnHUDRestartGame();
		}
	}

	public void KeepPlayingButtonAction()
	{
		if (OnHUDKeepPlaying != null)
		{
			OnHUDKeepPlaying();
		}
	}

	public void GiveUpButtonAction()
	{
		if (OnHUDFinishPlaying != null)
		{
			OnHUDFinishPlaying();
		}
	}

	public void CoinsShopButtonAction()
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		if (popupsManager != null)
		{
			popupsManager.DisplayPopup<CoinsShopPopupController>();
		}
	}

	#endregion


	#region Public

	public void UpdateScore(int newScore)
	{
		if (_scoreLabel != null)
		{
			_scoreLabel.text = newScore.ToString();
		}
	}

	public void DisplayPausePanel()
	{
		if (_pausePanel != null)
		{
			_pausePanel.SetActive(true);
		}
	}

	public void HidePausePanel()
	{
		if (_pausePanel != null)
		{
			_pausePanel.SetActive(false);
		}
	}

	public void DisplayKeepPlayingPanel()
	{
		if (_keepPlayingPanel != null)
		{
			_keepPlayingPanel.SetActive(true);
		}
	}

	public void HideKeepPlayingPanel()
	{
		if (_keepPlayingPanel != null)
		{
			_keepPlayingPanel.SetActive(false);
		}
	}

	public void EnableCoinsPanel()
	{
		_enableCoinsPanel = true;
	}

	private void DisplayCoinsDropAnimationEffect()
	{
		StartCoroutine(DisplayCoinDropEffectCorutine());
	}

	IEnumerator DisplayCoinDropEffectCorutine()
	{
		if (_coinsEffect != null)
		{
			_coinsEffect.SetActive(true);

			yield return new WaitForSeconds(2.0f);

			_coinsEffect.SetActive(false);
		}
	}

	#endregion


	#region Event Handlers
		
	void HandleOnInventoryUpdate (IInventory inventoryManager)
	{
		if (inventoryManager != null && _enableCoinsPanel)
		{
			int currenctCoinsAmount = int.Parse(_coinsAmountLabel.text.Replace(",", ""));
			if (currenctCoinsAmount < inventoryManager.CoinsCount)
			{
				DisplayCoinsDropAnimationEffect();
			}
			RunningNumbersUtil.RunNumbers(_coinsAmountLabel, inventoryManager.CoinsCount, 1.5f, "###,###,###,###,###,###,###", null);
		}
	}
	
	#endregion

}
