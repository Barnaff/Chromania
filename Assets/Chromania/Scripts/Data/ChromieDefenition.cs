﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChromieDefenition
{
    #region Public Properties

    public int ChromieID;

    public eChromieType ChromieColor;

    public string ChromieName;

    public Color ColorValue;

    public string ColorName;

    public string PassiveDescription;

    public string ActiveDescription;

    public ePowerups.Active ActivePowerup;

    public ePowerups.Passive PassivePowerup;

    public int CountForPowerup;

    public float PowerupDuration;

    public float PassivePowerupValue;

    public float ActivePowerupValue;

    public float CritValue;

    public float CritMultipier;

    [Header("Resources")]
    public Sprite ChromieSprite;

    public GameObject CharacterPrefab;

    public GameObject ColorZonePrefab;

    #endregion
}
