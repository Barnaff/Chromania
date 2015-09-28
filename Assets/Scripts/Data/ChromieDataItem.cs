using UnityEngine;
using System.Collections;
using System.Xml;
using System;

[Serializable]
public class ChromieDataItem {

	#region Public Properties

	public int ChromieID;

	public ColorType ChromieColor;

	public string ChromieName;

	public Color ColorValue;

	public string ColorName;

	public string PassiveDescription;

	public string ActiveDescription;

    public ActivePowerupType ActivePowerup;

    public PassivePowerupType PassivePowerup;

    public bool UnlockedAtStart;

	public int UnlockPrice;

	public int UnlockWithAchievment;

	public string UnlockShopItem;

	public int UnlockAtLevel;

	public int InventorySlotId;

	public int CountForPowerup;

	public float PowerupDuration;

	public float PassivePowerupValue;

	public float ActivePowerupValue;

	public float CritValue;

	public float CritMultipier;

  

	#endregion


	#region Public 

	public override string ToString ()
	{
		return string.Format ("[ChromieDataItem]");
	}

	#endregion
}
