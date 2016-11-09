﻿using UnityEngine;
using System.Collections;

public class ColorZoneController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private ColorZoneCharacterController _colorZoneCharacter;

    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    private ChromieDefenition _overrideChromieDefenition = null;

    [SerializeField]
    private float _baseScale = 1f;

    #endregion


    #region Initiliaze

    void Start()
    {
        _overrideChromieDefenition = null;
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    }

    #endregion

    #region Public

    public void SetColorZone(ChromieDefenition chromieDefention)
    {
        _chromieDefenition = chromieDefention;
        SetColorZoneCharacter(_chromieDefenition);
    }

    public eChromieType Color
    {
        get
        {
            if (_overrideChromieDefenition != null)
            {
                return _overrideChromieDefenition.ChromieColor;
            }
            return _chromieDefenition.ChromieColor;
        }
    }

    public void SetOverrideColor(ChromieDefenition chromieDefention)
    {
        _overrideChromieDefenition = chromieDefention;
        if (_overrideChromieDefenition == null)
        {
            SetColorZoneCharacter(_chromieDefenition);
        }
        else
        {
            SetColorZoneCharacter(_overrideChromieDefenition);
        }
    }

    public void AddScaleFactor(float scaleFactorToAdd)
    {
        _baseScale += scaleFactorToAdd;
        this.transform.localScale = Vector3.one * _baseScale;
    }

    public void RemoveScaleFactor(float scaleFactorToRemove)
    {
        _baseScale -= scaleFactorToRemove;
        this.transform.localScale = Vector3.one * _baseScale;
    }

    #endregion

    #region Private 

    private void SetColorZoneCharacter(ChromieDefenition chromieDefention)
    {
        if (_colorZoneCharacter != null && _colorZoneCharacter.gameObject != null)
        {
            Destroy(_colorZoneCharacter.gameObject);
        }
        GameObject colorZoneCharacter = Instantiate(chromieDefention.ColorZonePrefab) as GameObject;
        colorZoneCharacter.transform.SetParent(this.transform);
        colorZoneCharacter.transform.localPosition = Vector3.zero;
        _colorZoneCharacter = colorZoneCharacter.GetComponent<ColorZoneCharacterController>();
       
    }

    #endregion


    #region Events

    public void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (colorZone == this)
        {
            //TODO: Display collection animation 
            _colorZoneCharacter.Collected();
            
        }
    }

    #endregion
}
