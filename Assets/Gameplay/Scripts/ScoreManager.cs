using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	#region Public Properties

	public Text ScoreLabel;

    public Text ScoreMultiplierLabel;

	#endregion


	#region private Properties

	private int _score;

	private ColorType _lastColorCollected;

	private int _comboCount;

    private int _scoreMultiplier = 1;

    #endregion


    // Use this for initialization
    void Start () {
	
		if (ScoreLabel != null)
		{
			ScoreLabel.text = _score.ToString();
		}
        if (ScoreMultiplierLabel != null)
        {
            ScoreMultiplierLabel.gameObject.SetActive(false);
        }
	}
	
	#region Public

	public void ResetScore()
	{
		_score = 0;
		if (ScoreLabel != null)
		{
			ScoreLabel.text = _score.ToString();
		}
	}

	public void CollectedChromie(ChromieController chromieController)
	{
		AddScore(1);
	}

	public void EndGame()
	{
		PlayerPrefs.SetInt("score", _score);
	}

    public void SetScoreMultiplier(int scoreMultiplierValue, float duration)
    {
        _scoreMultiplier = scoreMultiplierValue;
        DisplayScoreMultipliyerIndicator();
        StartCoroutine(ScoreMultiplierRoutine(duration));
    }


	#endregion


	#region Private
	 
	private void AddScore(int scoreToAdd)
	{
		_score += scoreToAdd * _scoreMultiplier;
		if (ScoreLabel != null)
		{
			ScoreLabel.text = _score.ToString();
			iTween.PunchScale(ScoreLabel.gameObject, iTween.Hash("time", 0.3f, "x", 1.3f, "y", 1.3f));
		}

	}

    private void DisplayScoreMultipliyerIndicator()
    {
        ScoreMultiplierLabel.gameObject.SetActive(true);
        ScoreMultiplierLabel.gameObject.transform.localScale = new Vector3(1,1,1);
        ScoreMultiplierLabel.text = "x" + _scoreMultiplier.ToString();
        iTween.PunchScale(ScoreMultiplierLabel.gameObject, iTween.Hash("time", 1.0f, "x", 1.2f, "y", 1.2f));
       
    }

    IEnumerator ScoreMultiplierRoutine(float duration)
    {
        yield return new WaitForSeconds(duration - (duration * 0.2f));

        float blinkCount = 10.0f;
        float delay = (duration * 0.2f) / blinkCount;
        Color color = ScoreMultiplierLabel.color;
       
        for (int i=0; i< (int)blinkCount; i++)
        {
            if (i % 2 == 0)
            {
                color.a = 0.1f;
            }
            else
            {
                color.a = 1.5f;
            }

            ScoreMultiplierLabel.color = color;
            yield return new WaitForSeconds(delay);
        }

        color.a = 1.5f;
        ScoreMultiplierLabel.color = color;

        yield return new WaitForSeconds(delay);

        iTween.ScaleTo(ScoreMultiplierLabel.gameObject, iTween.Hash("time", 0.5f, "x", 0.01f, "y", 0.01f));

        yield return new WaitForSeconds(0.5f);

        ScoreMultiplierLabel.gameObject.SetActive(false);

        _scoreMultiplier = 1;

        yield return null;
    }
	#endregion
}
