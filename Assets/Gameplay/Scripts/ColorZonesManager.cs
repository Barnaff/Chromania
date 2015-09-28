using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorZonesManager : MonoBehaviour {

	public GameObject ColorZonePrefab;

	public ColorType[] ColorZonesTypes;

	public Vector2[] ColorZonesPositions;

    private List<ColorZoneController> _colorZones = new List<ColorZoneController>();

    private float _colorZoneSclae = 1.0f;

	// Use this for initialization
	void Start () {

		IGameSetup gameSetupmanager = ComponentsFactory.GetAComponent<IGameSetup>() as IGameSetup;
		ColorZonesTypes = gameSetupmanager.SelectedColors;

		//SetColorZones();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetColorZones(ColorType[] colorTypes = null)
	{
		if (colorTypes != null)
		{
			ColorZonesTypes = colorTypes;
		}
        _colorZones = new List<ColorZoneController>();
        for (int i=0; i< ColorZonesTypes.Length ; i++)
		{
            CreateColorZoneObject(ColorZonesTypes[i] , i);
		}
	}


    public Vector3 PositionForColorZone(int index)
    {
        Vector2 colorZonePosition = ColorZonesPositions[index];
        colorZonePosition.x *= Screen.width;
        colorZonePosition.y *= Screen.height;
        colorZonePosition = Camera.main.ScreenToWorldPoint(colorZonePosition);
        return colorZonePosition;
    }

    public GameObject CreateColorZoneObject(ColorType color, int index, bool animated = true, System.Action completionAction = null)
	{
        GameObject colorZone = (GameObject)Instantiate(ColorZonePrefab, PositionForColorZone(index), Quaternion.identity);
		ColorZoneController colorZoneController = colorZone.GetComponent<ColorZoneController>() as ColorZoneController;
		if (colorZoneController != null)
		{
			colorZoneController.SetColorZone(color);
            _colorZones.Add(colorZoneController);
            colorZoneController.SetColorZoneScale(_colorZoneSclae, false);
            if (animated)
            {
                colorZoneController.DisplayIntroAnimation(completionAction);
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


    


}
