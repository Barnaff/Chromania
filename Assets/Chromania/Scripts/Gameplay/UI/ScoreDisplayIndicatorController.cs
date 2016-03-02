using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplayIndicatorController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _comboLabel;

    #endregion

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetScore(int score, int comboCount)
    {
        _scoreLabel.text = "+" + score.ToString();
        if (comboCount > 1)
        {
          //  _comboLabel.gameObject.SetActive(true);
           // _comboLabel.text = comboCount.ToString();
        }
        else
        {
          //  _comboLabel.gameObject.SetActive(false);
        }
    }

    public IEnumerator DisplayEnterAnimation()
    {
        _scoreLabel.CrossFadeAlpha(1.0f, 0, true);

        iTween.PunchScale(this.gameObject, iTween.Hash("time", 0.5f, "amount", new Vector3(1.2f, 1.2f, 1.2f)));
        iTween.MoveBy(this.gameObject, iTween.Hash("time", 1.5f, "y" , 20.0f));
        yield return new WaitForSeconds(0.5f);

        _scoreLabel.CrossFadeAlpha(0.0f, 0, true);

        yield return new WaitForSeconds(1.0f);
    }

    
}
