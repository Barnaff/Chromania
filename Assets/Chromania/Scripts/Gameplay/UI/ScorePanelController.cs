using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private Text _scoreLabel;


    #endregion


    #region Initialization

    void Start ()
    {
        UpdateScoreLabel(0,0,false);
    }

    #endregion


    #region Public

    public void AddScore(int currentScore, int scoreToAdd)
    {
        UpdateScoreLabel(currentScore, scoreToAdd, true);
    }


    #endregion


    #region Private

    private void UpdateScoreLabel(int currentScore, int scoreToAdd, bool animated = true)
    {
        _scoreLabel.text = currentScore.ToString();
        if (animated)
        {
            iTween.PunchScale(_scoreLabel.gameObject, iTween.Hash("time", 0.5f, "amount", new Vector3(1.1f, 1.1f, 1.1f)));
        }
        else
        {
            _scoreLabel.text = "0";
        }
    }

    #endregion
}
