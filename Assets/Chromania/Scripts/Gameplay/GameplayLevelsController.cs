using UnityEngine;
using System.Collections;

public class GameplayLevelsController : MonoBehaviour {

    [SerializeField]
    private int _currentLevel = 0;

    [SerializeField]
    private int _collectedChromiezCount = 0;

    [SerializeField]
    private int _levelInterval = 5;

	// Use this for initialization
	void Start ()
    {
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHadnler;
    }

    #region Private

    private void LevelUp()
    {
        _currentLevel++;
        GameplayEventsDispatcher.SendLevelUpdate(_currentLevel);
    }

    #endregion

    #region Events

    private void OnChromieCollectedHadnler(ChromieController chromieController, ColorZoneController colorZone)
    {
        _collectedChromiezCount++;

        if (_collectedChromiezCount % _levelInterval == 0)
        {
            LevelUp();
        }
    }

    #endregion
}
