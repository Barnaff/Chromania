using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class GameplayGameOverManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private int _baseNumberOfKeepPlaying;

    [SerializeField]
    private int _numberOfKeepPlayingUsed;

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    [SerializeField]
    private bool _isGameOver;

    #endregion

    #region Public

    public void Init(GameplayTrackingData gameplayTrackingData)
    {
        _baseNumberOfKeepPlaying = GameplaySettings.Instance.NumberOfKeepPlaying;

        _gameplayTrackingData = gameplayTrackingData;
        GameplayEventsDispatcher.Instance.OnOutOfLives += OnOutOfLivesHandler;
        GameplayEventsDispatcher.Instance.OnTimeUp += OnTimeUpHandler;
    }

    #endregion

    #region Events

    private void OnOutOfLivesHandler()
    {
        if (!_isGameOver)
        {
            Timing.RunCoroutine(GameOverSequance());
        }
    }

    private void OnTimeUpHandler()
    {
        if (!_isGameOver)
        {
            Timing.RunCoroutine(GameOverSequance());
        }
    }

    private void OnKeepPlayingHandler()
    {
        _isGameOver = false;
        _numberOfKeepPlayingUsed++;
        PopupsManager.Instance.ClosePopup<GameOverMessagePopup>();
        PopupsManager.Instance.ClosePopup<TimesUpMessagePopupController>();

        GameplayEventsDispatcher.SendKeepPlaying();
    }

    private void OnKeepPlayingTimeUpOrCancelhandler()
    {
        _gameplayTrackingData.CollectedCurrency += (int)(_gameplayTrackingData.Score * 0.1f * GameplayBuffsManager.GetValue(eBuffType.CurrencyCollectedMultiplier));

        FlowManager.Instance.GameOver(_gameplayTrackingData);
    }

    #endregion


    #region Private

    private IEnumerator<float> GameOverSequance()
    {
        GameplayEventsDispatcher.SendGameOver();
        _isGameOver = true;

        switch (GameSetupManager.Instance.SelectedPlayMode)
        {
            case eGameplayMode.Classic:
                {
                    PopupsManager.Instance.DisplayPopup<GameOverMessagePopup>();
                    break;
                }
            case eGameplayMode.Rush:
                {
                    PopupsManager.Instance.DisplayPopup<TimesUpMessagePopupController>();
                    break;
                }
            case eGameplayMode.Default:
            default:
                {
                    break;
                }
        }

        yield return Timing.WaitForSeconds(3.5f);

        if (_numberOfKeepPlayingUsed < _baseNumberOfKeepPlaying)
        {
            KeepPlayingPopupController keepPlayingPopup = PopupsManager.Instance.DisplayPopup<KeepPlayingPopupController>();
            keepPlayingPopup.OnKeepPlaying += OnKeepPlayingHandler;
            keepPlayingPopup.OnCloseAction += OnKeepPlayingTimeUpOrCancelhandler;
        }
        else
        {
            OnKeepPlayingTimeUpOrCancelhandler();
        }
    }

    #endregion

}
