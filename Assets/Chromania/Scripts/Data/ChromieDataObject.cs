using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChromieDataObject  {

    #region Public Properties

    public int ChromieID;

    public eChromieType ChromieColor;

    public string ChromieName;

    public Color ColorValue;

    public string ColorName;

    public string PassiveDescription;

    public string ActiveDescription;

   // public eActivePowerup ActivePowerup;

   // public ePassivePowerup PassivePowerup;

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
}
