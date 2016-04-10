﻿using UnityEngine;
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

    private Vector3 _originalScale;

    #endregion


    #region private Properties

    private ChromieDataObject _colorDefenition;

    private Vector3 _baseScale;

    private float _scaleValue = 1.0f;

    private System.Action _completionAction;

    private Vector3 _whirlwindBaseScale;
    private Vector3 _glowBaseScale;

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
        
        ColorZoneType = colorType;

        IGameData gameDataLoader = ComponentFactory.GetAComponent<IGameData>();
        _colorDefenition = gameDataLoader.GameData.GetChromie(ColorZoneType);
        if (_colorDefenition != null)
        {
            Color overlayColor = _colorDefenition.ColorValue;
            overlayColor.a = 1.0f;
            if (Whirlwind != null)
            {
                Whirlwind.GetComponent<SpriteRenderer>().color = overlayColor;
                _whirlwindBaseScale = Whirlwind.transform.localScale;
            }
            if (Glow != null)
            {
                Glow.GetComponent<SpriteRenderer>().color = overlayColor;
                _glowBaseScale = Glow.transform.localScale;
            }
        }
        else
        {
            Debug.LogError("Cant get color defenition!");
        }

        _baseScale = this.gameObject.transform.localScale;
        
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

    public void SetForIntro()
    {
      //  this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        this.gameObject.SetActive(false);
        _scaleValue = 1.0f;
    }

    public void DisplayIntroAnimation(System.Action completionAction)
    {
        this.gameObject.SetActive(true);
        _completionAction = completionAction;
        iTween.ScaleFrom(this.gameObject, iTween.Hash("time", 1.5f, "scale", new Vector3(0,0,0), "easetype",
            iTween.EaseType.easeOutElastic, "oncomplete", "FinishedIntroAnimation", "oncompletetarget", this.gameObject));

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

        iTween.Stop(Whirlwind);
        iTween.Stop(Glow);

        Whirlwind.transform.localScale = _whirlwindBaseScale;
        Glow.transform.localScale = _glowBaseScale;

        yield return null;

        //  this.gameObject.transform.localScale = _baseScale * _scaleValue;

        float punchScaleValue = _whirlwindBaseScale.magnitude * 1.5f;
        iTween.PunchScale(Whirlwind, iTween.Hash("time", 1.0f, "amount", new Vector3(punchScaleValue, punchScaleValue, punchScaleValue)));
        iTween.PunchScale(Glow, iTween.Hash("time", 1.0f, "amount", new Vector3(punchScaleValue, punchScaleValue, punchScaleValue)));


        yield return null;
    }

    #endregion
}
