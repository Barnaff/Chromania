﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorZonesController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Vector2[] _colorZonesPosition;

    [SerializeField]
    private List<ColorZoneController> _colorZones;

    [SerializeField]
    private ColorZoneController _colorZoneControllerPrefab;

    #endregion

    #region Public

    public List<ColorZoneController> CreateColorZones(List<ChromieDefenition> selectedColors)
    {
        _colorZones = new List<ColorZoneController>();
        for (int i=0; i< selectedColors.Count; i++)
        {
            ColorZoneController colorZoneController = CreateColorZone(selectedColors[i], i);
            _colorZones.Add(colorZoneController);
            colorZoneController.gameObject.SetActive(false);
        }
        return _colorZones;
    }

    public void OverrideColorZonesColor(ChromieDefenition chromieDefenition)
    {
        foreach (ColorZoneController colorZone in _colorZones)
        {
            colorZone.SetOverrideColor(chromieDefenition);
        }
    }

    public void ResetColorZonesToOriginalColor()
    {
        foreach (ColorZoneController colorZone in _colorZones)
        {
            colorZone.SetOverrideColor(null);
        }
    }

    public void ActivateColorZone(int index)
    {
        _colorZones[index].DisplayIntroAndActivate();
    }

    public ColorZoneController GetColorZone(int index)
    {
        return _colorZones[index];
    }

    #endregion


    #region Private

    private Vector3 PositionForColorzoneAtIndex(int index)
    {
        Vector3 colorZonePosition = _colorZonesPosition[index];
        colorZonePosition.x *= Screen.width;
        colorZonePosition.y *= Screen.height;
        colorZonePosition = Camera.main.ScreenToWorldPoint(colorZonePosition);
        colorZonePosition.z = 0;
        return colorZonePosition;
    }

    private ColorZoneController CreateColorZone(ChromieDefenition chromieDefenition, int index)
    {
        ColorZoneController colorZoneController = Instantiate(_colorZoneControllerPrefab);
        colorZoneController.transform.SetParent(this.transform);
        colorZoneController.transform.position = PositionForColorzoneAtIndex(index);
        colorZoneController.transform.localScale = Vector3.one;
        colorZoneController.SetColorZone(chromieDefenition);
        return colorZoneController;
    }

    #endregion
}
