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

    #endregion

    #region Initialization

    void Start()
    {
        _currentKeepPlayingTimeCount = _keepPlayingTimerDuration;
    }

    #endregion


    #region Update

    void Update()
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
        if (OnKeepPlaying != null)
        {
            OnKeepPlaying();
        }
        OnCloseAction = null;
        ClosePopup();
    }

    #endregion
}
