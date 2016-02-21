using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private int _currentScore;

    #endregion


    #region Initialization

    void Start ()
    {
        _currentScore = 0;
        UpdateScoreLabel(false);
    }

    #endregion


    #region Public

    public void AddScore(int scoreToAdd)
    {
        _currentScore += scoreToAdd;
        UpdateScoreLabel(true);
    }

    #endregion


    #region Private

    private void UpdateScoreLabel(bool animated)
    {
        _scoreLabel.text = _currentScore.ToString();
        if (animated)
        {
            iTween.PunchScale(_scoreLabel.gameObject, iTween.Hash("time", 0.5f, "amount", new Vector3(1.1f, 1.1f, 1.1f)));
        }
    }

    #endregion
}
