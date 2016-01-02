using UnityEngine;
using System.Collections;

public class ColorZoneController : MonoBehaviour {

    #region Public Properties

    /// <summary>
    /// The type of the color zone.
    /// </summary>
    public eChromieType ColorZoneType;

    /// <summary>
    /// The whirlwind.
    /// </summary>
    public GameObject Whirlwind;

    /// <summary>
    /// The glow.
    /// </summary>
    public GameObject Glow;

    /// <summary>
    /// The rotation speed.
    /// </summary>
    public float RotationSpeed = 20.0f;

    #endregion


    #region private Properties

    private ChromieDataObject _colorDefenition;

    private Vector3 _baseScale;

    private float _scaleValue = 1.0f;

    private System.Action _completionAction;

    #endregion


    #region Initialize

    // Use this for initialization
    void Start()
    {

    }

    #endregion


    #region Update

    // Update is called once per frame
    void Update()
    {
        Whirlwind.transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
    }

    #endregion


    #region Public

    public void SetColorZone(eChromieType colorType)
    {
        /*
        ColorZoneType = colorType;

        IGameDataLoader gameDataLoader = ComponentsFactory.GetAComponent<IGameDataLoader>() as IGameDataLoader;
        _colorDefenition = gameDataLoader.GetGameData().GetChromie(ColorZoneType);
        if (_colorDefenition != null)
        {
            Color overlayColor = _colorDefenition.ColorValue;
            overlayColor.a = 1.0f;
            if (Whirlwind != null)
            {
                Whirlwind.GetComponent<SpriteRenderer>().color = overlayColor;
            }
            if (Glow != null)
            {
                Glow.GetComponent<SpriteRenderer>().color = overlayColor;
            }
        }
        else
        {
            Debug.LogError("Cant get color defenition!");
        }

        _baseScale = this.gameObject.transform.localScale;
        */
    }

    public void CollectChromie(ChromieController chromieController)
    {
        StartCoroutine(DisplayCollectAnimation());
    }

    public void SetColorZoneScale(float scaleValue, bool animated = false)
    {
        _scaleValue = scaleValue;
        if (animated)
        {
            StartCoroutine(DisplayCollectAnimation());
        }
    }

    public void DisplayIntroAnimation(System.Action completionAction)
    {
        _completionAction = completionAction;
        iTween.PunchScale(this.gameObject, iTween.Hash("time", 0.5f, "x", _scaleValue, "y", _scaleValue, "z", _scaleValue, "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "FinishedIntroAnimation", "oncompletetarget", this.gameObject));

    }

    #endregion


    #region Private

    private void FinishedIntroAnimation()
    {
        if (_completionAction != null)
        {
            _completionAction();
            _completionAction = null;
        }
    }

    private IEnumerator DisplayCollectAnimation()
    {
        iTween.Stop(this.gameObject);
        yield return new WaitForEndOfFrame();

        this.gameObject.transform.localScale = _baseScale * _scaleValue;
        iTween.PunchScale(this.gameObject, iTween.Hash("time", 1.0f, "x", 4.0f, "y", 4.0f, "z", 4.0f));

        yield return null;
    }

    #endregion
}
