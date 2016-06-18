using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameMessage : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Image _overlayImage;

    [SerializeField]
    private Image _messageImage;

    #endregion


    #region Initialization

    // Use this for initialization
    void Start ()
    {
       // this.gameObject.SetActive(false);
	}

    #endregion


    #region Public

    public void PlayMessageAnimation(System.Action completionAction)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(DisplayMessageCorutine(completionAction));
    }

    #endregion


    #region Private

    private IEnumerator DisplayMessageCorutine(System.Action completionAction)
    {
        _messageImage.gameObject.SetActive(false);
        _overlayImage.CrossFadeAlpha(0, 0, true);

        yield return null;

        _overlayImage.CrossFadeAlpha(1, 0.5f, true);

        yield return null;

        _messageImage.gameObject.SetActive(true);

        float animationDuration = 1.0f;

        iTween.PunchScale(_messageImage.gameObject, iTween.Hash("time", animationDuration, "amount", new Vector3(2,2,2)));

        yield return new WaitForSeconds(animationDuration);

        yield return new WaitForSeconds(1.5f);

        if (completionAction != null)
        {
            completionAction();
        }
    }

    #endregion

}
