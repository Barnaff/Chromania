using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeepPlayingPopupController : PopupBaseController {

    #region Public

    public delegate void KeepPalyingDelegae();

    public event KeepPalyingDelegae OnKeepPlaying;

    #endregion


    #region Private Properties

    [SerializeField]
    private float _keepPlayingTimerDuration;

    [SerializeField]
    private Image _timeBarImage;

    private float _currentKeepPlayingTimeCount;

    private bool _isTimerRunning = false;

    [SerializeField]
    private Text _currencyLabel;

    #endregion

    #region Initialization

    void Start()
    {
        _keepPlayingTimerDuration = GameplaySettings.Instance.KeepPlayingTImerDuration;
        _currentKeepPlayingTimeCount = _keepPlayingTimerDuration;
        _isTimerRunning = true;
    }

    #endregion


    #region Update

    void Update()
    {
        if (_isTimerRunning)
        {
            _currentKeepPlayingTimeCount -= Time.deltaTime;
            if (_timeBarImage != null)
            {
                _timeBarImage.fillAmount = _currentKeepPlayingTimeCount / _keepPlayingTimerDuration;
            }

            if (_currentKeepPlayingTimeCount <= 0)
            {
                TimesUpOrCancel();
            }
        }

        if (_currencyLabel != null)
        {
            _currencyLabel.text = InventoryManager.Instance.Currency.ToString() + "C";
        }
    }

    #endregion


    #region Private

    private void TimesUpOrCancel()
    {
        ClosePopup();
    }

    #endregion


    #region Public - UI Interactions

    public void CancelButtonAction()
    {
        TimesUpOrCancel();
    }

    public void KeepPlayingButtonAction()
    {
        _isTimerRunning = false;
        ShopManager.Instance.Pay(100, (sucsess) =>
        {
            if (sucsess)
            {
                if (OnKeepPlaying != null)
                {
                    OnKeepPlaying();
                }
                OnCloseAction = null;
                ClosePopup();
            }
        });
    }

    #endregion
}
