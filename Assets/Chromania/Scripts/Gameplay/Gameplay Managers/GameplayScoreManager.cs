using UnityEngine;
using System.Collections;

public class GameplayScoreManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private int _currentScore;

    [SerializeField]
    private int _currentScoreMultiplier = 1;

    [SerializeField]
    private int _currentComboMultiplier = 1;

    [SerializeField]
    private int _currentComboCount = 0;

    [SerializeField]
    private eChromieType _lastCollectedChromie;

    #endregion


    #region Public

    public void Init()
    {
        _currentScore = 0;
        _currentScoreMultiplier = 1;
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
        GameplayEventsDispatcher.Instance.OnChromieDropped += OnChromieDroppedHandler;
    }

    public void UpdateScoreMultiplier()
    {
        _currentScoreMultiplier = (int)GameplayBuffsManager.GetValue(eBuffType.ScoreMultiplier);
        GameplayEventsDispatcher.SendScoreMultiplierUpdate(_currentScoreMultiplier);
    }

    public void UpdateComboMultiplier()
    {
        _currentComboMultiplier = (int)GameplayBuffsManager.GetValue(eBuffType.ComboMultiplier);
    }

    #endregion

    #region Private 


    #endregion

    #region Events

    private void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        int scoreToAdd = 1;

        if (_lastCollectedChromie == chromieController.ChromieType)
        {
            _currentComboCount++;
        }
        else
        {
            _currentComboCount = 0;
        }

        scoreToAdd += (_currentComboCount * _currentComboMultiplier);

        scoreToAdd *= _currentScoreMultiplier;

        _currentScore += scoreToAdd;

        GameplayEventsDispatcher.Instance.ScoreUpdate(scoreToAdd, _currentScore, chromieController);

        _lastCollectedChromie = chromieController.ChromieType;
    }

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        _currentComboCount = 0;
        _lastCollectedChromie = eChromieType.None;
    }

    #endregion
}
