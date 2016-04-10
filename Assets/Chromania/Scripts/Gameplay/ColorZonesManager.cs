using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorZonesManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _colorZonePrefab;

    [SerializeField]
    private eChromieType[] _colorZonesTypes;

    [SerializeField]
    private Vector2[] _colorZonesPositions;

    private List<ColorZoneController> _colorZones = new List<ColorZoneController>();

    private float _colorZoneSclae = 1.0f;

    #endregion


    #region Public

    public void Init(eChromieType[] colorTypes = null)
	{
		if (colorTypes != null)
		{
			_colorZonesTypes = colorTypes;
		}
        _colorZones = new List<ColorZoneController>();
        for (int i=0; i< _colorZonesTypes.Length ; i++)
		{
            CreateColorZoneObject(_colorZonesTypes[i] , i);
		}
	}


    public Vector3 PositionForColorZone(int index)
    {
        Vector2 colorZonePosition = _colorZonesPositions[index];
        colorZonePosition.x *= Screen.width;
        colorZonePosition.y *= Screen.height;
        colorZonePosition = Camera.main.ScreenToWorldPoint(colorZonePosition);
        return colorZonePosition;
    }

    public GameObject CreateColorZoneObject(eChromieType color, int index, bool animated = true, System.Action completionAction = null)
	{
        GameObject colorZone = Lean.LeanPool.Spawn(_colorZonePrefab, PositionForColorZone(index), Quaternion.identity);
		ColorZoneController colorZoneController = colorZone.GetComponent<ColorZoneController>() as ColorZoneController;
		if (colorZoneController != null)
		{
			colorZoneController.SetColorZone(color);
            _colorZones.Add(colorZoneController);
            colorZoneController.SetColorZoneScale(_colorZoneSclae, false);
            if (animated)
            {
              //  colorZoneController.DisplayIntroAnimation(completionAction);
            }
        }

		return colorZone;
	}

    public void EnlargeColorZones(float enlargeValue)
    {
        _colorZoneSclae *= enlargeValue;

        foreach (ColorZoneController colorZone in _colorZones)
        {
            if (colorZone != null)
            {
                colorZone.SetColorZoneScale(_colorZoneSclae, true);
            }
        }
    }

    public ColorZoneController ColorZoneForColor(eChromieType chromieType)
    {
        foreach (ColorZoneController colorZone in _colorZones)
        {
            if (colorZone.ColorZoneType == chromieType)
            {
                return colorZone;
            }
        }
        return null;
    }

    #endregion



}
