using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreManager : Kobapps.Singleton<HighScoreManager> {

    #region Private properties

    [SerializeField]
    private List<GameplayTrackingData> _highScoresList;

    private const string STORED_HIGH_SCORES_KEY = "storedHighscores";

    void Awake()
    {
        LoadHighScores();
    }

    #endregion

    public GameplayTrackingData GetHighScore(eGameplayMode gameplayMode)
    {
        foreach (GameplayTrackingData gameplayTrackingData in _highScoresList)
        {
            if (gameplayTrackingData.GameplayMode == gameplayMode)
            {
                return gameplayTrackingData;
            }
        }
        return null;
    }

    public void SendGametrackingData(GameplayTrackingData newGameplayTrackingData)
    {
        ServerRequestsManager.Instance.PostLeaderboardEntry(newGameplayTrackingData, () =>
        {

        });

        foreach (GameplayTrackingData gameplayTrackingData in _highScoresList)
        {
            if (newGameplayTrackingData.GameplayMode == gameplayTrackingData.GameplayMode)
            {
                if (newGameplayTrackingData.Score > gameplayTrackingData.Score)
                {
                    SaveNewgameplayTracking(newGameplayTrackingData);
                }
                else
                {
                   
                }
            }
        }
       
    }

    private void LoadHighScores()
    {
        if (PlayerPrefsUtil.HasKey(STORED_HIGH_SCORES_KEY))
        {
            _highScoresList = (List<GameplayTrackingData>)PlayerPrefsUtil.GetObject(STORED_HIGH_SCORES_KEY);
        }
        else
        {
            _highScoresList = new List<GameplayTrackingData>();
            

            foreach (eGameplayMode gameplayMode in System.Enum.GetValues(typeof(eGameplayMode)))
            {
                GameplayTrackingData gameplayTrackingData = new GameplayTrackingData();
                gameplayTrackingData.GameplayMode = gameplayMode;
                _highScoresList.Add(gameplayTrackingData);
            }
        }
    }

    private void SaveNewgameplayTracking(GameplayTrackingData gameplayTrackingData)
    {
        int replacedIndex = -1;
        for (int i=0; i< _highScoresList.Count; i++)
        {
            if (_highScoresList[i].GameplayMode == gameplayTrackingData.GameplayMode)
            {
                replacedIndex = i;
                break;
            }
        }

        if (replacedIndex > -1)
        {
            _highScoresList[replacedIndex] = gameplayTrackingData;
            PlayerPrefsUtil.SetObject(STORED_HIGH_SCORES_KEY, _highScoresList);
        }
    }
}
