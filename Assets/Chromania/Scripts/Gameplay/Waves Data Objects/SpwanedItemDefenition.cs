using System.Collections.Generic;
using UnityEngine;

public enum eSpwanedColorType
{
	RandomCorner,
	BottomLeft,
	TopLeft,
	TopRight,
	BottomRight,
}

public class SpwanedItemDefenition  {

	#region Public 

	public int ID;
	public Vector2 ForceVector;
	public float XPosition;
	public eSpwanedColorType SpwanedColor;

	#endregion


	#region Public
	
    
	public SpwanedItemDefenition(JSONObject jsonObject)
	{
		ID = int.Parse(jsonObject["id"].ToString());
		string[] forceVectorStringArray = jsonObject["forceVector"].ToString().Replace("\"", "").Split(","[0]);
		ForceVector = new Vector2(float.Parse(forceVectorStringArray[0]), float.Parse(forceVectorStringArray[1]));
		XPosition = float.Parse(jsonObject["xPosition"].ToString());
		SpwanedColor = (eSpwanedColorType)int.Parse(jsonObject["colorType"].ToString());
	}
	

	#endregion
}
