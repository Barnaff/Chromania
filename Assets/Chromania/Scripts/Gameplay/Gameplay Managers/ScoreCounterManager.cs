using UnityEngine;
using System.Collections;

public class ScoreCounterManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private int _currentScore;

    [SerializeField]
    private int _currentCombo;

    [SerializeField]
    private eChromieType _lastCollectedChromieType;

    [SerializeField]
    private int _scoreMultiplier = 1;

    [SerializeField]
    private int _chromieBaseScoreValue = 1;

    [SerializeField]
    private float _baseCritValue = 0.5f;

    [SerializeField]
    private float _critValueModifier = 1.0f;

    [SerializeField]
    private ScorePanelController _scorePanelController;

    [SerializeField]
    private GameObject _scoreIndicatorPrefab;

    #endregion

    // Use this for initialization
    void Start ()
    {
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
	}
	
	

    public int Score
    {
        get
        {
            return _currentScore;
        }
    }

    private void OnChromieCollectedHandler(ChromieController chromieController)
    {
        if (chromieController != null)
        {
            if (_lastCollectedChromieType == chromieController.ChromieType)
            {
                _currentCombo++;
            }
            else
            {
                _lastCollectedChromieType = chromieController.ChromieType;
                _currentCombo = 0;
            }

            int scoreToAdd = _chromieBaseScoreValue;

            scoreToAdd += _currentCombo;

            _currentScore += scoreToAdd;

            _scorePanelController.AddScore(_currentScore, scoreToAdd);


            Vector3 position = Camera.main.WorldToScreenPoint(chromieController.transform.position);

            Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

            Vector3 newPosition = (position - center) * 0.1f;

            position = position - newPosition;

            StartCoroutine(DisplayScoreIndicator(position, scoreToAdd, _currentCombo, false));
        }
    }

    private IEnumerator DisplayScoreIndicator(Vector3 position, int score, int comboCount, bool isCrit)
    {
        GameObject scoreIndicator = Lean.LeanPool.Spawn(_scoreIndicatorPrefab, position, Quaternion.identity);

        scoreIndicator.transform.SetParent(_scorePanelController.transform);
        scoreIndicator.transform.localScale = new Vector3(1, 1, 1);

        ScoreDisplayIndicatorController scoreIndicatorController = scoreIndicator.GetComponent<ScoreDisplayIndicatorController>();
        if (scoreIndicatorController != null)
        {
            scoreIndicatorController.SetScore(score, comboCount);
        }

        yield return StartCoroutine(scoreIndicatorController.DisplayEnterAnimation());

        Lean.LeanPool.Despawn(scoreIndicator);
    }
}
